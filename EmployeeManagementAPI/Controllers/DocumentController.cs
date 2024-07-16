using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly EmployeeDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DocumentsController(EmployeeDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpPost("{taskId}")]
    public async Task<IActionResult> UploadDocument(int taskId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine(_environment.ContentRootPath, "uploads", file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var document = new Document
        {
            TaskId = taskId,
            FilePath = path,
            UploadedAt = DateTime.Now
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(new { document.DocumentId });
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(int taskId)
    {
        var documents = await _context.Documents.Where(d => d.TaskId == taskId).ToListAsync();
        return Ok(documents);
    }
}
