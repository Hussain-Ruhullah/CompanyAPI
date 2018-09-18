using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CompanyAPI.Model;
using System.Data;
using CompanyAPI.Controllers.Helper;
using CompanyAPI.Repository;

namespace CompanyAPI.Repository
{
    class CompanyRepo : ICompanyRepo
    {
        private object ex;

        public List<Company> Read(int id = 0)
        {
            if (id == 0)
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");
                conn.Open();
                var result = conn.Query<Company>("SELECT * FROM viCompany").ToList();
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
                    var result = conn.Query<Company>("SELECT Name FROM Company Where id = @Id", param).ToList();
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


        public Company Delete(int id)
        {
            if(id < 0)
            {
                throw new RepoException(RepoException.ExceptionType.INVALIDARGUMENT);
            }

            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");
            
                conn.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                var result = conn.QueryFirstOrDefault<Company>(
                    "UPDATE Company SET DeletedTime = GetDate() WHERE Id = @Id SELECT Id FROM Company WHERE Id = @Id",param);
              
                return result;
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }
        }


        public Company Create(Company model)
        {
            return AddOrUpdate(model);
        }

        public Company Update(Company model)
        {
            return AddOrUpdate(model,model.Id);

        }

        private Company AddOrUpdate(Company pCompany, int? @Id = null)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=tappqa;Initial Catalog=Traning-HR-Company;Integrated Security=True");

                DynamicParameters dParm = new DynamicParameters();
                dParm.Add("@Id", @Id);
                dParm.Add("@Name", pCompany.Name);


                return conn.QueryFirstOrDefault<CompanyAPI.Model.Company>("spInsertOrUpdateCompany", dParm, null, null, CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new RepoException(ex.ToString(), RepoException.ExceptionType.SQLERROR);
            }

        }

    }   
}
