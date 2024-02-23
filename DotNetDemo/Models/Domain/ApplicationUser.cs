using Microsoft.AspNetCore.Identity;

namespace DotNetDemo.Models.Domain
{
    //This is application db user that is identity user
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? DocFile { get; set; }
    }
    public class UserDetails
    {
        public List<ApplicationUser>? Userlist { get; set; }
    }

}
