using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore.Contexts;
using WebCore.Models;

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly MyContext _context;
        public UserRoleController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        //public async Task<List<User>> GetAll()
        public List<UserRole> GetAll()
        {
            List<UserRole> list = new List<UserRole>();
            foreach (var item in _context.UserRoles)
            {
                list.Add(item);
            }
            return list;
        }

        [HttpGet("{id}")]
        public UserRole GetID(int id)
        {
            return _context.UserRoles.Find(id);
        }

        [HttpPost]
        public IActionResult Create(UserRole userRole)
        {
            var userRoles = new UserRole();
            userRoles.Id = userRole.Id;
            userRoles.Role = userRole.Role;
            userRoles.User = userRole.User;
            _context.UserRoles.AddAsync(userRoles);
            _context.SaveChanges();
            return Ok("Successfully Created");
            //return data;
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserRole userRole)
        {
            var getId = _context.UserRoles.Find(id);
            getId.Id = userRole.Id;
            getId.Role = userRole.Role;
            getId.User = userRole.User;
            _context.SaveChanges();
            return Ok("Successfully Update");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getId = _context.UserRoles.Find(id);
            _context.UserRoles.Remove(getId);
            _context.SaveChanges();
            return Ok("Successfully Delete");
        }
    }
}