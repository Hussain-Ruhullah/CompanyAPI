using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Model
{
    public class Department
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public string DepartmentName { get; set; }
    }
}
