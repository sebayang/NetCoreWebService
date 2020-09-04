using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCore.Contexts;
using WebCore.Models;
using WebCore.ViewModels;

namespace WebCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private Random _random = new Random();

        public string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }
        private readonly MyContext _context;
        public UserController(MyContext myContext)
        {
            _context = myContext;
        }

        // GET api/values
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
            userVM.RoleName = "Sales"; 
            var hasPass = BCrypt.Net.BCrypt.HashPassword(userVM.Password, 12);

            var user = new User();
            var roleUser = new UserRole();
            var role = _context.Roles.Where(r => r.Name == userVM.RoleName).FirstOrDefault();
            //email
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


            _context.UserRoles.AddAsync(roleUser);
            _context.Users.AddAsync(user);
            _context.SaveChanges();

            return Ok("Successfully Created");
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
            if (isValid) { Ok("Failed Update"); }
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
            var geget = _context.UserRoles.Where(a => a.UserId == id).FirstOrDefault();
            var getId = _context.Users.Find(id);
            _context.UserRoles.Remove(geget);
            _context.Users.Remove(getId);
            _context.SaveChanges();
            return Ok("Successfully Delete");
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
                    user.UserName = getUserRole.User.UserName;
                    user.Email = getUserRole.User.Email;
                    user.Password = getUserRole.User.PasswordHash; 
                    user.RoleName = getUserRole.Role.Name;
                    user.code = getUserRole.User.NormalizedEmail;
                    return StatusCode(200, user);
                }
                return BadRequest(500);
            }
            
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                this.Create(userVM);
                return StatusCode(200);
            }
            return BadRequest(500);
        }

         
    }
}