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
    class DepartmentRepo : IDepartmentRepo
    {
        private object ex;

        public List<Department> Read(int id = 0)
        {
            if (id == 0)
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");
                conn.Open();
                var result = conn.Query<Department>("SELECT * FROM viDepartment").ToList();
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
                    var result = conn.Query<Department>("SELECT Id,Country,City,ZipCode,Street FROM Department Where id = @Id", param).ToList();
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

        public Department Delete(int id)
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
                var result = conn.QueryFirstOrDefault<Department>(
                    "UPDATE Department SET DeletedTime = GetDate() WHERE Id = @Id SELECT Id FROM viDepartment WHERE Id = @Id", param);

                return result;
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }
        }

        internal object Delete(object deleteDepartment)
        {
            throw new NotImplementedException();
        }

        public Department Create(Department model)
        {
            return AddOrUpdate(model);
        }

        public Department Update(Department model)
        {
            return AddOrUpdate(model, model.Id);

        }

        private Department AddOrUpdate(Department pDepartment, int? @Id = null)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                DynamicParameters dParm = new DynamicParameters();
                dParm.Add("@Id", @Id);
                dParm.Add("@Country", pDepartment.CompanyId);
                dParm.Add("@City", pDepartment.DepartmentName);

                return conn.QueryFirstOrDefault<Department>("spInsertOrUpdateDepartment", dParm, null, null, CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }

        }

    }
}
