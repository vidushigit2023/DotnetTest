using DotNetDemo.Models.DTO;

namespace DotNetDemo.Repository.Interface
{
    public interface IUserAuthentication
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> RegistrationAsync(Registration model);
        Task LogoutAsync();
        Task<Status> ForgotPasswordAsync(ForgotPasswordModel model);
    }
}
