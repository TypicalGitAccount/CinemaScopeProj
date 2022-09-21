using Moq;
using MovieService.Contexts;
using MovieService.Entities;
using MovieService.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CinemaScope.Tests.Movies.Repositories
{
    [TestFixture]
    public class GenreRepositoryTest
    {
        private Movie testMovie;

        [SetUp]
        public void SetUp()
        {
            testMovie = new Movie();
        }

        private Mock<MovieContext> SetUpMockDb(IQueryable<Genre> data, Mock<DbSet<Genre>> mockDbSet)
        {
            mockDbSet.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockDb = new Mock<MovieContext>();
            mockDb.Setup(m => m.Genres).Returns(mockDbSet.Object);

            return mockDb;
        }

        [Test]
        public void GetRangeByName_DefaultScenario_ReturnsGenreList()
        {
            var nameList = new List<string>() { "Drama", "Fiction"};
            
            var testModels = new List<Genre>() { new Genre() { Name = "Drama"}, new Genre { Name = "Fiction" } };
            var data = testModels.AsQueryable();
            var mockDbSet = new Mock<DbSet<Genre>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new GenreRepository(mockDb.Object);

            var resultGenres = testRepo.GetRangeByName(nameList);

            for (int i = 0; i < resultGenres.Count; i++) {
                Assert.That(resultGenres[i].Name == nameList[i]);
            }
        }

        [TestCase(null)]
        public void GetRangeByName_EmptyNameList_ReturnsNull(List<string> nameList)
        {
            var testModels = new List<Genre>() { new Genre() { Name = "Drama" }, new Genre { Name = "Fiction" } };
            var data = testModels.AsQueryable();
            var mockDbSet = new Mock<DbSet<Genre>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<GenreRepository>(mockDb.Object) { CallBase = true };            

            Assert.That(testRepo.Object.GetRangeByName(nameList) == null);
        }
    }
}
