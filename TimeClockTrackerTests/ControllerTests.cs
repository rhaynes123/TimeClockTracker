using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TimeClockTracker.Controllers;
using TimeClockTracker.Models;
using Microsoft.AspNetCore.Mvc;
using TimeClockTracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TimeClockTrackerTests
{
    public class ControllerTests
    {
        private readonly Mock<ILogger<TimePunchesController>> timepunchlogger = new Mock<ILogger<TimePunchesController>>();
        private readonly Mock<ApplicationDbContext> mockdbcontext = new Mock<ApplicationDbContext>();
        private readonly Mock<UserManager<ApplicationUser>> mockuserManager = new Mock<UserManager<ApplicationUser>>();
        public ControllerTests()
        {

            mockuserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1"});
        }
        // https://www.michalbialecki.com/2020/11/28/unit-tests-in-entity-framework-core-5/
        // https://asp.net-hacker.rocks/2019/01/15/unit-testing-data-access-dotnetcore.html
        // https://medium.com/@samueleresca/unit-testing-asp-net-core-identity-e2b18254cc8a
        // https://docs.microsoft.com/en-us/aspnet/core/test/razor-pages-tests?view=aspnetcore-5.0
        public static DbContextOptions<ApplicationDbContext> TestDbContextOptions()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        [Fact]
        public void GetTimePunchesReturnsCollection()
        {
            using var db = new ApplicationDbContext(TestDbContextOptions(), new Mock<IOptions<IdentityServer4.EntityFramework.Options.OperationalStoreOptions>>().Object);
            var timepunchController = new TimePunchesController(timepunchlogger.Object, db);
            IEnumerable<TimePunch> result = timepunchController.Get();
            Assert.IsAssignableFrom<IEnumerable<TimePunch>>(result);
        }
        [Fact]
        public void GetTimePunchesIsNotEmpty()
        {
            var timepunchController = new TimePunchesController(timepunchlogger.Object,  mockdbcontext.Object);
            IEnumerable<TimePunch> result = timepunchController.Get();
            Assert.NotEmpty(result);
        }
        [Fact]
        public void PostTimePunchReturnsOk()
        {
            var timepunchController = new TimePunchesController(timepunchlogger.Object,  mockdbcontext.Object);
            var result = timepunchController.Post(It.IsAny<TimePunch>());
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
