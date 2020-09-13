using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
                    var token = "Bearer " + data;
                    var info = DecodeToken(data);
                    if (data != "")
                    {
                        //var json = JsonConvert.DeserializeObject(data).ToString();
                        //var account = JsonConvert.DeserializeObject<UserVM>(json);



                        if (BCrypt.Net.BCrypt.Verify(userVM.Password, info[2]) && (info[3] == "Admin" || info[3] == "Sales") && info[4] == "false")
                        {
                            HttpContext.Session.SetString("id", info[5]);
                            HttpContext.Session.SetString("uname", info[0]);
                            HttpContext.Session.SetString("email", info[1]);
                            HttpContext.Session.SetString("lvl", info[3]);
                            HttpContext.Session.SetString("token", token);
                            //HttpContext.Session.SetString("everif", account.code);

                            if (HttpContext.Session.GetString("lvl") == "Sales")
                            {

                                return Json(new { status = true, msg = "Login Successfully !" });
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Login Successfully !" });
                            }
                        }
                        else if (BCrypt.Net.BCrypt.Verify(userVM.Password, info[2]) && (info[3] == "Admin" || info[3] == "Sales") && info[0] != "false")
                        { 
                            HttpContext.Session.SetString("id", info[5]);
                            HttpContext.Session.SetString("uname", info[0]);
                            HttpContext.Session.SetString("email", info[1]);
                            HttpContext.Session.SetString("lvl", info[3]);
                            HttpContext.Session.SetString("everif", info[4]);
                            HttpContext.Session.SetString("token", token);
                            if (HttpContext.Session.GetString("lvl") == "Sales")
                            {
                                return Json(new { status = "everif", msg = "param1" });
                            }
                            else
                            {
                                return Json(new { status = true, msg = "Login Successfully !" });
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
                    userVM.UserName = null;
                    Validate(userVM);
                    return Json(new { status = "everif", msg = "Account has been created please login!" });
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
            if (HttpContext.Session.GetString("everif") == code)
            {
                var user = new UserVM();
                user.code = code;
                var json = JsonConvert.SerializeObject(user);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync("user/verifycode/", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }

            }
            return Redirect("/everif");
        }

        protected List<string> DecodeToken(string token)
        {
            List<string> result = new List<string>();
            string rawKey = "febrioibrahimsebayang";
            var key = Encoding.ASCII.GetBytes(rawKey);
            var handler = new JwtSecurityTokenHandler();
            var validation = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var claims = handler.ValidateToken(token, validation, out var tokenSecure);
            IEnumerable<Claim> data = claims.Claims;
            result.Add(data.SingleOrDefault(p => p.Type == "Username").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Email").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Password").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "RoleName").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "code").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Id").Value);
            return result;
        }

        public ActionResult GetUser()
        {
            var id = HttpContext.Session.GetString("id");
            DetailsVM detail = null; 

            var resTask = client.GetAsync("User/Details/" + id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<DetailsVM>();
                readTask.Wait();

                detail = readTask.Result;
            }
            var response = Tuple.Create(detail, result);

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings());
        }

    }
}