using MessagingService.Models;

namespace MessagingService.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<Message?> GetMessageAsync(string id);
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<IEnumerable<Message>> GetMessagesByUserAsync(string userId);
        Task DeleteMessageAsync(string id);
        Task MarkAsReadAsync(string id);
    }
}
