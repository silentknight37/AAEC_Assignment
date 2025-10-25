using Amazon.DynamoDBv2.DataModel;

namespace MessagingService.Models
{
    [DynamoDBTable("SkillBridge_Messages")]
    public class Message
    {
        [DynamoDBHashKey]
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string SenderId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string ReceiverId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string MessageText { get; set; } = string.Empty;

        [DynamoDBProperty]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [DynamoDBProperty]
        public bool IsRead { get; set; } = false;
    }
}
