using BookingService.Models;
using System.Threading.Tasks;

namespace BookingService.Repositories
{
    public interface IBookingRepository
    {
        Task AddBookingAsync(Booking booking);
        Task<Booking?> GetBookingAsync(string id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
    }
}
