using PaymentService.Models;

namespace PaymentService.Repositories
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentAsync(string id);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByBookingAsync(string bookingId);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(string id);
    }
}
