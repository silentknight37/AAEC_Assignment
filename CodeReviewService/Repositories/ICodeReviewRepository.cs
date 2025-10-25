using CodeReviewService.Models;

namespace CodeReviewService.Repositories
{
    public interface ICodeReviewRepository
    {
        Task AddReviewAsync(CodeReview review);
        Task<CodeReview?> GetReviewAsync(string id);
        Task<IEnumerable<CodeReview>> GetAllReviewsAsync();
        Task<IEnumerable<CodeReview>> GetReviewsByMenteeAsync(string menteeId);
        Task<IEnumerable<CodeReview>> GetReviewsByMentorAsync(string mentorId);
        Task UpdateReviewAsync(CodeReview review);
        Task DeleteReviewAsync(string id);
    }
}
