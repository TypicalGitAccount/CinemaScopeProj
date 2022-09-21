using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace CinemaScopeWeb.ViewModels
{
    public class FilteringViewModel
    {
        public List<MovieToHomeViewModel> Movies { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Types { get; set; }
        public List<int> Years { get; set; }
        public List<string> Countries { get; set; }
        public bool IsWatched { get; set; }
    }
}