using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class CountryInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CountryContext>
    {
        protected override void Seed(CountryContext context)
        {
            var countries = new List<Country>
            {
                new Country { ID = 1, Name = "Turkey", FullName = "Republic of Turkey", Code = "TR", Currency = "Turkish lira", Flag = "TR", Language = "Turkish", CapitalCity = "Ankara", Population = 78741053, Region = "Asia"  }
            };

            countries.ForEach(country => context.Countries.Add(country));
        }
    }
}