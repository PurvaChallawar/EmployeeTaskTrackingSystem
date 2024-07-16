using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class DocumentsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DocumentsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            var response = await httpClient.GetAsync("https://localhost:5203/api/documents");

            if (response.IsSuccessStatusCode)
            {
                var documentsJson = await response.Content.ReadAsStringAsync();
                var documents = JsonSerializer.Deserialize<List<Document>>(documentsJson);

                return View("DocumentPage", documents); 
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
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("File", "Please select a file to upload.");
            return View("DocumentPage");
        }

        var httpClient = _httpClientFactory.CreateClient();
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

        try
        {
            var response = await httpClient.PostAsync("https://localhost:5203/api/documents/upload", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DocumentPage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to upload file to API.");
                return View("DocumentPage");
            }
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, "Failed to communicate with API.");
            return View("DocumentPage");
        }
    }
}
