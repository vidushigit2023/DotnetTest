using DotNetDemo.Models.Domain;
using DotNetDemo.Models.DTO;
using DotNetDemo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDemo.Controllers
{
   
    public class AdminController : Controller
    {

        private readonly IUserService _userService;


        public AdminController(IUserService userservice)
        {

            _userService = userservice;
        }
        [Authorize(Roles = "admin,user")]
        public IActionResult Display()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UserList()
        {

            var users = await _userService.GetAllUsersAsync();
            UserDetails ulist = new UserDetails();
            ulist.Userlist = users;
            return View(ulist);
        }

        //Delete User from grid
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<Status> Delete(string Id)
        {
            var user = await _userService.GetUserDetailsAsync(Id);


            var status = await _userService.DeleteUserAsync(user);

            Status sboj = new Status();

            if (status == true)
            {
                sboj.StatusCode = 1;
                sboj.Message = "Data deleted successfully.";
            }
            else
            {
                sboj.StatusCode = 0;
                sboj.Message = "Data deleted Failed.";
            }

            return sboj;

        }



       

        //public async Task<IActionResult> AddUser(Registration model)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
