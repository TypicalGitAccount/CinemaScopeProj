using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Identity.Models;

namespace Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<AboutUser> AboutUsers { get; set; }

        public IdentityContext() : base("IdentityDbContext") { }

        public static IdentityContext Create()
        {            
            return new IdentityContext();
        }
    }
}
