using Amazon.DynamoDBv2.DataModel;

namespace MenteeService.Models
{
    [DynamoDBTable("SkillBridge_Mentees")]
    public class Mentee
    {
        [DynamoDBHashKey]
        public string MenteeId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string Name { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string FocusArea { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Goals { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Email { get; set; } = string.Empty;

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
