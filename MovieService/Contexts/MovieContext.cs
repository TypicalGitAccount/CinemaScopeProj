using System.Data.Entity;
using MovieService.Entities;

namespace MovieService.Contexts
{
    public class MovieContext : DbContext
    {
        public virtual DbSet<MovieType> Types { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public DbSet<UserToMovie> UserToMovies { get; set; }

        public MovieContext() : base("MovieDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToMovie>().HasKey(k => new {k.ApplicationUserId, k.MovieId});
            base.OnModelCreating(modelBuilder);
        }
    }
}
