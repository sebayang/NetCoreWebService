﻿using System;
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
    public class DivisionsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44327/api/")
        };
        [Route("divisions")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("token") != null && HttpContext.Session.GetString("lvl") == "Admin")
            {
                return View("~/Views/Home/Divisions.cshtml");
            }
            else
            {
                return Redirect("/error");
            }

        }

        public IActionResult LoadDivisions()
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                IEnumerable<Division> divisions;
                var restask = client.GetAsync("division");
                restask.Wait();
                var result = restask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Division>>();
                    readTask.Wait();
                    divisions = readTask.Result;
                }
                else
                {
                    divisions = Enumerable.Empty<Division>();
                    ModelState.AddModelError(string.Empty, "Error Load Departments");
                }
                return Json(divisions);
            }
            else
            {
                return Redirect("/error");
            }

        }
        public IActionResult GetDivision(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            Division division;
            var restask = client.GetAsync("division/" + id);
            restask.Wait();
            var result = restask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Division>();
                readTask.Wait();
               division = readTask.Result;
            }
            else
            {
                division = null;
                ModelState.AddModelError(string.Empty, "Error Load Department");
            }
            return Json(division);
        }

        public IActionResult InsertOrUpdateDivision(int id, Division division)
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
                var json = JsonConvert.SerializeObject(division);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (division.Id == 0)
                {
                    var result = client.PostAsync("division", byteContent).Result;
                    return Ok(200);
                }
                else if (division.Id == id)
                {
                    var result = client.PutAsync("division/" + id, byteContent).Result;
                    return Ok(200);
                }
                return BadRequest(404);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult DeleteDivision(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("token"));
            var result = client.DeleteAsync("division/" + id).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(200);
            }
            return BadRequest(500);
        }
    }
}