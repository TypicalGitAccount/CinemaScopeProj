using Moq;
using MovieService.Entities;
using MovieService.Imdb;
using MovieService.Interfaces;
using MovieService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CinemaScope.Tests.Movies.Imdb
{
    [TestFixture]
    public class ImdbServiceTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<CustomHttpClient> mockHttpClient;
        private ImdbService testService;

        [SetUp]
        public void SetUp()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockHttpClient = new Mock<CustomHttpClient>();
            testService = new ImdbService(mockUnitOfWork.Object, mockHttpClient.Object);
        }


        [TestCase(null)]
        [TestCase("")]
        public void GetMovieById_PassedNullOrEmptyString_ReturnsFalse(string movieId)
        {
            Assert.IsFalse(testService.GetMovieByImdbId(movieId));
        }
    }
}