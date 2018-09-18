using CompanyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CompanyAPI.Repository
{
    public interface ICompanyRepo
    {
        Company Create(Company model);
        List<Company> Read(int id);
        //List<Company> Read();
        Company Update(Company model);
        Company Delete(int id);

    }
}