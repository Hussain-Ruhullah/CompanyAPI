using CompanyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Interfaces
{
    public interface IDepartmentRepo
    {
        Department Create(Department model);
        List<Department> Read(int id);
        //List<Address> Read();
        Department Update(Department model);
        Department Delete(int id);
    }
}
