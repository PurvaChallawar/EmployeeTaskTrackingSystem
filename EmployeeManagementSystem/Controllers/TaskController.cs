using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourMvcProject.Controllers
{
    public class TasksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TasksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:5203/api/tasks");

            if (response.IsSuccessStatusCode)
            {
                var tasksJson = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<List<Tasks>>(tasksJson);

                return View("TasksPage",tasks); 
            }
            else
            {
                return View("Error");
            }
        }
    }
}
