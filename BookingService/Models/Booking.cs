using Amazon.DynamoDBv2.DataModel;

namespace BookingService.Models
{
    [DynamoDBTable("SkillBridge_Bookings")]
    public class Booking
    {
        [DynamoDBHashKey]
        public string BookingId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string MentorId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string MenteeId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public DateTime SlotTime { get; set; }

        [DynamoDBProperty]
        public string Status { get; set; } = "PendingPayment";
    }
}
