using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore.Base;
using WebCore.Contexts;
using WebCore.Models;
using WebCore.Repositories.Data;

namespace WebCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : BaseController<Division, DivisionRepository>
    {
        readonly DivisionRepository _division;
        readonly MyContext _context;
        public DivisionController(DivisionRepository divisionRepository) : base(divisionRepository)
        {
            this._division = divisionRepository;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Division division)
        {
            var findId = await _division.GetId(id);
            findId.Name = division.Name;
            findId.DepartmentId = division.DepartmentId;
            var data = await _division.Update(findId);
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