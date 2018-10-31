using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private readonly DataContext _db;
    public ValuesController(DataContext db)
    {
      _db = db;
    }



    // GET api/values
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      //retreive values from the DB
      var values = await _db.Values.ToListAsync();
      //respond with values list and 200 status
      return Ok(values);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var value = await _db.Values.FirstOrDefaultAsync(val => val.Id == id);
      if (value == null) return NotFound();
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
