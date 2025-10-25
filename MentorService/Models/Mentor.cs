using Amazon.DynamoDBv2.DataModel;

namespace MentorService.Models
{
    [DynamoDBTable("SkillBridge_Mentors")]
    public class Mentor
    {
        [DynamoDBHashKey]                      // Partition key
        public string MentorId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string Name { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Domain { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string ExperienceLevel { get; set; } = string.Empty;

        [DynamoDBProperty]
        public List<string> Availability { get; set; } = new();

        [DynamoDBProperty]
        public List<string> Badges { get; set; } = new();
    }
}
