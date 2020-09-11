using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Base;

namespace WebCore.Models
{
    [Table("tb_m_department")]
    public class Department : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
        public DateTimeOffset DeleteTime { get; set; }
        public bool IsDelete { get; set; }

    //    public Department() { }
    //    public Department(Department department)
    //    { 
    //        this.Name = department.Name;
    //        this.CreateTime = DateTimeOffset.Now;
    //        this.IsDelete = false;
    //    }

    //    public void Update(Department department) {
    //        this.Name = department.Name;
    //        this.UpdateTime = DateTimeOffset.Now;
    //    }
    //    public void Delete(int id) {
    //        this.IsDelete = true;
    //        this.UpdateTime = DateTimeOffset.Now;
    //    } 
    }
}
