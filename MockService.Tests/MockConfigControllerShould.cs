using MockService.Controllers;
using MockService.Dtos.Input;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;
using MockLogic;
using Microsoft.EntityFrameworkCore;

namespace MockService.Tests
{
    public class MockConfigControllerShould
    {
        private DbContextOptions<MockServiceDbContext> mockDbOptions = new DbContextOptionsBuilder<MockServiceDbContext>()
            .UseInMemoryDatabase(databaseName: "MockDb")
            .Options;

        [Fact]
        public async void CreateAMock()
        {
            // Arrange
            var mockDb = new MockServiceDbContext(mockDbOptions);
            var controller = new MockConfigController(mockDb);
            var dto = new Mock
            {
                Request = new Request { Method = "POST", Endpoint = "/test" },
                Response = new Response { Headers = "TestHeaders", Body = "Success"}
            };

            // Act
            var actionResult = await controller.CreateAsync(dto);

            // Assert
            actionResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = actionResult as OkObjectResult;
            okObjectResult?.Value.Should().BeOfType(typeof(Guid));
        }
    }
}
