using MaximBackEnd.Models;
using MaximBackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MaximBackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel
            {
                services = _context.Services.Where(x=> x.IsDeleted == false).ToList()
            };
            return View(homeVM);
        }

     
    }
}
