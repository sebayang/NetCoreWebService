using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebCore.ViewModels;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44327/api/")
        };

        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("validate")]
        public IActionResult Validate(UserVM userVM)
        {
            if (userVM.UserName == null)
            {
                var jsonUserVM = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonUserVM);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resTask = client.PostAsync("user/login/", byteContent);
                resTask.Wait();
                var result = resTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsStringAsync().Result;
                    if (data != "")
                    {
                        var json = JsonConvert.DeserializeObject(data).ToString();
                        var account = JsonConvert.DeserializeObject<UserVM>(json);
                        if (BCrypt.Net.BCrypt.Verify(userVM.Password, account.Password) && (account.RoleName == "Admin" || account.RoleName == "Sales") && account.code == null)
                        {
                            HttpContext.Session.SetString("uname", account.UserName);
                            HttpContext.Session.SetString("email", account.Email);
                            HttpContext.Session.SetString("lvl", account.RoleName);
                            //HttpContext.Session.SetString("everif", account.code);
                            if (HttpContext.Session.GetString("lvl") == "Sales")
                            {

                                return Json(new { status = true, msg = "Login Successfully !", acc = "Admin" });
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Login Successfully !", acc = "Sales" });
                            }
                        }
                        else if (BCrypt.Net.BCrypt.Verify(userVM.Password, account.Password) && (account.RoleName == "Admin" || account.RoleName == "Sales") && account.code != null)
                        {
                            HttpContext.Session.SetString("uname", account.UserName);
                            HttpContext.Session.SetString("email", account.Email);
                            HttpContext.Session.SetString("lvl", account.RoleName);
                            HttpContext.Session.SetString("everif", account.code);
                            if (HttpContext.Session.GetString("lvl") == "Sales")
                            {
                                return Redirect("/everif");
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Login Successfully !", acc = "Sales" });
                            }
                        }
                        else
                        {
                            return Json(new { status = false, msg = "Invalid Username or Password!" });
                        }
                    }
                    else
                    {
                        return Json(new { status = false, msg = "Username Not Found!" });
                    }
                }
                else
                {
                    //return RedirectToAction("Login","Auth");
                    return Json(new { status = false, msg = "Username Not Found!" });
                }
            }
            else if (userVM.UserName != null)
            {
                var json = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync("user/register/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                { 
                    var buffer1 = System.Text.Encoding.UTF8.GetBytes(json);
                    var byteContent1 = new ByteArrayContent(buffer1);
                    byteContent1.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var resTask = client.PostAsync("user/login/", byteContent1);
                    resTask.Wait();
                    var result1 = resTask.Result;
                    var data = result.Content.ReadAsStringAsync().Result;
                    var json1 = JsonConvert.DeserializeObject(data).ToString();
                    var account = JsonConvert.DeserializeObject<UserVM>(json1);
                    HttpContext.Session.SetString("uname", account.UserName);
                    HttpContext.Session.SetString("email", account.Email);
                    HttpContext.Session.SetString("lvl", account.RoleName);
                    HttpContext.Session.SetString("everif", account.code);
                    return Json(new { status = true, code = result, msg = "Register Success! " });
                }
                else
                {
                    return Json(new { status = false, msg = "Something Wrong!" });
                }
            }
            return Redirect("/login");
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Redirect("/login");
        }

        [Route("everif")]
        public IActionResult emailVerification()
        {
            return View();
        }

        [Route("verifemail")]
        public IActionResult verifemail(string code)
        {
            if (HttpContext.Session.GetString("code") == code)
            {
                return Redirect("/home");
            }
            return Redirect("/everif");
        }
    }
}