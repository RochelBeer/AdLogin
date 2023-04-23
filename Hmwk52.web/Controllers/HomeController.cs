using Hmwk52.data;
using Hmwk52.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hmwk52.web.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=AdLogin; Integrated Security=true;";

        public IActionResult Index()
        {
            UserRepository repo = new(connectionString);
            ViewModel viewModel = new()
            {
                Ads = repo.GetAds()
            };

            string currentUserEmail = User.Identity.Name;
            if (User.Identity.Name != null)
            {
                viewModel.CurrentUser = repo.GetByEmail(currentUserEmail);
            }

            return View(viewModel);
        }
        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            UserRepository repo = new(connectionString);
            string currentUserEmail = User.Identity.Name;

            User user = repo.GetByEmail(currentUserEmail);
            repo.Add(ad, user.Id);
            return RedirectToAction("index");

        }
        public IActionResult DeleteAd(int id)
        {
            UserRepository repo = new(connectionString);
            repo.DeleteAd(id);
            return RedirectToAction("index");
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            UserRepository repo = new(connectionString);
            string currentUserEmail = User.Identity.Name;
            User currentUser = repo.GetByEmail(currentUserEmail);
            ViewModel viewModel = new()
            {
                Ads = repo.MyAds(currentUser.Id)
            };

            return View(viewModel);

        }


    }
}