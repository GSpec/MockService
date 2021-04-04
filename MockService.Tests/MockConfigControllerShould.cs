using MockService.Controllers;
using MockService.Dtos.Input;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;

namespace MockService.Tests
{
    public class MockConfigControllerShould
    {
        [Fact]
        public void CreateAMock()
        {
            // Arrange
            var controller = new MockConfigController();
            var dto = new Mock
            {
                Request = new Request { Method = "POST", Endpoint = "/test" },
                Response = new Response { Headers = "TestHeaders", Body = "Success"}
            };

            // Act
            var actionResult = controller.Create(dto);

            // Assert
            actionResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = actionResult as OkObjectResult;
            okObjectResult?.Value.Should().BeOfType(typeof(Guid));
        }
    }
}
