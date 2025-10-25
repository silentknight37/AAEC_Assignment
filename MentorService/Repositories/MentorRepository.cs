using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MentorService.Models;

namespace MentorService.Repositories
{
    public class MentorRepository : IMentorRepository
    {
        private readonly DynamoDBContext _context;

        public MentorRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddMentorAsync(Mentor mentor)
        {
            await _context.SaveAsync(mentor);
        }

        public async Task<Mentor?> GetMentorAsync(string id)
        {
            return await _context.LoadAsync<Mentor>(id);
        }

        public async Task<IEnumerable<Mentor>> GetAllMentorsAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Mentor>(conditions).GetRemainingAsync();
        }

        public async Task UpdateMentorAsync(Mentor mentor)
        {
            await _context.SaveAsync(mentor);   // Same as Add, updates existing record
        }

        public async Task DeleteMentorAsync(string id)
        {
            await _context.DeleteAsync<Mentor>(id);
        }
    }
}
