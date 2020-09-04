using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        public IActionResult index()
        {
            var role = HttpContext.Session.GetString("lvl");
            if (HttpContext.Session.GetString("lvl") == "Sales")
            {
                return View();
            }
            return Redirect("/login");
        }
    }
}
