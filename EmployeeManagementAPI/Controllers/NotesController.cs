using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly EmployeeDbContext _context;

    public NotesController(EmployeeDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Note>> AddNote(Note note)
    {
        note.CreatedAt = DateTime.Now;
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNote), new { id = note.NoteId }, note);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var note = await _context.Notes.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return note;
    }

    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes(int taskId)
    {
        var notes = await _context.Notes.Where(n => n.TaskId == taskId).ToListAsync();
        return Ok(notes);
    }
}
