using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FilterOptions
    {
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<string> SelectedCountries { get; set; }
    }
}