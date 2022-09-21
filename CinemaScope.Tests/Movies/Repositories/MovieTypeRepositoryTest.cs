using Moq;
using MovieService.Contexts;
using MovieService.Entities;
using MovieService.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CinemaScope.Tests.Movies.Repositories
{
    [TestFixture]
    public class MovieTypeRepositoryTest
    {
        private Mock<MovieContext> SetUpMockDb(IQueryable<MovieType> data, Mock<DbSet<MovieType>> mockDbSet)
        {
            mockDbSet.As<IQueryable<MovieType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<MovieType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<MovieType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<MovieType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockDb = new Mock<MovieContext>();
            mockDb.Setup(m => m.Types).Returns(mockDbSet.Object);

            return mockDb;
        }

        [Test]
        public void GetByName_DefaultScenario_ReturnsTypeId()
        {
            var data = new List<MovieType>() { new MovieType() { Name = "TV Series", Id = 0} }.AsQueryable();
            var mockDbSet = new Mock<DbSet<MovieType>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<MovieTypeRepository>(mockDb.Object);
            testRepo.Setup(r => r.GetAll()).Returns(data);

            Assert.That(testRepo.Object.GetByName("TV Series") == 0);
        }

        [Test]
        public void GetByName_NoTypeInDb_CreatesType()
        {
            var data = new List<MovieType>() { new MovieType() { Name = "Drama"} }.AsQueryable();
            var mockDbSet = new Mock<DbSet<MovieType>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<MovieTypeRepository>(mockDb.Object) { CallBase = true };
            testRepo.Setup(r => r.GetAll()).Returns(data);
            testRepo.Setup(r => r.Add(It.IsAny<MovieType>())).Callback( (MovieType val) => data = data.Append(val).AsQueryable() );
            var testName = "TV Series";

            testRepo.Object.GetByName(testName);

            Assert.That(data.Last().Name == testName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetByName_NullOrEmptyStringPassed_ThrowsArgumentNullException(string name)
        {
            var data = new List<MovieType>() { new MovieType() { Name = "TV Series", Id = 0 } }.AsQueryable();
            var mockDbSet = new Mock<DbSet<MovieType>>();
            var mockDb = SetUpMockDb(data, mockDbSet);
            var testRepo = new Mock<MovieTypeRepository>(mockDb.Object) { CallBase = true };

            Assert.Throws<ArgumentNullException>(() => testRepo.Object.GetByName(name));
        }
    }
}
