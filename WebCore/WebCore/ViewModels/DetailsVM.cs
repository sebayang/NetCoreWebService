using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ViewModels
{
    public class DetailsVM
    {
        public string Id { get; set; }
        public string Email { get; set; }  
        public string Phone { get; set; }
        public string RoleName { get; set; } 
        public string Username { get; set; }
        public string Address { get; set; } 
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; } 
    }
}
