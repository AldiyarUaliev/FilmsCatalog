using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Film> Films { get; set; }

        public Category()
        {
            Films = new List<Film>();
        }
    }
}
