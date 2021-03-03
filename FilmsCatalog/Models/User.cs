using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FilmsCatalog.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<Film> Films { get; set; }

        public User()
        {
            Films = new List<Film>();
        }
    }
}