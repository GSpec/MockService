using Microsoft.AspNetCore.Mvc;
using MockLogic;
using MockLogic.Models;
using MockLogic.Models.Enums;

namespace MockService.Controllers
{
    [ApiController]
    [Route("[controller]/{*endpoint}")]
    public class MockController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public IActionResult ReturnMock()
        {
            var endpoint = Request.Path.Value[5..];

            var request = new Request { Method = Method.POST, Endpoint = endpoint };
            var mock = MockStore.Retrieve(request);

            if (mock is null) return BadRequest("Oops! You've not created a mock for your request yet.");

            return Ok(mock.Response.Body);
        }
    }
}
