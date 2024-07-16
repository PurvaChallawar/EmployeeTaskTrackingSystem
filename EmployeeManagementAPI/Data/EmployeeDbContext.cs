using EmployeeManagementAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Account> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
