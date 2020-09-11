using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebCore.Contexts;
using WebCore.ViewModels;

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly MyContext _context;
        public EmployeeController(MyContext myContext, IConfiguration iconfiguration)
        {
            _context = myContext;
            _configuration = iconfiguration;
        }

        public async Task<List<EmployeeVM>> GetAll()
        {
            var getData = await _context.Employees.Include("User").Where(x => x.IsDelete == false).ToListAsync(); 
            List<EmployeeVM> list = new List<EmployeeVM>();
            foreach (var employee in getData)
            { 
                EmployeeVM emp = new EmployeeVM()
                {
                    EmpId = employee.User.Id,
                    Username = employee.User.UserName,
                    Address = employee.Address,
                    Phone = employee.User.PhoneNumber,
                    CreateTime = employee.CreateTime,
                    UpdateTime = employee.UpdateTime,
                    DeleteTime = employee.DeleteTime 
                };
                list.Add(emp);
            }
            return list;
        }

        [HttpGet("{id}")]
        public EmployeeVM GetID(string id)
        {
            var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
            EmployeeVM emp = new EmployeeVM()
            {
                EmpId = getData.EmpId,
                Username = getData.User.UserName,
                Address = getData.Address,
                Phone = getData.User.PhoneNumber,
                CreateTime = getData.CreateTime,
                UpdateTime = getData.UpdateTime,
                DeleteTime = getData.DeleteTime
            };
            return emp;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
                if (getData == null)
                {
                    return BadRequest("Not Seccessfully");
                }

                getData.DeleteTime = DateTimeOffset.Now;
                getData.IsDelete = true;


                _context.Entry(getData).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok("Successfully Delete");
            }
            return Ok("Delete Failed");

        }
    }
}