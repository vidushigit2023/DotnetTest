using DotNetDemo.Models.Domain;
using DotNetDemo.Models.DTO;
using DotNetDemo.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Repository.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string UserId)       //Get Use details by id
        {
			try
			{

                return await userManager.FindByIdAsync(UserId);

            }
			catch (Exception)
			{

				throw;
			}
        }
//Update user 
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                var a=await userManager.UpdateAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }       
//Get All user list for admin
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await userManager.Users.ToListAsync();
        }
        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            try
            {
                var a = await userManager.DeleteAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
//Add User 
        public async Task<Status> AddUserAsync(Registration model)
        {
            var status = new Status();
            var userExist = await userManager.FindByNameAsync(model.Username);
            if (userExist != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            //Role Manager

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }
    }
}
