using Amazon.DynamoDBv2.DataModel;

namespace PaymentService.Models
{
    [DynamoDBTable("SkillBridge_Payments")]
    public class Payment
    {
        [DynamoDBHashKey]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string BookingId { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string PayerId { get; set; } = string.Empty; // Mentee ID

        [DynamoDBProperty]
        public string PayeeId { get; set; } = string.Empty; // Mentor ID

        [DynamoDBProperty]
        public decimal Amount { get; set; }

        [DynamoDBProperty]
        public string Currency { get; set; } = "USD";

        [DynamoDBProperty]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

        [DynamoDBProperty]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [DynamoDBProperty]
        public string Method { get; set; } = "CreditCard"; // or PayPal, Bank, etc.
    }
}
