using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMed.API.Utils;
using SmartMed.Database;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClientEntities = SmartMed.Client.Entities;
using DatabaseEntities = SmartMed.Database.Entities;

namespace SmartMed.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationsController : ControllerBase
    {
        private readonly SmartMedContext _context;

        public MedicationsController(SmartMedContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Medication.AsNoTracking());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientEntities.Medication data)
        {
            if (data == null)
            {
                return BadRequest(new HttpResponseMessage("A request body is required."));
            }

            if (string.IsNullOrEmpty(data.Name))
            {
                return BadRequest(new HttpResponseMessage("The name can not be null."));
            }

            if (await _context.Medication.AsNoTracking().AnyAsync(x => x.Name.Equals(data.Name)))
            {
                return Conflict(new HttpResponseMessage("There is already a project with this name."));
            }

            if (data.Quantity <= 0)
            {
                return BadRequest(new HttpResponseMessage("The quantity must be greater than zero."));
            }

            await _context.Medication.AddAsync(new DatabaseEntities.Medication
            {
                Name = data.Name,
                Quantity = data.Quantity
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new HttpResponseMessage("A valid Id is required."));
            }

            Expression<Func<DatabaseEntities.Medication, bool>> expr = x => x.Id.ToString().Equals(id);

            if (!await _context.Medication.AsNoTracking().AnyAsync(expr))
            {
                return NotFound();
            }

            _context.Medication.Remove(await _context.Medication.AsNoTracking().FirstAsync(expr));
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
