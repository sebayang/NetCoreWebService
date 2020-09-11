using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebCore.Models;

namespace WebClient.Controllers
{
    public class DepartmentsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44327/api/")
        };
        [Route("departments")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("token") != null && HttpContext.Session.GetString("lvl") == "Admin")
            {
                return View("~/Views/Home/Departments.cshtml");
            }
            else
            {
                return Redirect("/error");
            }
            
        }  

        public IActionResult LoadDepartments()
        {
            if (HttpContext.Session.GetString("token") != null && HttpContext.Session.GetString("lvl") == "Admin")
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                IEnumerable<Department> departments;
                var restask = client.GetAsync("department");
                restask.Wait();
                var result = restask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Department>>();
                    readTask.Wait();
                    departments = readTask.Result;
                }
                else
                {
                    departments = Enumerable.Empty<Department>();
                    ModelState.AddModelError(string.Empty, "Error Load Departments");
                }
                return Json(departments);
            }
            else
            {
                return Redirect("/error");
            }
            
        } 
        public IActionResult GetDepartment(int id)
        { 
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            Department department;
            var restask = client.GetAsync("department/" + id);
            restask.Wait();
            var result = restask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Department>();
                readTask.Wait();
                department = readTask.Result;
            }
            else
            {
                department = null;
                ModelState.AddModelError(string.Empty, "Error Load Department");
            }
            return Json(department);
        }

        public IActionResult InsertOrUpdateDepartment(int id, Department department)
        {
            try
            { 
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                var json = JsonConvert.SerializeObject(department);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (department.Id == 0)
                {
                    var result = client.PostAsync("department", byteContent).Result;
                    return Ok(200);
                }
                else if (department.Id == id)
                {
                    var result = client.PutAsync("department/" + id, byteContent).Result;
                    return Ok(200);
                }
                return BadRequest(404);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult DeleteDepartment(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var result = client.DeleteAsync("department/" + id).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(200);
            }
            return BadRequest(500);
        }
    }
}