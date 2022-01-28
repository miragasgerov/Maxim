using MaximBackEnd.Helper;
using MaximBackEnd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static System.Collections.Specialized.BitVector32;

namespace MaximBackEnd.Areas.manage.Controllers
{
    [Area("manage")]
    public class ServiceController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page =1)
        {
            return View(_context.Services.ToList().ToPagedList(page,2));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Service service)
        {
            if (service.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Sekil Daxil Edin!");
            }

            if(service.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Sekilin max olcusu 2MB-dir !");
            }

            if(service.ImageFile.ContentType != "image/jpeg" && service.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Sekilin Type-i Jpeg ve ya Png olmalidir!");
            }

            if (!ModelState.IsValid) return View();

            service.Image = FileManager.Save(_env.WebRootPath, "uploads/services", service.ImageFile);

            _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Service service = _context.Services.FirstOrDefault(x => x.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Service service)
        {


            if (service.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Sekil Daxil Edin!");
            }

            if (service.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Sekilin max olcusu 2MB-dir !");
            }

            if (service.ImageFile.ContentType != "image/jpeg" && service.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Sekilin Type-i Jpeg ve ya Png olmalidir!");
            }

            if (!ModelState.IsValid) return View();

            Service existService = _context.Services.FirstOrDefault(x => x.Id == service.Id);

            if (existService == null) return NotFound();



            if (string.IsNullOrWhiteSpace(service.Image))
            {
                FileManager.Delete(_env.WebRootPath, "Uploads/Services", existService.Image);
                existService.Image = FileManager.Save(_env.WebRootPath, "Uploads/Services", service.ImageFile);
            }




            existService.Title = service.Title;
            existService.Text = service.Text;

            _context.SaveChanges();

            return RedirectToAction("index");

        }




        public IActionResult Delete(int id)
        {
            Service service = _context.Services.FirstOrDefault(x => x.Id == id);

            if (service == null) return NotFound();


            if (!string.IsNullOrWhiteSpace(service.Image))
            {
                FileManager.Delete(_env.WebRootPath, "uploads/services", service.Image);
            }

            service.IsDeleted = true;


            _context.SaveChanges();

            return View("index");

            //Muellim Delete isleyir mende sadece buttona basanda null gosterir ctrl+shift+R  basib yenileyende Isdeleted true olur
            //home indexdede filtirlenir ve silinen data dusmur indexe  amma o nullin sebebini tapa bilmedim.

        }
    }
}

