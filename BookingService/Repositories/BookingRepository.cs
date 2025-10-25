using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BookingService.Models;

namespace BookingService.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DynamoDBContext _context;

        public BookingRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _context.SaveAsync(booking);
        }

        public async Task<Booking?> GetBookingAsync(string id)
        {
            return await _context.LoadAsync<Booking>(id);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Booking>(conditions).GetRemainingAsync();
        }
    }
}
