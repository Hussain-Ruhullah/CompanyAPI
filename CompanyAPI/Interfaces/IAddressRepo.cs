using CompanyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPI.Interfaces
{
    public interface IAddressRepo
    {

        Address Create(Address model);
        List<Address> Read(int id);
        //List<Address> Read();
        Address Update(Address model);
        Address Delete(int id);
    }
}
