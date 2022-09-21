using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{  
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public bool IsBanned { get; set; }

        public ApplicationUser() { }
    }
}
