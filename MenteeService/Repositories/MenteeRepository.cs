using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using MenteeService.Models;

namespace MenteeService.Repositories
{
    public class MenteeRepository: IMenteeRepository
    {
        private readonly DynamoDBContext _context;

        public MenteeRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddMenteeAsync(Mentee mentee)
        {
            await _context.SaveAsync(mentee);
        }

        public async Task<Mentee?> GetMenteeAsync(string id)
        {
            return await _context.LoadAsync<Mentee>(id);
        }

        public async Task<IEnumerable<Mentee>> GetAllMenteesAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Mentee>(conditions).GetRemainingAsync();
        }

        public async Task UpdateMenteeAsync(Mentee mentee)
        {
            await _context.SaveAsync(mentee);
        }

        public async Task DeleteMenteeAsync(string id)
        {
            await _context.DeleteAsync<Mentee>(id);
        }
    }
}
