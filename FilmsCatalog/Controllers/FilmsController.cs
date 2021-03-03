using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FilmsCatalog.Data;
using FilmsCatalog.Models;
using FilmsCatalog.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FilmsCatalog.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FilmsController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _db = db;
            _hostEnvironment = hostEnvironment;

        }
        // GET: FilmsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FilmsController/Details/5
        public ActionResult Details(int id)
        {
            var model = _db.Films.Include(s => s.Category).AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (model != null)
            {
                model.IsHisPostedFilm = IsHisPostedFilm(id);
            }
            return View(model);
        }
        /// <summary>
        /// Универсальный метод для создания и изменения (Создал потому что не хотел каждый раз повторять код)
        /// Если это Edit то айди передаем
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: FilmsController/Create
        public ActionResult CreateOrEdit(int? id)
        {
            IEnumerable<Category> CatList = _db.Categories.ToList();
            FilmsVM filmVM = new FilmsVM()
            {
                Film = new Film(),
                CategoryList = CatList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(filmVM);
            }
            //this is for edit
            filmVM.Film = _db.Films.Find(id.GetValueOrDefault());
            if (filmVM.Film == null)
            {
                return NotFound();
            }
            return View(filmVM);
        }

        // POST: FilmsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit(FilmsVM filmVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\film");
                        var extenstion = Path.GetExtension(files[0].FileName);

                        if (filmVm.Film.Poster != null)
                        {
                            //это для edit и мы удаляем старое изображение
                            var imagePath = Path.Combine(webRootPath, filmVm.Film.Poster.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                        {
                            files[0].CopyTo(filesStreams);
                        }
                        filmVm.Film.Poster = @"\images\film\" + fileName + extenstion;
                    }
                    else
                    {
                        //Если мы не обновляем изображение
                        if (filmVm.Film.Id != 0)
                        {
                            Film objFromDb = _db.Films.AsNoTracking().FirstOrDefault(s => s.Id == filmVm.Film.Id);
                            filmVm.Film.Poster = objFromDb?.Poster;
                        }
                    }
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    filmVm.Film.UserId = userId;

                    if (filmVm.Film.Id == 0)
                    {
                        _db.Films.Add(filmVm.Film);

                    }
                    else
                    {
                        _db.Films.Update(filmVm.Film);
                    }

                    _db.SaveChanges();
                    return RedirectToAction("Index","Home");
                }

                IEnumerable<Category> CatList = _db.Categories.ToList();
                filmVm.CategoryList = CatList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if (filmVm.Film.Id != 0)
                {
                    filmVm.Film = _db.Films.Find(filmVm.Film.Id);
                }

                return View(filmVm);
            }
            catch
            {
                return View();
            }
        }

        // GET: FilmsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilmsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilmsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FilmsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Проверяет выложил ли пользователь этот фильм
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
        public bool IsHisPostedFilm(int filmId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return false;
            }

            var model = _db.Films.FirstOrDefault(s => s.Id == filmId && s.UserId == userId);
            if (model == null)
            {
                return false;
            }
            return true;
        }
    }
}
