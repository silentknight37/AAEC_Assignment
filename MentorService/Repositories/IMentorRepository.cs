using MentorService.Models;

namespace MentorService.Repositories
{
    public interface IMentorRepository
    {
        Task AddMentorAsync(Mentor mentor);
        Task<Mentor?> GetMentorAsync(string id);
        Task<IEnumerable<Mentor>> GetAllMentorsAsync();
        Task UpdateMentorAsync(Mentor mentor);
        Task DeleteMentorAsync(string id);
    }
}
