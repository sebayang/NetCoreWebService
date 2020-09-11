using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Base
{
    public interface BaseModel
    {
        int Id { get; set; }
        string Name { get; set; }
        DateTimeOffset CreateTime { get; set; }
        DateTimeOffset UpdateTime { get; set; }
        DateTimeOffset DeleteTime { get; set; }
        bool IsDelete { get; set; }
    }
}
