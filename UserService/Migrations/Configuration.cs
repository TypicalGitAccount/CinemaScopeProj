namespace Identity.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using Identity.Contexts;
    using Identity.Managers;
    using Identity.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdentityContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            var adminRole = new ApplicationRole 
            { 
                Name = "Administrator", 
                Description = "Administrator manages movies, users and About Us page." 
            };
            var userRole = new ApplicationRole 
            { 
                Name = "User", 
                Description = "User uses accessible information about movies." 
            };

            roleManager.Create(adminRole);
            roleManager.Create(userRole);

            var admin = new ApplicationUser 
            { 
                Email = "admin@gmail.com", 
                UserName = "admin" 
            };
            var password = "FluffyRabbits123";
            var result = manager.Create(admin, password);

            if (!result.Succeeded) throw new InvalidOperationException("Failed");

            manager.AddToRole(admin.Id, adminRole.Name);
            manager.AddToRole(admin.Id, userRole.Name);
        }
    }
}
