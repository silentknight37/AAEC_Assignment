output "ecr_repository_urls" {
  description = "Map of service -> ECR repo URL"
  value = {
    for k, r in aws_ecr_repository.repos : k => r.repository_url
  }
}

output "ecs_cluster_name" {
  value = aws_ecs_cluster.this.name
}

output "services" {
  value = var.services
}
