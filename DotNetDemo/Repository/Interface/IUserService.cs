using DotNetDemo.Models.Domain;
using DotNetDemo.Models.DTO;

namespace DotNetDemo.Repository.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserDetailsAsync(string Username);
        Task<bool> UpdateUserAsync(ApplicationUser user);

        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(ApplicationUser user);
        Task<Status> AddUserAsync(Registration model);
    }

}
