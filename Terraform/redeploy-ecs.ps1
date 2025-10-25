# ------------------------------------------
# Redeploy all ECS services with latest images
# Author: Shanaka Deployment Helper 😊
# ------------------------------------------

# Set your AWS Region and ECS Cluster name
$region = "ap-southeast-1"
$cluster = "skillbridge-cluster"

# List of all ECS services
$services = @(
    "MentorService",
    "MenteeService",
    "BookingService",
    "MessagingService",
    "PaymentService",
    "CodeReviewService"
)

Write-Host "🚀 Starting ECS redeployments..." -ForegroundColor Cyan
Write-Host "Cluster: $cluster" -ForegroundColor Yellow
Write-Host "Region : $region" -ForegroundColor Yellow
Write-Host ""

foreach ($service in $services) {
    Write-Host "🔄 Redeploying service: $service ..." -ForegroundColor Cyan
    try {
        aws ecs update-service `
            --cluster $cluster `
            --service $service `
            --force-new-deployment `
            --region $region | Out-Null

        Write-Host "✅ Successfully triggered new deployment for $service" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Failed to redeploy $service" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor DarkRed
    }

    Start-Sleep -Seconds 3
}

Write-Host ""
Write-Host "🎉 All redeployments triggered successfully!" -ForegroundColor Green
Write-Host "You can monitor progress in the AWS ECS console or via:" -ForegroundColor Cyan
Write-Host "aws ecs describe-services --cluster $cluster --region $region --services <ServiceName>" -ForegroundColor Yellow
