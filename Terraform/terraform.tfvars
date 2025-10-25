aws_region       = "ap-southeast-1"
ecs_cluster_name = "skillbridge-cluster"

services = [
  "MentorService",
  "MenteeService",
  "BookingService",
  "MessagingService",
  "PaymentService",
  "CodeReviewService"
]

service_ports = {
  MentorService     = 8081
    MenteeService     = 8082
    BookingService    = 8083
    MessagingService  = 8084
    PaymentService    = 8085
    CodeReviewService = 8086
}

# (Optional) pin image tags per service; else 'latest' is used
image_tags = {
  CodeReviewService = "latest"
}

desired_counts = {
  MentorService     = 1
  MenteeService     = 1
  BookingService    = 1
  MessagingService    = 1
  PaymentService    = 1
  CodeReviewService = 1
}
