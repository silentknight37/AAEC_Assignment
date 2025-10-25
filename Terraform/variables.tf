variable "aws_region" {
  default = "ap-southeast-1"
}

variable "ecs_cluster_name" {
  default = "skillbridge-cluster"
}

variable "services" {
  default = [
    "MentorService",
    "MenteeService",
    "BookingService",
    "MessagingService",
    "PaymentService",
    "CodeReviewService"
  ]
}

variable "service_ports" {
  description = "Map of service -> port"
  type        = map(number)
  default = {
    MentorService     = 8081
    MenteeService     = 8082
    BookingService    = 8083
    MessagingService  = 8084
    PaymentService    = 8085
    CodeReviewService = 8086
  }
}
# Per-service desired counts
variable "desired_counts" {
  description = "Map of service -> desired task count"
  type        = map(number)
  default     = {}
}

# Per-service image tag (default 'latest')
variable "image_tags" {
  description = "Map of service -> image tag"
  type        = map(string)
  default     = {}
}

# Extra env vars per service
variable "extra_env" {
  description = "Map of service -> map of env vars"
  type        = map(map(string))
  default     = {}
}

# Task sizing
variable "task_cpu" {
  description = "Fargate CPU units (256=0.25vCPU,512=0.5vCPU,1024=1vCPU)"
  type        = string
  default     = "512"
}

variable "task_memory" {
  description = "Fargate memory (MB)"
  type        = string
  default     = "1024"
}