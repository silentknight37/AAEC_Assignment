using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using PaymentService.Models;

namespace PaymentService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DynamoDBContext _context;

        public PaymentRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.SaveAsync(payment);
        }

        public async Task<Payment?> GetPaymentAsync(string id)
        {
            return await _context.LoadAsync<Payment>(id);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Payment>(conditions).GetRemainingAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingAsync(string bookingId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("BookingId", ScanOperator.Equal, bookingId)
            };
            return await _context.ScanAsync<Payment>(conditions).GetRemainingAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await _context.SaveAsync(payment);
        }

        public async Task DeletePaymentAsync(string id)
        {
            await _context.DeleteAsync<Payment>(id);
        }
    }
}
