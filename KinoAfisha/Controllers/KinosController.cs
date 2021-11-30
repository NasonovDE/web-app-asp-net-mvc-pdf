using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using KinoAfisha.Models;


namespace KinoAfisha.Controllers
{
    public class KinosController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new KinoAfishaContext();
            var kino = db.Kinos.ToList();


            return View(kino);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var kino = new Kino();
            return View(kino);

        }

        [HttpPost]
        public ActionResult Create(Kino model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new KinoAfishaContext();
            model.CreateAt = DateTime.Now;
            model.NextArrivalDate = DateTime.Now;
            if (model.FilmIds != null && model.FilmIds.Any())
            {
                var film = db.Films.Where(s => model.FilmIds.Contains(s.Id)).ToList();
                model.Films = film;
            }
            if (model.CinemaIds != null && model.CinemaIds.Any())
            {
                var cinema = db.Cinemas.Where(s => model.CinemaIds.Contains(s.Id)).ToList();
                model.Cinemas = cinema;
            }



            db.Kinos.Add(model);
            db.SaveChanges();


            return RedirectPermanent("/Kinos/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new KinoAfishaContext();
            var kino = db.Kinos.FirstOrDefault(x => x.Id == id);
            if (kino == null)
                return RedirectPermanent("/Kinos/Index");

            db.Kinos.Remove(kino);
            db.SaveChanges();

            return RedirectPermanent("/Kinos/Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new KinoAfishaContext();
            var kino = db.Kinos.FirstOrDefault(x => x.Id == id);

            if (kino == null)
                return RedirectPermanent("/Kinos/Index");

            return View(kino);
        }

        [HttpPost]
        public ActionResult Edit(Kino model)
        {

            var db = new KinoAfishaContext();
            var kino = db.Kinos.FirstOrDefault(x => x.Id == model.Id);

            

            if (kino == null)
            {
                ModelState.AddModelError("Id", "кино не найдено");
            }
            if (!ModelState.IsValid)
                return View(model);
            
            MappingKino(model, kino, db);
            
            db.Entry(kino).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectPermanent("/Kinos/Index");
        }


        private void MappingKino(Kino sourse, Kino destination, KinoAfishaContext db)
        {

            destination.Price = sourse.Price;
            destination.NextArrivalDate = sourse.NextArrivalDate;
            destination.KinoTime = sourse.KinoTime;
            destination.FilmIds = sourse.FilmIds;




            if (destination.Films != null)
                destination.Films.Clear();

            if (sourse.FilmIds != null && sourse.FilmIds.Any())
                destination.Films = db.Films.Where(s => sourse.FilmIds.Contains(s.Id)).ToList();

            if (destination.Cinemas != null)
                destination.Cinemas.Clear();

            if (sourse.CinemaIds != null && sourse.CinemaIds.Any())
                destination.Cinemas = db.Cinemas.Where(s => sourse.CinemaIds.Contains(s.Id)).ToList();


        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var db = new KinoAfishaContext();
            var kino = db.Kinos.FirstOrDefault(x => x.Id == id);
            if (kino == null)
                return RedirectPermanent("/Kinos/Index");

            return View(kino);
        }

    }
}
