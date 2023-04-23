using Hmwk52.data;
using Hmwk52.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmwk52.web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=AdLogin; Integrated Security=true;";

        public IActionResult Login()
        {
            ViewModel viewModel = new()
            {
                Message = (string)TempData["Message"]
            };
            return View(viewModel);
        }

        [HttpPost]
            public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.LoginUserByEmailAndPassword(email, password);

            if(user == null)
            {
                TempData["Message"] = "Invalid User";
                return RedirectToAction("Login");
            }

            //this code logs in the current user
            var claims = new List<Claim>
            {
                new Claim("user", email) //this will store the users email into the login cookie
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();


            return Redirect("/home/newad");
        }
        public IActionResult Logout()
        {

            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user)
        {
            UserRepository repo = new(_connectionString);
            repo.Add(user);

            return RedirectToAction("Login");
        }
    }
}
