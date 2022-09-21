using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using Identity.Interfaces;
using Identity.Contexts;
using Identity.Models;

namespace Identity.Repositories
{
    public class AboutUsRepository : IAboutUsRepository
    {
        private IdentityContext _context;

        public AboutUsRepository(IdentityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new AboutUser object.
        /// </summary>
        /// <param name="item">AboutUser value.</param>
        /// <exception cref="ArgumentNullException">The user must be not null and the user's name and description must be required.</exception>
        public void Create(AboutUser item)
        {
            if(item is null)
                throw new ArgumentNullException("User cannot be null.");
            if(String.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentNullException("User's name cannot be null or empty.");
            if(String.IsNullOrWhiteSpace(item.Description))
                throw new ArgumentNullException("User's description cannot be null or empty.");            

            _context.AboutUsers.Add(item);
        }

        /// <summary>
        /// Update an existing AboutUser object or create a new one if it does not exist.
        /// </summary>
        /// <param name="item">AboutUser value.</param>
        /// <exception cref="ArgumentNullException">The user must be not null and the user's name and description must be required.</exception>
        public void Update(AboutUser item)
        {
            if (item is null)
                throw new ArgumentNullException("User cannot be null.");
            if (String.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentNullException("User's name cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(item.Description))
                throw new ArgumentNullException("User's description cannot be null or empty.");

            _context.AboutUsers.AddOrUpdate(item);
        }

        /// <summary>
        /// Delete AboutUser object by its id.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        public void DeleteById(int id)
        {
            var user = _context.AboutUsers.FirstOrDefault(x => x.Id == id);
            if(user != null)
                _context.AboutUsers.Remove(user);
        }

        /// <summary>
        /// Get all existing AboutUser objects.
        /// </summary>
        /// <returns>IEnumerable of AboutUser objects.</returns>
        public IEnumerable<AboutUser> GetAll()
        {  
            return _context.Set<AboutUser>().ToList();
        }

        /// <summary>
        /// Get an existing AboutUser object by its id.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        /// <returns>AboutUser object or null if it doesn't exist.</returns>
        public AboutUser GetById(int id)
        {            
            return _context.Set<AboutUser>().FirstOrDefault(u => u.Id == id);
        }               
    }
}
