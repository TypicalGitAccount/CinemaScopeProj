using System;

namespace Identity.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAboutUsRepository AboutUsRepository { get; }

        void Save();
    }
}
