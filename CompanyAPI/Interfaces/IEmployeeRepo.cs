using CompanyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Interfaces
{
    public interface IEmployeeRepo
    {

        Employee Create(Employee model);
        List<Employee> Read(int id);
        //List<Employee> Read();
        Employee Update(Employee model);
        Employee Delete(int id);
    }
}
