using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CompanyAPI.Controllers.Helper;
using CompanyAPI.Model;
using Dapper;
using CompanyAPI.Interfaces;




namespace CompanyAPI.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private object ex;

        public List<Employee> Read(int id = 0)
        {
            if (id == 0)
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");
                conn.Open();
                var result = conn.Query<Employee>("SELECT * FROM viEmployee").ToList();
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
                    var result = conn.Query<Employee>("SELECT Id,Country,City,ZipCode,Street FROM Employee Where id = @Id", param).ToList();
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

        //internal object EmployeeList()
        //{
        //    throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
        //}

        public Employee Delete(int id)
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
                var result = conn.QueryFirstOrDefault<Employee>(
                    "UPDATE Employee SET DeletedTime = GetDate() WHERE Id = @Id SELECT Id FROM viEmployee WHERE Id = @Id", param);

                return result;
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }
        }

        internal object Delete(object deleteEmployee)
        {
            throw new NotImplementedException();
        }

        public Employee Create(Employee model)
        {
            return AddOrUpdate(model);
        }

        public Employee Update(Employee model)
        {
            return AddOrUpdate(model, model.Id);

        }

        private Employee AddOrUpdate(Employee pEmployee, int? @Id = null)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                DynamicParameters dParm = new DynamicParameters();
                dParm.Add("@Id", @Id);
                dParm.Add("@Country", pEmployee.FirstName);
                dParm.Add("@City", pEmployee.LastName);
                dParm.Add("@ZipCode", pEmployee.Gender);
                dParm.Add("@Street", pEmployee.AddressId);

                return conn.QueryFirstOrDefault<Employee>("spInsertOrUpdateEmployee", dParm, null, null, CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }

        }

    }
}
