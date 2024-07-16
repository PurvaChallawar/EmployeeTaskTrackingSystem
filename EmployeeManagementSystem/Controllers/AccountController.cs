using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EmployeeManagementSystem.Models;


namespace EmployeeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5203/"); 
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(Account account)
        {
            try
            {
                using (var response = await _client.PostAsJsonAsync("api/Users/login", account))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();

                    if (result == "Login successful")
                    {
                        using (var employeeResponse = await _client.GetAsync("api/Employees"))
                        {
                            employeeResponse.EnsureSuccessStatusCode();
                            var employees = await employeeResponse.Content.ReadAsAsync<List<Employee>>();
                            return View("DetailPage", employees);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid credentials");
                        return View("Login");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to authenticate: " + ex.Message);
                return View("Login");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
                using (var response = await _client.PostAsJsonAsync("api/Employees", employee))
                {
                    response.EnsureSuccessStatusCode();
                    var createdEmployee = await response.Content.ReadAsAsync<Employee>(); 
                    return RedirectToAction(nameof(Verify), createdEmployee); 
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to add employee: " + ex.Message);
                return View("DetailPage"); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            try
            {
                using (var response = await _client.PutAsJsonAsync($"api/Employees/{employee.Id}", employee))
                {
                    response.EnsureSuccessStatusCode(); 
                    var updatedEmployee = await response.Content.ReadAsAsync<Employee>();
                    return RedirectToAction(nameof(Verify), updatedEmployee); 
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to update employee: " + ex.Message);
                return View("DetailPage"); 
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                using (var response = await _client.DeleteAsync($"api/Employees/{id}"))
                {
                    response.EnsureSuccessStatusCode(); 
                    return RedirectToAction(nameof(Verify)); 
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete employee: " + ex.Message);
                return View("DetailPage"); 
            }
        }

    }
}
