output "ecs_cluster_name" {
  description = "The ECS cluster name"
  value       = aws_ecs_cluster.this.name
}

output "ecs_service_names" {
  description = "List of ECS service names"
  value       = [for svc in aws_ecs_service.service : svc.name]
}

output "ecs_task_definitions" {
  description = "Task definitions ARNs"
  value       = [for td in aws_ecs_task_definition.service : td.arn]
}

output "security_group_id" {
  description = "Security group ID for ECS services"
  value       = aws_security_group.ecs_services.id
}

output "log_groups" {
  description = "CloudWatch Log Groups for all services"
  value       = [for lg in aws_cloudwatch_log_group.service : lg.name]
}
