using itemsproject.Data;
using itemsproject.Models;
using itemsproject.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace itemsproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult AboutApp()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            var totalItems = context.Items.Count();
            var totalCategories = context.Categories.Count();
            var totalClients = context.Clients.Count();
            var recentItems = context.Items
                .Include(i => i.Category)
                .OrderByDescending(i => i.Id)
                .Take(5)
                .ToList();

            ViewBag.TotalItems = totalItems;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalClients = totalClients;
            ViewBag.RecentItems = recentItems;

            return View();
        }
    }
}
