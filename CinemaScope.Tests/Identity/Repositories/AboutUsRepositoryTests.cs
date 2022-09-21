using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using Identity.Interfaces;
using Identity.Contexts;
using Identity.Models;
using System.Data.Entity;
using Identity.Repositories;

namespace CinemaScope.Tests.Identity.Repositories
{
    [TestFixture]
    public class AboutUsRepositoryTests
    {
        IdentityContext _context;
        IAboutUsRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var fakeContext = new Mock<IdentityContext>();
            fakeContext.Object.AboutUsers = SetUpAboutUs<AboutUser>().Object;
            fakeContext.Setup(d => d.AboutUsers).Returns(SetUpAboutUs<AboutUser>().Object);
            _context = fakeContext.Object;           
            _repository = new AboutUsRepository(_context);
        }

        [Test]
        [TestCase(null)]
        public void Create_NullableAboutUserObject_ThrowsArgumentNullException(AboutUser item)
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Create(item));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("        ")]
        public void Create_NullableAboutUserName_ThrowsArgumentNullException(string name)
        {
            var item = new AboutUser() { Name = name, Description = "description" };
            Assert.Throws<ArgumentNullException>(() => _repository.Create(item));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("        ")]
        public void Create_NullableAboutUserDescription_ThrowsArgumentNullException(string description)
        {
            var item = new AboutUser() { Name = "name", Description = description };
            Assert.Throws<ArgumentNullException>(() => _repository.Create(item));
        }

        [Test]
        public void Create_ValidAboutUser_OneMoreObjectInContext()
        {
            var item = new AboutUser() { Name = "name", Description = "description" };
            var countBefore = _context.AboutUsers.Count();
            _repository.Create(item);
            var countAfter = _context.AboutUsers.Count();
            Assert.AreEqual(countBefore + 1, countAfter);
        }

        [Test]
        [TestCase(null)]
        public void Update_NullableAboutUserObject_ThrowsArgumentNullException(AboutUser item)
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Update(item));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("        ")]
        public void Update_NullableAboutUserName_ThrowsArgumentNullException(string name)
        {
            var item = new AboutUser() { Name = name, Description = "description" };
            Assert.Throws<ArgumentNullException>(() => _repository.Update(item));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("        ")]
        public void Update_NullableAboutUserDescription_ThrowsArgumentNullException(string description)
        {
            var item = new AboutUser() { Name = "name", Description = description };
            Assert.Throws<ArgumentNullException>(() => _repository.Update(item));
        }        

        [Test]
        [TestCase(1)]
        public void DeleteById_ValidId_OneLessObjectInContext(int id)
        {
            var countBefore = _context.AboutUsers.Count();
            _repository.DeleteById(id);
            var countAfter = _context.AboutUsers.Count();
            Assert.AreEqual(countBefore - 1, countAfter);
        }

        [Test]
        [TestCase(100)]
        [TestCase(-10)]
        public void DeleteById_NotValidId_TheSameContext(int id)
        {
            var countBefore = _context.AboutUsers.Count();
            _repository.DeleteById(id);
            var countAfter = _context.AboutUsers.Count();
            Assert.AreEqual(countBefore, countAfter);
        }


        private Mock<DbSet<AboutUser>> SetUpAboutUs<T>() where T : AboutUser
        {
            var sourceList = new List<AboutUser>
            {
                new AboutUser{
                    Id = 1,
                    Name = "user1",
                    Description = "user1 Description"
                },
                new AboutUser{
                    Id = 2,
                    Name = "user2",
                    Description = "user2 Description"
                },
                new AboutUser{
                    Id = 3,
                    Name = "user3",
                    Description = "user3 Description"
                }
            };
            var queryable = sourceList.AsQueryable();
            var aboutUsDbSet = new Mock<DbSet<AboutUser>>();
            aboutUsDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            aboutUsDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            aboutUsDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            aboutUsDbSet.As<IQueryable<AboutUser>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            aboutUsDbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            aboutUsDbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            return aboutUsDbSet;
        }
    }
}
