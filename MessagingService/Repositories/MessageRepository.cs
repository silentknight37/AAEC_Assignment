using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using MessagingService.Models;

namespace MessagingService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DynamoDBContext _context;

        public MessageRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddMessageAsync(Message message)
        {
            await _context.SaveAsync(message);
        }

        public async Task<Message?> GetMessageAsync(string id)
        {
            return await _context.LoadAsync<Message>(id);
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Message>(conditions).GetRemainingAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserAsync(string userId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("SenderId", ScanOperator.Equal, userId)
            };

            return await _context.ScanAsync<Message>(conditions).GetRemainingAsync();
        }

        public async Task DeleteMessageAsync(string id)
        {
            await _context.DeleteAsync<Message>(id);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var msg = await _context.LoadAsync<Message>(id);
            if (msg != null)
            {
                msg.IsRead = true;
                await _context.SaveAsync(msg);
            }
        }
    }
}
