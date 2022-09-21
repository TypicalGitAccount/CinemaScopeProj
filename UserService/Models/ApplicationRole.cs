using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public ApplicationRole() { }        
    }
}
