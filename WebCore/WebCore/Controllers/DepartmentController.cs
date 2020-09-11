using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore.Base;
using WebCore.Models;
using WebCore.Repositories.Data;

namespace WebCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController<Department, DepartmentRepository>
    {
        readonly DepartmentRepository _department;
        public DepartmentController(DepartmentRepository dep) : base(dep)
        {
            this._department = dep;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Department department)
        {
            var findId = await _department.GetId(id);
            findId.Name = department.Name;
            var data = await _department.Update(findId);
            if (data.Equals(null))
            {
                return BadRequest("Update Failed");
            }
            else
            {
                return data;
            }
        }
    }
}