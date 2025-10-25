using MenteeService.Models;

namespace MenteeService.Repositories
{
    public interface IMenteeRepository
    {
        Task AddMenteeAsync(Mentee mentee);
        Task<Mentee?> GetMenteeAsync(string id);
        Task<IEnumerable<Mentee>> GetAllMenteesAsync();
        Task UpdateMenteeAsync(Mentee mentee);
        Task DeleteMenteeAsync(string id);
    }
}
