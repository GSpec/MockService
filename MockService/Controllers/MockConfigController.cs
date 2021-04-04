using Microsoft.AspNetCore.Mvc;
using MockLogic;
using MockLogic.Models.Enums;
using System;
using System.Threading;
using Dto = MockService.Dtos.Input;
using Model = MockLogic.Models;

namespace MockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MockConfigController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] Dto.Mock input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            Enum.TryParse(typeof(Method), input.Request.Method, out var method);
            var mock = new Model.Mock((Method)method, input.Request.Endpoint, input.Response.Headers, input.Response.Body);

            MockStore.CreateOrUpdate(mock);

            return Ok(mock.Reference);
        }

        [HttpPut]
        [Route("{reference}")]
        public IActionResult Update(Guid reference, [FromBody] Dto.Mock input)
        {
            var mock = MockStore.Retrieve(reference);

            if (mock == null)
            {
                return NotFound();
            }

            if (input == null)
            {
                return BadRequest();
            }

            mock.Response.Headers = input.Response.Headers;
            mock.Response.Body = input.Response.Body;

            MockStore.CreateOrUpdate(mock);

            return Ok(mock.Reference);
        }

        [HttpGet]
        public IActionResult Retrieve()
        {
            var mocks = MockStore.RetrieveAll();

            return Ok(mocks);
        }

        [HttpGet]
        [Route("{reference}")]
        public IActionResult Retrieve(Guid reference)
        {
            var mock = MockStore.Retrieve(reference);

            if (mock == null)
            {
                return NotFound();
            }

            return Ok(mock);
        }

        [HttpDelete]
        [Route("{reference}")]
        public IActionResult Delete(Guid reference)
        {
            MockStore.Delete(reference);

            return Ok();
        }
    }
}
