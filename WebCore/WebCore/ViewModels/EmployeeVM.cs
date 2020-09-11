using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ViewModels
{
    public class EmployeeVM
    {
        public string EmpId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; } 
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
        public DateTimeOffset DeleteTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
