using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CountriesController : Controller
    {
        private CountryContext db = new CountryContext();

        // GET: Countries
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCountries()
        {
            List<Country> result = db.Countries.OrderByDescending(i => i.Population).ToList();
            return Json(result , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCountry(int id)
        {
            var result = false;
            Country country = db.Countries.Find(id);
            db.Countries.Remove(country);
            db.SaveChanges();
            result = true;
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCountry (Country country)
        {
            var result = false;
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                result = true;
            }
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateInitialData(List<Country> countries)
        {
            var result = false;
            var count = db.Countries.Count();
            if (count != 250)
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Country]");
                db.Countries.AddRange(countries);
                db.SaveChanges();
                result = true;
            }

            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
