using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CodeReviewService.Models;

namespace CodeReviewService.Repositories
{
    public class CodeReviewRepository : ICodeReviewRepository
    {
        private readonly DynamoDBContext _context;

        public CodeReviewRepository(IAmazonDynamoDB db)
        {
            _context = new DynamoDBContext(db);
        }

        public async Task AddReviewAsync(CodeReview review)
        {
            await _context.SaveAsync(review);
        }

        public async Task<CodeReview?> GetReviewAsync(string id)
        {
            return await _context.LoadAsync<CodeReview>(id);
        }

        public async Task<IEnumerable<CodeReview>> GetAllReviewsAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<CodeReview>(conditions).GetRemainingAsync();
        }

        public async Task<IEnumerable<CodeReview>> GetReviewsByMenteeAsync(string menteeId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("MenteeId", ScanOperator.Equal, menteeId)
            };
            return await _context.ScanAsync<CodeReview>(conditions).GetRemainingAsync();
        }

        public async Task<IEnumerable<CodeReview>> GetReviewsByMentorAsync(string mentorId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("MentorId", ScanOperator.Equal, mentorId)
            };
            return await _context.ScanAsync<CodeReview>(conditions).GetRemainingAsync();
        }

        public async Task UpdateReviewAsync(CodeReview review)
        {
            await _context.SaveAsync(review);
        }

        public async Task DeleteReviewAsync(string id)
        {
            await _context.DeleteAsync<CodeReview>(id);
        }
    }
}
