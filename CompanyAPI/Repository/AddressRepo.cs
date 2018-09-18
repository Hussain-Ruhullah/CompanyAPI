using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CompanyAPI.Model;
using System.Data;
using CompanyAPI.Controllers.Helper;
using CompanyAPI.Interfaces;

namespace CompanyAPI.Repository
{
    class AddressRepo : IAddressRepo
    {
        private object ex;

        public List<Address> Read(int id = 0)
        {
            if (id == 0)
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");
                conn.Open();
                var result = conn.Query<Address>("SELECT * FROM viAddress").ToList();
                if (result.Any() == false)
                {
                    throw new RepoException(RepoException.ExceptionType.NOCONTENT);
                }
                return result;
            }
            else
            {
                if (id < 0)
                {
                    throw new RepoException(RepoException.ExceptionType.INVALIDARGUMENT);
                }

                try
                {
                    SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                    conn.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    var result = conn.Query<Address>("SELECT Id,Country,City,ZipCode,Street FROM Address Where id = @Id", param).ToList();
                    if (result == null)
                    {
                        throw new RepoException(RepoException.ExceptionType.NOCONTENT);
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
                }
            }
        }

        //internal object AddressList()
        //{
        //    throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
        //}

        public Address Delete(int id)
        {
            if (id < 0)
            {
                throw new RepoException(RepoException.ExceptionType.INVALIDARGUMENT);
            }

            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                conn.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                var result = conn.QueryFirstOrDefault<Address>(
                    "UPDATE Address SET DeletedTime = GetDate() WHERE Id = @Id SELECT Id FROM Address WHERE Id = @Id", param);

                return result;
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }
        }

        internal object Delete(object deleteAddress)
        {
            throw new NotImplementedException();
        }

        public Address Create(Address model)
        {
            return AddOrUpdate(model);
        }

        public Address Update(Address model)
        {
            return AddOrUpdate(model, model.Id);

        }

        private Address AddOrUpdate(Address pAddress, int? @Id = null)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                DynamicParameters dParm = new DynamicParameters();
                dParm.Add("@Id", @Id);
                dParm.Add("@Country", pAddress.Country);
                dParm.Add("@City", pAddress.City);
                dParm.Add("@ZipCode", pAddress.ZipCode);
                dParm.Add("@Street", pAddress.Street);
                dParm.Add("@HouseNumber", pAddress.Street);

                return conn.QueryFirstOrDefault<Address>("spInsertOrUpdateAddress", dParm, null, null, CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }

        }

    }
}
