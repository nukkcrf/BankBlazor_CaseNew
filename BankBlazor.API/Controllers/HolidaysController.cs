using System.Text.Json;
using BankBlazor.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankBlazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;

        public HolidaysController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        [HttpGet("next-scotland")]
        public async Task<IActionResult> GetNextScottishHoliday()
        {
            var client = _httpFactory.CreateClient();
            var res = await client.GetAsync("https://www.gov.uk/bank-holidays.json");
            if (!res.IsSuccessStatusCode) return StatusCode((int)res.StatusCode);

            var json = await res.Content.ReadAsStringAsync();
            // Deserialisera hela dokumentet som en Dictionary<string, Division>
            var root = JsonSerializer.Deserialize<Dictionary<string, Division>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            if (!root.TryGetValue("scotland", out var scotland))
                return NotFound("No Scotland holidays found.");


            // Hitta första händelse med datum i framtiden
            var next = scotland.Events
                .Select(e => new { e.Title, Date = DateTime.Parse(e.Date) })
                .Where(e => e.Date > DateTime.UtcNow.Date)
                .OrderBy(e => e.Date)
                .FirstOrDefault();

            return next is not null
                ? Ok(next)
                : NotFound("No upcoming holidays.");
        }
    }

    // Hjälpklasser för att deserialisera gov.uk‐svaret:
    public class Division
    {
        public string DivisionName { get; set; } = default!;
        public HolidayEvent[] Events { get; set; } = default!;
    }
    public class HolidayEvent
    {
        public string Title { get; set; } = default!;
        public string Date { get; set; } = default!;
    }
}
