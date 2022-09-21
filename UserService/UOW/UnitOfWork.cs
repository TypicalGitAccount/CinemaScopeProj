using Identity.Interfaces;
using Identity.Contexts;
using Identity.Repositories;

namespace Identity.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IdentityContext _context;

        private IAboutUsRepository _aboutUsRepository;

        public UnitOfWork()
        {
            _context = new IdentityContext();
        }

        public IAboutUsRepository AboutUsRepository =>
            _aboutUsRepository ?? (_aboutUsRepository = new AboutUsRepository(_context));

        /// <summary>
        /// Save changes in the Identity context.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
