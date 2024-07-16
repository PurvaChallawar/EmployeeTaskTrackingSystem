using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourMvcProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> WeeklyReport()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:5203/api/reports/weekly");

            if (response.IsSuccessStatusCode)
            {
                var tasksJson = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<List<Task>>(tasksJson);

                return View("TaskPage",tasks);
            }
            else
            {
                return View("TaskPage","Error");
            }
        }

        public async Task<IActionResult> MonthlyReport()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:5203/api/reports/monthly");

            if (response.IsSuccessStatusCode)
            {
                var tasksJson = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<List<Task>>(tasksJson);

                return View("ReportPage",tasks); 
            }
            else
            {
                return View("Error");
            }
        }
    }
}
