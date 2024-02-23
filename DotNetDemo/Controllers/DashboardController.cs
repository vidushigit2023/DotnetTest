using DotNetDemo.Models.Domain;
using DotNetDemo.Models.DTO;
using DotNetDemo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetDemo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;

        public DashboardController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Display()
        {
            return View();
        }
        //view user profile
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> MyProfile(string userId=null)
        {
            if (userId == null)
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            
            ApplicationUser userinfo = await _userService.GetUserDetailsAsync(userId);
            UpdateUserModel updateUserModel = new UpdateUserModel();
            updateUserModel.UserName = userinfo.UserName;
            updateUserModel.PhoneNumber = userinfo.PhoneNumber;
            updateUserModel.LastName= userinfo.LastName;
            updateUserModel.FirstName = userinfo.FirstName;
            updateUserModel.ProfilePicture = userinfo.ProfilePicture;
            updateUserModel.DocFile = userinfo.DocFile;
            TempData["UserId"] = userId;
            return View(updateUserModel);

        }
        [Authorize(Roles = "admin,user")]
        [HttpPost]
        public async Task<IActionResult> MyProfile(UpdateUserModel model)
        {
            string userId;
            if (TempData["UserId"] != null)
            {
                userId = TempData["UserId"].ToString();
            }
            else
            {
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
           
            ApplicationUser userinfo = await _userService.GetUserDetailsAsync(userId);
            //File location of upload
            string uploadPath = "wwwroot//ProfileImg";
            string docuploadPath = "wwwroot//Cvfile";

            var files = HttpContext.Request.Form.Files;
            if (files.Count != 0)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        if (file.Name == "ProfilePicture")
                        {
                            //Rename files
                            var fileName = files[0].FileName.Split('.')[0].ToString().Replace("/", "-").Replace(" ", "-") + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + Path.GetExtension(file.FileName);
                        var uploadPathWithfileName = Path.Combine(uploadPath, fileName);
                        var uploadAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), uploadPathWithfileName);
                        using (var fileStream = new FileStream(uploadAbsolutePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);

                            userinfo.ProfilePicture = fileName;


                        }
                    }
                        else if(file.Name == "DocFile")
                        {
                            //Rename files
                            var fileName = files[0].FileName.Split('.')[0].ToString().Replace("/", "-").Replace(" ", "-") + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + Path.GetExtension(file.FileName);
                            var uploadPathWithfileName = Path.Combine(docuploadPath, fileName);
                            var uploadAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(), uploadPathWithfileName);
                            using (var fileStream = new FileStream(uploadAbsolutePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);

                                userinfo.DocFile = fileName;


                            }
                        }
                    }
                }
            }   


            userinfo.PhoneNumber= model.PhoneNumber;
            userinfo.LastName= model.LastName;
            userinfo.FirstName= model.FirstName;
            var result = await _userService.UpdateUserAsync(userinfo);
            if (result)
            {
                TempData["msg"] = "Profile Updated Successfully.";
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                TempData["msg"] = "Profile Updat Fail.";
                return View(model);
            }
        }

        //If user try to access unauthorize page from url or something else, then this redirect to access denide page
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        }
}
