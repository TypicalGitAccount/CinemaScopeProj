namespace MovieService.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImdbId = c.String(),
                        Title = c.String(nullable: false, maxLength: 220),
                        Poster = c.String(),
                        Year = c.String(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Cast = c.String(),
                        Plot = c.String(),
                        Budget = c.String(),
                        BoxOffice = c.String(),
                        RatingIMDb = c.String(),
                        SiteUsersRating = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserToMovies",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        MovieId = c.Int(nullable: false),
                        IsLiked = c.Boolean(nullable: false),
                        IsDisLiked = c.Boolean(nullable: false),
                        IsWatched = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUserId, t.MovieId });
            
            CreateTable(
                "dbo.MovieCountries",
                c => new
                    {
                        Movie_Id = c.Int(nullable: false),
                        Country_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_Id, t.Country_Id })
                .ForeignKey("dbo.Movies", t => t.Movie_Id, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_Id, cascadeDelete: true)
                .Index(t => t.Movie_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.GenreMovies",
                c => new
                    {
                        Genre_Id = c.Int(nullable: false),
                        Movie_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_Id, t.Movie_Id })
                .ForeignKey("dbo.Genres", t => t.Genre_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_Id, cascadeDelete: true)
                .Index(t => t.Genre_Id)
                .Index(t => t.Movie_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "TypeId", "dbo.MovieTypes");
            DropForeignKey("dbo.GenreMovies", "Movie_Id", "dbo.Movies");
            DropForeignKey("dbo.GenreMovies", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.MovieCountries", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.MovieCountries", "Movie_Id", "dbo.Movies");
            DropIndex("dbo.GenreMovies", new[] { "Movie_Id" });
            DropIndex("dbo.GenreMovies", new[] { "Genre_Id" });
            DropIndex("dbo.MovieCountries", new[] { "Country_Id" });
            DropIndex("dbo.MovieCountries", new[] { "Movie_Id" });
            DropIndex("dbo.Movies", new[] { "TypeId" });
            DropTable("dbo.GenreMovies");
            DropTable("dbo.MovieCountries");
            DropTable("dbo.UserToMovies");
            DropTable("dbo.MovieTypes");
            DropTable("dbo.Genres");
            DropTable("dbo.Movies");
            DropTable("dbo.Countries");
        }
    }
}
