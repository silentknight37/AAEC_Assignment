using Amazon.DynamoDBv2.DataModel;

namespace CodeReviewService.Models
{
    [DynamoDBTable("SkillBridge_CodeReviews")]
    public class CodeReview
    {
        [DynamoDBHashKey]
        public string ReviewId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string MenteeId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string MentorId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string CodeSnippet { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Language { get; set; } = "C#";

        [DynamoDBProperty]
        public string Feedback { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Status { get; set; } = "Pending"; // Pending, Reviewed

        [DynamoDBProperty]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [DynamoDBProperty]
        public DateTime? ReviewedAt { get; set; }
    }
}
