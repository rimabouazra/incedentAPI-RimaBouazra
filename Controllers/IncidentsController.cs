using incedentAPI_RimaBouazra.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace incedentAPI_RimaBouazra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;

        private static readonly string[] AllowedSeverities =
            { "LOW", "MEDIUM", "HIGH", "CRITICAL" };

        private static readonly string[] AllowedStatuses =
            { "OPEN", "IN_PROGRESS", "RESOLVED" };

        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            if (!AllowedSeverities.Contains(incident.Severity))
                return BadRequest("Invalid severity");

            incident.Id = _nextId++;
            incident.Status = "OPEN";
            incident.CreatedAt = DateTime.Now;

            _incidents.Add(incident);

            return Ok(incident);
        }

        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
                return NotFound();

            return Ok(incident);
        }

        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
                return NotFound();

            if (!AllowedStatuses.Contains(status))
                return BadRequest("Invalid status");

            incident.Status = status;
            return Ok(incident);
        }

        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
                return NotFound();

            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
                return BadRequest("Cannot delete critical open incident");

            _incidents.Remove(incident);
            return NoContent();
        }

        [HttpGet("filter-by-status")]
        public IActionResult FilterByStatus([FromQuery] string status)
        {
            var result = _incidents
                .Where(i => i.Status.Contains(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }

        [HttpGet("filter-by-severity")]
        public IActionResult FilterBySeverity([FromQuery] string severity)
        {
            var result = _incidents
                .Where(i => i.Severity.Contains(severity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }

    }
}
