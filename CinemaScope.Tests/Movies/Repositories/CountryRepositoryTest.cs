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
    public class CountryRepositoryTest
    {
        private Movie testMovie;

        private Mock<MovieContext> SetUpMockDb(IQueryable<Country> data, Mock<DbSet<Country>> mockDbSet)
        {
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockDb = new Mock<MovieContext>();
            mockDb.Setup(m => m.Countries).Returns(mockDbSet.Object);

            return mockDb;
        }

        [SetUp]
        public void SetUp()
        {
            testMovie = new Movie();
        }

        [Test]
        public void GetRangeByName_DefaultScenario_ReturnsTypeId()
        {
            var names = new List<string>() { "USA", "Canada" };
            var data = new List<Country>() { new Country() { Name = "USA"}, new Country() { Name = "Canada" } }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<CountryRepository>(mockDb.Object) { CallBase = true };
            testRepo.Setup(r => r.Add(It.IsAny<Country>())).Callback((Country val) => data = data.Append(val).AsQueryable());
            testRepo.Setup(r => r.Update(It.IsAny<Country>())).Callback((Country val) =>
            {
                data = data.Append(val).AsQueryable();
                var toUpdate = data.Where(x => x.Name == val.Name).First();
                toUpdate = val;
            });

            var result = testRepo.Object.GetRangeByName(names);

            for (int i = 0; i < names.Count; i++)
            {
                Assert.That(result[i].Name == names[i]);
            }
        }

        [Test]
        public void GetByName_NoCountryInDb_CreatesCountry()
        {
            var names = new List<string>() { "Canada" };
            var data = new List<Country>() { }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<CountryRepository>(mockDb.Object) { CallBase = true };
            testRepo.Setup(r => r.Add(It.IsAny<Country>())).Callback((Country val) => data = data.Append(val).AsQueryable());
            testRepo.Setup(r => r.Update(It.IsAny<Country>())).Callback((Country val) =>
            {
                data = data.Append(val).AsQueryable();
                var toUpdate = data.First(x => x.Name == val.Name);
                toUpdate = val;
            });

            var result = testRepo.Object.GetRangeByName(names);

            Assert.That(result.First().Name == "Canada");
        }

        [Test]
        public void GetByName_NullOrEmptyStringPassed_ReturnsNull()
        {
            var data = new List<Country>() { }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new CountryRepository(mockDb.Object);

            Assert.That(testRepo.GetRangeByName(new List<string>()) == null);
            Assert.That(testRepo.GetRangeByName(null) == null);
        }
    }
}
