using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebCore.Contexts;
using WebCore.Models;
using WebCore.ViewModels;

namespace WebCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;

        private Random _random = new Random();

        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }

        private readonly MyContext _context;
        public UserController(MyContext myContext, IConfiguration iconfiguration)
        {
            _context = myContext;
            _configuration = iconfiguration;
        }

        // GET api/values
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        //public async Task<List<User>> GetAll()
        public List<UserVM> GetAll()
        {
            List<UserVM> list = new List<UserVM>(); 
            foreach (var user in _context.Users)
            {
                var roleu = _context.UserRoles.Where(ru => ru.User.Id == user.Id).FirstOrDefault();
                var role = _context.Roles.Where(r => r.Id == roleu.RoleId).FirstOrDefault();
                UserVM userVM = new UserVM()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    Phone = user.PhoneNumber, 
                    RoleName = role.Name
                }; 
                list.Add(userVM);
            }
            return list; 
        }

        [HttpGet("{id}")]
        public UserVM GetID(string id)
        {
            var getId = _context.Users.Find(id); 
            var roleu = _context.UserRoles.Where(ru => ru.User.Id == id).FirstOrDefault();
            var role = _context.Roles.Where(r => r.Id == roleu.RoleId).FirstOrDefault(); 
            UserVM user = new UserVM()
            {
                Id = getId.Id,
                UserName = getId.UserName,
                Email = getId.Email,
                Password = getId.PasswordHash,
                Phone = getId.PhoneNumber,
                RoleName = role.Name
            }; 
            return user;
        }



        [HttpPost]
        public IActionResult Create(UserVM userVM)
        {
            try
            {
                //instance role dan hasing password
                userVM.RoleName = "Sales";
                var hasPass = BCrypt.Net.BCrypt.HashPassword(userVM.Password, 12);
                //create object  
                var user = new User();
                var roleUser = new UserRole();
                var role = _context.Roles.Where(r => r.Name == userVM.RoleName).FirstOrDefault();
                //Pengiriman code verifikasi ke email pendaftar
                var code = GenerateRandomNo();
                MailMessage mail = new MailMessage("rio.mii.b36@gmail.com", userVM.Email);
                string today = DateTime.Now.ToString();
                mail.Subject = "Register Code (" + today + ")";
                mail.Body = string.Format("Hi {0},<br /><br />This is your verification E-Mail Code : <br />{1}<br /><br />Thank You.", userVM.Email, code);
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential NC = new System.Net.NetworkCredential();
                NC.UserName = "rio.mii.b36@gmail.com";
                NC.Password = "bootcamp36";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NC;
                smtp.Port = 587;
                smtp.Send(mail);
                //create
                user.UserName = userVM.UserName;
                user.Email = userVM.Email;
                user.EmailConfirmed = false;
                user.PasswordHash = hasPass;
                user.PhoneNumber = userVM.Phone;
                user.PhoneNumberConfirmed = false;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.NormalizedEmail = code;

                roleUser.User = user;
                roleUser.Role = role; 

                var emp = new Employee
                {
                    EmpId = user.Id,
                    CreateTime = DateTimeOffset.Now,
                    IsDelete = false
                };

                _context.Employees.Add(emp);
                _context.UserRoles.AddAsync(roleUser);
                _context.Users.AddAsync(user);
                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            catch (Exception)
            {

                return BadRequest(500);
            }
            
            //return data;
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {  

            var role = _context.Roles.Where(r => r.Name == userVM.RoleName).FirstOrDefault();
            var getId = _context.Users.Find(id); 
            getId.UserName = userVM.UserName;
            getId.Email = userVM.Email;
            var isValid = BCrypt.Net.BCrypt.Verify(userVM.Password, getId.PasswordHash);
            if (isValid) { }
            else
            {
                var hasPass = BCrypt.Net.BCrypt.HashPassword(userVM.Password, 12);
                getId.PasswordHash = hasPass;
            }
            getId.PhoneNumber = userVM.Phone;  
            var oldRU = _context.UserRoles.Where(ru => ru.UserId == id).FirstOrDefault();
            oldRU.Role = role; 
            _context.SaveChanges();
            return Ok("Successfully Update");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if(id != null){
                var geget = _context.UserRoles.Where(a => a.UserId == id).FirstOrDefault();
                var getId = _context.Users.Find(id);
                _context.UserRoles.Remove(geget);
                _context.Users.Remove(getId);
                _context.SaveChanges();
                return Ok("Successfully Delete");
            }
            return Ok("Delete Failed");

        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserVM userVM)
        {
            var user1 = _context.Users.Where(us => us.Email == userVM.Email).FirstOrDefault();
            var getUserRole = _context.UserRoles.Include("User").Include("Role").SingleOrDefault(x => x.User.Email == userVM.Email);
            if (user1 == null )
            {
                return BadRequest(500);
            }else {
                var cek = BCrypt.Net.BCrypt.Verify(userVM.Password , user1.PasswordHash);
                if (cek)
                {
                    var user = new UserVM();
                    user.Id = getUserRole.User.Id;
                    user.UserName = getUserRole.User.UserName;
                    user.Email = getUserRole.User.Email;
                    user.Password = getUserRole.User.PasswordHash; 
                    user.RoleName = getUserRole.Role.Name;
                    user.code = getUserRole.User.NormalizedEmail;
                    if (user.code == null)
                    {
                        user.code = "false";
                    }
                    if (user != null)
                    {
                        var claims = new List<Claim> {
                            new Claim("Username", user.UserName),
                            new Claim("Password", user.Password),
                            new Claim("Email", user.Email),
                            new Claim("RoleName", user.RoleName),
                            new Claim("code", user.code),
                            new Claim("Id", user.Id) 
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(15) , signingCredentials: signIn);

                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                    else
                    {
                        return BadRequest(500);
                    }
                    return StatusCode(200, user);
                }
                return BadRequest(500);
            } 
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                this.Create(userVM);
                return StatusCode(200);
            }
            return BadRequest(500);
        }

        [HttpPost]
        [Route("verifycode")]
        public IActionResult Verify(UserVM user)
        {
            if (ModelState.IsValid)
            {
                var getId = _context.Users.Where(u => u.NormalizedEmail == user.code).SingleOrDefault();
                getId.NormalizedEmail = null;
                _context.SaveChanges();
                return StatusCode(200);
            }
            return BadRequest(500);
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var detail = await _context.UserRoles.Include("User").Include("Role").Where(u => u.User.Id == id).SingleOrDefaultAsync();
            var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
            DetailsVM details = new DetailsVM()
            {
                Id = id,
                Email = detail.User.Email,
                Username = detail.User.UserName,
                RoleName = detail.Role.Name,
                Phone = detail.User.PhoneNumber,
                Address = getData.Address,
                CreateTime = getData.CreateTime,
                UpdateTime = getData.UpdateTime 
            };
            return Ok(details);
        }
    }
}