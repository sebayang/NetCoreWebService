using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebCore.ViewModels;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44327/api/")
        };

        [Route("home")]
        public IActionResult index()
        {
            var role = HttpContext.Session.GetString("lvl");
            if (HttpContext.Session.GetString("lvl") == "Admin")
            {
                return View();
            }else if(HttpContext.Session.GetString("lvl") != null)
            {
                return View();
            }
            return Redirect("/error");
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            return View("~/Views/Home/Profile.cshtml");
        }

        public IActionResult LoadPie()
        {
            IEnumerable<PieChartVM> pie = null; 
            var resTask = client.GetAsync("chart/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<PieChartVM>>();
                readTask.Wait();
                pie = readTask.Result;
            }
            else
            {
                pie = Enumerable.Empty<PieChartVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(pie);
        }
    }
}
