using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore.Models;
using WebCore.ViewModels;

namespace WebClient.Controllers
{
    public class EmployeeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44327/api/")
        };
        [Route("employee")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Employees.cshtml");
        }

        public IActionResult LoadEmployees()
        {
            if (HttpContext.Session.GetString("token") != null && HttpContext.Session.GetString("lvl") == "Admin")
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                IEnumerable<EmployeeVM> employees;
                var restask = client.GetAsync("employee");
                restask.Wait();
                var result = restask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<EmployeeVM>>();
                    readTask.Wait();
                    employees = readTask.Result;
                }
                else
                {
                    employees = Enumerable.Empty<EmployeeVM>();
                    ModelState.AddModelError(string.Empty, "Error Load Departments");
                }
                return Json(employees);
            }
            else
            {
                return Redirect("/error");
            }

        }

        public IActionResult GetEmployee(string id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            EmployeeVM employee;
            var restask = client.GetAsync("employee/" + id);
            restask.Wait();
            var result = restask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<EmployeeVM>();
                readTask.Wait();
                employee = readTask.Result;
            }
            else
            {
                employee = null;
                ModelState.AddModelError(string.Empty, "Error Load Department");
            }
            return Json(employee);
        }
        public IActionResult DeleteEmployee(string id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var result = client.DeleteAsync("employee/" + id).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(200);
            }
            return BadRequest(500);
        }
    }
}