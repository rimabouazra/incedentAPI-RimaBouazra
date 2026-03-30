using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using incedentAPI_RimaBouazra.models;

namespace incedentAPI_RimaBouazra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncedentsDBController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        public IncedentsDBController(IncidentsDbContext context)
        {
            _context = context;
        }

        private static readonly string[] AllowedSeverities =
    { "LOW", "MEDIUM", "HIGH", "CRITICAL" };

        private static readonly string[] AllowedStatuses =
            { "OPEN", "IN PROGRESS", "RESOLVED" };
        // GET: api/IncedentsDB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // GET: api/IncedentsDB/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        // PUT: api/IncedentsDB/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.Id)
            {
                return BadRequest();
            }

            _context.Entry(incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IncedentsDB
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            incident.Status = "IN PROGRESS";
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncident", new { id = incident.Id }, incident);
        }
        */
        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            incident.Status = "OPEN";
            incident.CreatedAt = DateTime.Now;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncident", new { id = incident.Id }, incident);
        }
        // DELETE: api/IncedentsDB/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }

        [HttpGet("filter-by-status/{status}")]
        public IActionResult FilterByStatus(string status)
        {
            var l = from inc in _context.Incidents where inc.Status == status
                    select inc ;

            return Ok(l);
        }
        [HttpGet("filter-by-severity/{severity}")]
        public IActionResult FilterBySeverity(string severity)
        {
            var l = from inc in _context.Incidents
                    where inc.Severity == severity
                    select inc;

            return Ok(l);
        }

        [HttpGet("filter-by-status-async/{status}")]
        public async Task<IActionResult> FilterByStatusAsync(string status)
        {
            var incidents = await _context.Incidents
                .Where(i => i.Status.Contains(status))
                .ToListAsync();

            return Ok(incidents);
        }

        [HttpGet("filter-by-severity-async/{severity}")]
        public async Task<IActionResult> FilterBySeverityAsync(string severity)
        {
            var incidents = await _context.Incidents
                .Where(i => i.Severity.Contains(severity))
                .ToListAsync();

            return Ok(incidents);
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PutIncidentStatus(int id, string status)
        {
            if (!AllowedStatuses.Contains(status.ToUpper()))
            {
                return BadRequest($"Status must be one of the following: {string.Join(", ", AllowedStatuses)}");
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound();

            incident.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
