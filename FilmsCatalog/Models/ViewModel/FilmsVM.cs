using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilmsCatalog.Models.ViewModel
{
    public class FilmsVM
    {
        public Film Film { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }

      
    }
}
