terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

# --- Get Default VPC and Subnets ---
data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# --- ECS Cluster ---
resource "aws_ecs_cluster" "this" {
  name = "skillbridge-cluster"
}

# --- IAM Role for ECS Task Execution ---
resource "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_role_policy" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

# --- Security Group ---
resource "aws_security_group" "ecs_services" {
  name        = "skillbridge-ecs-sg"
  description = "Allow ECS service traffic"
  vpc_id      = data.aws_vpc.default.id

  ingress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# --- Local Map of Services ---
locals {
  services = {
    MentorService     = 8080
    MenteeService     = 8080
    BookingService    = 8080
    MessagingService  = 8080
    PaymentService    = 8080
    CodeReviewService = 8080
  }
}

# --- ECS Task Definitions for Each Microservice ---
resource "aws_ecs_task_definition" "service" {
  for_each = local.services

  family                   = each.key
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn

  container_definitions = jsonencode([
    {
      name      = each.key
      image     = "${var.account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/${lower(each.key)}:latest"
      essential = true
      portMappings = [
        {
          containerPort = each.value
          hostPort      = each.value
          protocol      = "tcp"
        }
      ]
      environment = [
        { name = "ASPNETCORE_URLS",     value = "http://0.0.0.0:8080" },
        { name = "ASPNETCORE_PATHBASE", value = "/${lower(each.key)}" }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = "/ecs/${each.key}"
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "ecs"
        }
      }
    }
  ])
}

# --- CloudWatch Log Groups ---
resource "aws_cloudwatch_log_group" "service" {
  for_each          = local.services
  name              = "/ecs/${each.key}"
  retention_in_days = 7
}

# --- Application Load Balancer (ALB) ---
resource "aws_lb" "ecs_alb" {
  name               = "skillbridge-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.ecs_services.id]
  subnets            = data.aws_subnets.default.ids
}

# --- Target Groups (unique names) ---
resource "aws_lb_target_group" "service_tg" {
  for_each = local.services

  name        = "${lower(each.key)}-${substr(md5(each.key), 0, 5)}"
  port        = 8080
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = data.aws_vpc.default.id

  health_check {
    path                = "/${lower(each.key)}/api/health"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
    matcher             = "200-499"
  }

  lifecycle {
    create_before_destroy = true
  }
}

# --- ALB Listener (HTTP) ---
resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.ecs_alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "fixed-response"
    fixed_response {
      content_type = "text/plain"
      message_body = "SkillBridge ALB running"
      status_code  = "200"
    }
  }
}

# --- Listener Rules (path-based routing) ---
resource "aws_lb_listener_rule" "service_routes" {
  for_each = local.services

  listener_arn = aws_lb_listener.http.arn
  priority     = 100 + index(keys(local.services), each.key)

  action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.service_tg[each.key].arn
  }

  condition {
    path_pattern {
      values = ["/${lower(each.key)}/*", "/${lower(each.key)}"]
    }
  }

  lifecycle {
    create_before_destroy = true
  }
}

# --- ECS Services ---
resource "aws_ecs_service" "service" {
  for_each = local.services

  name            = each.key
  cluster         = aws_ecs_cluster.this.id
  task_definition = aws_ecs_task_definition.service[each.key].arn
  desired_count   = 1
  launch_type     = "FARGATE"

  load_balancer {
    target_group_arn = aws_lb_target_group.service_tg[each.key].arn
    container_name   = each.key
    container_port   = each.value
  }

  network_configuration {
    subnets          = data.aws_subnets.default.ids
    security_groups  = [aws_security_group.ecs_services.id]
    assign_public_ip = true
  }

  depends_on = [
    aws_lb_listener_rule.service_routes,
    aws_cloudwatch_log_group.service,
    aws_iam_role_policy_attachment.ecs_task_execution_role_policy
  ]
}

# --- Output ---
output "alb_dns_name" {
  value       = aws_lb.ecs_alb.dns_name
  description = "Public DNS of the Application Load Balancer"
}
