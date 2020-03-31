using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HockeyApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyApp.API.Controllers
{
    [Authorize] // Authorization, must be logged in. (attribute)
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        //Read only with _ before the name
        private readonly DataContext _context;

        //Inject the data context here by creating a constructor an an instance variable
        public ValuesController(DataContext context)
        {
            this._context = context;

        }
        // GET api/values (use the http)
        //Use asynch whenever possible.  Easy to implement, and inexpensive.
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.Values.ToListAsync();

            return Ok(values);
        }

        // GET api/values/5
        //Return Null is better than returning an exception since exceptions are expensive
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}