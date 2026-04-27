using incedentAPI_RimaBouazra.Controllers;
using incedentAPI_RimaBouazra.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTests
{   [Trait("Category", "Unit")] 
    public class IncidentsTests
    {
        private IncidentsDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<IncidentsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IncidentsDbContext(options);
        }
        [Fact]
        public async Task GetIncidents_WhenDataExists_ReturnsAllIncidents()
        {
            var context = GetDbContext();
            context.Incidents.AddRange(
            new Incident { Title = "Incident1", Status = "OPEN", Severity = "HIGH" },
            new Incident { Title = "Incident2", Status = "CLOSED", Severity = "LOW" }
            );
            context.SaveChanges();
            var controller = new IncedentsDBController(context);
            var result = await controller.GetIncidents();
            var incidents = Assert.IsType<List<Incident>>(result.Value);
            Assert.Equal(2, incidents.Count);
        }
        [Fact]
        public async Task GetIncident_ExistingId_ReturnsIncident()
        {
            var context = GetDbContext();
            var incident = new Incident { Id = 1, Title = "Test", Status = "OPEN" };
            context.Incidents.Add(incident);
            context.SaveChanges();
            var controller = new IncedentsDBController(context);
            var result = await controller.GetIncident(1);
            Assert.NotNull(result.Value);
            Assert.Equal("Test", result.Value.Title);
        }
        [Fact]
        public async Task GetIncidents_ReturnsAll()
        {
            var context = GetDbContext();

            context.Incidents.AddRange(
                new Incident { Title = "A", Status = "OPEN" },
                new Incident { Title = "B", Status = "CLOSED" }
            );
            context.SaveChanges();

            var controller = new IncedentsDBController(context);

            var result = await controller.GetIncidents();

            var data = Assert.IsType<List<Incident>>(result.Value);
            Assert.Equal(2, data.Count);
        }
        [Fact]
        public async Task PostIncident_ValidData_CreatesIncident()
        {
            var context = GetDbContext();
            var controller = new IncedentsDBController(context);
            var incident = new Incident { Title = "NewIncident", Status = "OPEN" };
            var result = await controller.PostIncident(incident);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdIncident = Assert.IsType<Incident>(createdResult.Value);
            Assert.Equal("NewIncident", createdIncident.Title);
            Assert.Equal(1, context.Incidents.Count());
        }
        [Fact]
        public async Task PutIncident_ValidUpdate_ReturnsNoContent()
        {
            var context = GetDbContext();
            var incident = new Incident { Id = 1, Title = "Old", Status = "OPEN" };
            context.Incidents.Add(incident);
            context.SaveChanges();
            var controller = new IncedentsDBController(context);
            incident.Title = "Updated";
            var result = await controller.PutIncident(1, incident);
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated", context.Incidents.First().Title);
        }
        [Fact]
        public async Task PutIncident_IdMismatch_ReturnsBadRequest()
        {
            var context = GetDbContext();
            var controller = new IncedentsDBController(context);
            var incident = new Incident { Id = 2, Title = "Test" };
            var result = await controller.PutIncident(1, incident);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task DeleteIncident_ExistingId_RemovesIncident()
        {
            var context = GetDbContext();
            var incident = new Incident { Id = 1, Title = "DeleteMe" };
            context.Incidents.Add(incident);
            context.SaveChanges();
            var controller = new IncedentsDBController(context);
            var result = await controller.DeleteIncident(1);
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Incidents);
        }
        [Fact]
        public async Task DeleteIncident_NotFound_ReturnsNotFound()
        {
            var context = GetDbContext();
            var controller = new IncedentsDBController(context);
            var result = await controller.DeleteIncident(99);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task FilterBySeverityAsync_ValidSeverity_ReturnsFilteredData()
        {
            var context = GetDbContext();
            context.Incidents.AddRange(
            new Incident { Title = "A", Severity = "HIGH" },
            new Incident { Title = "B", Severity = "LOW" }
            );
            context.SaveChanges();
            var controller = new IncedentsDBController(context);
            var result = await controller.FilterBySeverityAsync("HIGH");
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<List<Incident>>(okResult.Value);
            Assert.Single(data);
        }
        //[Fact]
        [Theory]
        [InlineData("Low")]
        [InlineData("Medium")]
        [InlineData("High")]
        [InlineData("Critical")]
        public async Task PostIncident_ValidSeverity_ReturnsCreated(string severity)
        {
            var context = GetDbContext();
            var controller = new IncedentsDBController(context);
            var incident = new Incident
            {
                Title = "Test",
                Status = "OPEN",
                Severity = severity
            };
            var result = await controller.PostIncident(incident);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdIncident = Assert.IsType<Incident>(createdResult.Value);
            Assert.Equal(severity, createdIncident.Severity);
        }

    }
}
