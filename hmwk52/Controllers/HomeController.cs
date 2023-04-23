using hmwk52.Models;
using hmwk52.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hmwk52.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewModel viewModel = new();
            return View(viewModel);
        }


    }
}