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
using PagedList;

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
        public ActionResult GetCountries(FilterOptions filterOptions)
        {
            ViewBag.CurrentSort = filterOptions.SortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(filterOptions.SortOrder) ? "population_desc" : "";
            ViewBag.DateSortParm = filterOptions.SortOrder == "Date" ? "date_desc" : "Date";

            if (filterOptions.SearchString != null)
            {
                filterOptions.Page = 1;
            }


            var countries = from s in db.Countries
                           select s;

            if (filterOptions.SelectedCountries != null && filterOptions.SelectedCountries.Any())
            {
                countries = from p in countries
                              where filterOptions.SelectedCountries.Any(val => p.Code.Contains(val))
                              select p; ;
            }

            if (!String.IsNullOrEmpty(filterOptions.SearchString))
            {
                countries = countries.Where(s => 
                    s.Code.Contains(filterOptions.SearchString)
                 || s.FullName.Contains(filterOptions.SearchString)
                 || s.CapitalCity.Contains(filterOptions.SearchString)
                 || s.Region.Contains(filterOptions.SearchString)
                 || s.Currency.Contains(filterOptions.SearchString)
                 || s.Language.Contains(filterOptions.SearchString)
                );
            }

            switch (filterOptions.SortOrder)
            {
                case "FullName_asc":
                    countries = countries.OrderBy(s => s.FullName);
                    break;
                case "FullName_desc":
                    countries = countries.OrderByDescending(s => s.FullName);
                    break;
                case "Population_asc":
                    countries = countries.OrderBy(s => s.Population);
                    break;
                case "Population_desc":
                    countries = countries.OrderByDescending(s => s.Population);
                    break;
                case "CapitalCity_asc":
                    countries = countries.OrderBy(s => s.CapitalCity);
                    break;
                case "CapitalCity_desc":
                    countries = countries.OrderByDescending(s => s.CapitalCity);
                    break;
                default:  // Name ascending 
                    countries = countries.OrderByDescending(s => s.Population);
                    break;
            }

            var result = countries.ToPagedList(filterOptions.Page, filterOptions.PageSize);
            var topFivePopulated = db.Countries.OrderByDescending(c => c.Population).Take(5);

            return Json(new { data = result, total = countries.LongCount(), topFivePopulated = topFivePopulated } , JsonRequestBehavior.AllowGet);
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
