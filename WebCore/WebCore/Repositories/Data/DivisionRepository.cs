using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Contexts;
using WebCore.Models;

namespace WebCore.Repositories.Data
{
    public class DivisionRepository : GeneralRepository<Division, MyContext>
    {
        MyContext _context;
        public DivisionRepository(MyContext context) : base(context) {
            _context = context;
        }

        public override async Task<List<Division>> GetAll()
        {
            var data = await _context.Divisions.Include("department").Where(D => D.IsDelete == false).ToListAsync();
            return data;
        }
    }
}
