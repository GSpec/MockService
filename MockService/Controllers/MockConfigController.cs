using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockLogic;
using MockLogic.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dto = MockService.Dtos.Input;
using Model = MockLogic.Models;

namespace MockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MockConfigController : ControllerBase
    {
        private readonly MockServiceDbContext _mockDb;
        public MockConfigController(MockServiceDbContext mockDb)
        {
            _mockDb = mockDb;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Dto.Mock input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Enum.TryParse(typeof(Method), input.Request.Method, out var method);
            var mock = new Model.Mock
            {
                Method = (Method)method,
                Endpoint = input.Request.Endpoint,
                ResponseHeaders = input.Response.Headers,
                ResponseBody = input.Response.Body
            };

            await _mockDb.Mocks.AddAsync(mock);
            await _mockDb.SaveChangesAsync();

            return Ok(mock.Id);
        }

        [HttpPut]
        [Route("{reference}")]
        public async Task<IActionResult> UpdateAsync(Guid reference, [FromBody] Dto.Mock input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mock = await _mockDb.Mocks.FirstOrDefaultAsync(m => m.Id == reference);
            mock.ResponseHeaders = input.Response.Headers;
            mock.ResponseBody = input.Response.Body;

            _mockDb.Mocks.Update(mock);
            await _mockDb.SaveChangesAsync();

            return Ok(mock.Id);
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAsync()
        {
            var mocks = await _mockDb.Mocks.ToListAsync();

            return Ok(mocks);
        }

        [HttpGet]
        [Route("{reference}")]
        public async Task<IActionResult> RetrieveAsync(Guid reference)
        {
            var mock = await _mockDb.Mocks.FirstOrDefaultAsync(m => m.Id == reference);

            if (mock == null) return NotFound();

            return Ok(mock);
        }

        [HttpDelete]
        [Route("{reference}")]
        public async Task<IActionResult> DeleteAsync(Guid reference)
        {
            var mock = await _mockDb.Mocks.FirstOrDefaultAsync(m => m.Id == reference);

            _mockDb.Mocks.Remove(mock);
            await _mockDb.SaveChangesAsync();

            return Ok();
        }
    }
}
