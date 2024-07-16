using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourMvcProject.Controllers
{
    public class NotesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.GetAsync("https://localhost:5203/api/notes");

                if (response.IsSuccessStatusCode)
                {
                    var notesJson = await response.Content.ReadAsStringAsync();
                    var notes = JsonSerializer.Deserialize<List<Note>>(notesJson);

                    return View("NotePage", notes);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.GetAsync($"https://localhost:5203/api/notes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var noteJson = await response.Content.ReadAsStringAsync();
                    var note = JsonSerializer.Deserialize<Note>(noteJson);

                    return View("NoteDetails", note);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> NotesForTask(int taskId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.GetAsync($"https://localhost:5203/api/notes/task/{taskId}");

                if (response.IsSuccessStatusCode)
                {
                    var notesJson = await response.Content.ReadAsStringAsync();
                    var notes = JsonSerializer.Deserialize<List<Note>>(notesJson);

                    return View("NotesForTask", notes);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var noteJson = JsonSerializer.Serialize(note);
                var content = new StringContent(noteJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://localhost:5203/api/notes", content);

                if (response.IsSuccessStatusCode)
                {
                    var createdNoteJson = await response.Content.ReadAsStringAsync();
                    var createdNote = JsonSerializer.Deserialize<Note>(createdNoteJson);

                    return RedirectToAction("Details", new { id = createdNote.NoteId });
                }
                else
                {
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }
    }
}
