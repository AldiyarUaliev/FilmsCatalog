using FilmsCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilmsCatalog.Data;
using FilmsCatalog.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;
using System.Security.Claims;
namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _db = db;
            _hostEnvironment = hostEnvironment;

        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1; // Если нет определенной страницы то по дефолту 1
            int pageSize = 2; // количество элементов в каждой странице
            var onePageOfFilms = _db.Films.Include(s => s.Category).ToList().ToPagedList(pageNumber, pageSize);
            return View(onePageOfFilms); 
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult CreateFilm()
        {
            return View();
        }

      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   
    }

}
