terraform {
  required_version = ">= 1.6.0"
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

data "aws_caller_identity" "current" {}
data "aws_region" "current" {}

# Get the default VPC dynamically
data "aws_vpcs" "all" {}

data "aws_vpc" "this" {
  id = data.aws_vpcs.all.ids[0]
}

# Get all subnets in that VPC
data "aws_subnets" "default_vpc_subnets" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.this.id]
  }
}

# ----------------------------------------------------
# 1) ECR Repositories (one per microservice)
# ----------------------------------------------------
resource "aws_ecr_repository" "repos" {
  for_each = toset(var.services)
  name     = lower(each.value)

  image_scanning_configuration {
    scan_on_push = true
  }

  encryption_configuration {
    encryption_type = "AES256"
  }

  tags = {
    Service = each.value
    Stack   = "skillbridge"
  }
}

# ----------------------------------------------------
# 2) ECS Cluster
# ----------------------------------------------------
resource "aws_ecs_cluster" "this" {
  name = var.ecs_cluster_name

  setting {
    name  = "containerInsights"
    value = "enabled"
  }

  tags = {
    Stack = "skillbridge"
  }
}

# ----------------------------------------------------
# 3) IAM Role for ECS Task Execution
# ----------------------------------------------------
data "aws_iam_policy_document" "ecs_tasks_assume" {
  statement {
    effect = "Allow"
    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
    actions = ["sts:AssumeRole"]
  }
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name               = "ecsTaskExecutionRole"
  assume_role_policy = data.aws_iam_policy_document.ecs_tasks_assume.json
  tags = {
    Stack = "skillbridge"
  }
}

resource "aws_iam_role_policy_attachment" "ecs_exec" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_iam_role_policy_attachment" "cw_logs" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/CloudWatchLogsFullAccess"
}

resource "aws_iam_role_policy_attachment" "ddb" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

# ----------------------------------------------------
# 4) Security Group
# ----------------------------------------------------
resource "aws_security_group" "ecs_services" {
  name        = "skillbridge-ecs-sg"
  description = "Allow inbound ports for SkillBridge microservices"
  vpc_id      = data.aws_vpc.this.id

  dynamic "ingress" {
    for_each = var.service_ports
    content {
      description = "Allow ${ingress.key}"
      from_port   = ingress.value
      to_port     = ingress.value
      protocol    = "tcp"
      cidr_blocks = ["0.0.0.0/0"]
      ipv6_cidr_blocks = ["::/0"]
    }
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Stack = "skillbridge"
  }
}

# ----------------------------------------------------
# 5) CloudWatch Log Groups
# ----------------------------------------------------
resource "aws_cloudwatch_log_group" "service" {
  for_each          = toset(var.services)
  name              = "/ecs/${each.value}"
  retention_in_days = 14
  tags = {
    Service = each.value
    Stack   = "skillbridge"
  }
}

# ----------------------------------------------------
# 6) ECS Task Definitions (Fargate)
# ----------------------------------------------------
locals {
  container_definitions = {
    for svc in var.services :
    svc => jsonencode([
      {
        name  = svc
        image = "${aws_ecr_repository.repos[svc].repository_url}:${lookup(var.image_tags, svc, "latest")}"
        essential    = true
        portMappings = [
          {
            containerPort = lookup(var.service_ports, svc, 8080)
            hostPort      = lookup(var.service_ports, svc, 8080)
            protocol      = "tcp"
          }
        ]
        environment = [
          { name = "ASPNETCORE_ENVIRONMENT", value = "Production" },
          { name = "AWS_REGION", value = var.aws_region },
          { name = "ASPNETCORE_URLS", value = "http://+:${lookup(var.service_ports, svc, 8080)}" }
        ]
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-group         = "/ecs/${svc}"
            awslogs-region        = var.aws_region
            awslogs-stream-prefix = "ecs"
          }
        }
      }
    ])
  }
}

resource "aws_ecs_task_definition" "task" {
  for_each                 = toset(var.services)
  family                   = "${each.value}-task"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.task_cpu
  memory                   = var.task_memory
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  container_definitions    = local.container_definitions[each.value]

  tags = {
    Service = each.value
    Stack   = "skillbridge"
  }
}

# ----------------------------------------------------
# 7) ECS Services
# ----------------------------------------------------
resource "aws_ecs_service" "svc" {
  for_each        = toset(var.services)
  name            = each.value
  cluster         = aws_ecs_cluster.this.id
  task_definition = aws_ecs_task_definition.task[each.value].arn
  desired_count   = lookup(var.desired_counts, each.value, 1)
  launch_type     = "FARGATE"
  force_new_deployment = true

  network_configuration {
    subnets          = data.aws_subnets.default_vpc_subnets.ids
    security_groups  = [aws_security_group.ecs_services.id]
    assign_public_ip = true
  }

  lifecycle {
    ignore_changes = [task_definition]
  }

  depends_on = [
    aws_cloudwatch_log_group.service
  ]

  tags = {
    Service = each.value
    Stack   = "skillbridge"
  }
}
