using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCore.Contexts;
using WebCore.ViewModels;

namespace WebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly MyContext _context;
        public ChartController(MyContext mycon)
        {
            _context = mycon;
        } 
        [HttpGet] 
        [Route("pie")]
        public async Task<List<PieChartVM>> GetPie()
        { 
            var data = await _context.Divisions.Include("department")
                    .Where(x => x.IsDelete == false)
                    .GroupBy(a => a.department.Name)
                    .Select(b => new PieChartVM
                    {
                        DepartmentName = b.Key,
                        Total = b.Count()
                    }).ToListAsync();
            return data;
        }
    }
}