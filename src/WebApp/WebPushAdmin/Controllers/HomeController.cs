namespace KitsorLab.WebApp.WebPushAdmin.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;
	using KitsorLab.WebApp.WebPushAdmin.Models;
	using Microsoft.AspNetCore.Mvc;

	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
