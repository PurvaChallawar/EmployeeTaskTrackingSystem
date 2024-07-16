using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbContext dbContext;
        public EmployeesController(EmployeeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployee = dbContext.Employees.ToList();
            return Ok(allEmployee);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id) 
        {
            var empInfo = dbContext.Employees.Find(id);
            if (empInfo == null)
            {
                return NotFound("Employee doesn't exist");
            }
            return Ok(empInfo);
        }


        [HttpPost]
        public IActionResult addEmployee(EmployeeDTO employeeDTO)
        {
            var employeeEntity = new Employee() { Name = employeeDTO.Name, Email = employeeDTO.Email, Phone = employeeDTO.Phone, Salary = employeeDTO.Salary};
            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult updateEmployee(Guid id, EmployeeDTO employeeDTO)
        {
            var empInfo = dbContext.Employees.Find(id);
            if (empInfo == null)
            {
                return NotFound("Employee doesn't exist");
            }
            empInfo.Name = employeeDTO.Name;
            empInfo.Email = employeeDTO.Email;
            empInfo.Phone = employeeDTO.Phone;
            empInfo.Salary = employeeDTO.Salary;

            dbContext.SaveChanges();

            return Ok(empInfo);
        }

        [HttpDelete]
        public IActionResult deleteEmployee(Guid id) 
        {
            var empInfo = dbContext.Employees.Find(id);
            if (empInfo == null)
            {
                return NotFound("Employee doesn't exist");
            }

            dbContext.Employees.Remove(empInfo);
            dbContext.SaveChanges();
            return Ok("Deleted Successfully"); 
        }
        [HttpPost("login")]
        public IActionResult Login(Account userDTO)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == userDTO.Username && u.PasswordHash == userDTO.PasswordHash);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login successful");
        }
    }
}
