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
public class ReportsController : ControllerBase
{
    private readonly EmployeeDbContext _context;

    public ReportsController(EmployeeDbContext context)
    {
        _context = context;
    }

    [HttpGet("weekly")]
    public async Task<ActionResult<IEnumerable<Tasks>>> GetWeeklyReport()
    {
        var oneWeekAgo = DateTime.Now.AddDays(-7);
        var tasks = await _context.Tasks
            .Where(t => t.DueDate >= oneWeekAgo && t.IsCompleted)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<IEnumerable<Tasks>>> GetMonthlyReport()
    {
        var oneMonthAgo = DateTime.Now.AddMonths(-1);
        var tasks = await _context.Tasks
            .Where(t => t.DueDate >= oneMonthAgo && t.IsCompleted)
            .ToListAsync();

        return Ok(tasks);
    }
}
