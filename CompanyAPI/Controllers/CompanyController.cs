using CompanyAPI.Repository;
using CompanyAPI.Controllers.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Interfaces;
using CompanyAPI.Helper;


namespace CompanyAPI.Controllers
{
    [Route("api/Company")]
    public class CompanyController : Controller
    {
        Authentication _Auth = new Authentication();

        //CompanyRepo repo = new CompanyRepo();
        ICompanyRepo repo;
        public CompanyController(ICompanyRepo repo)
        {
            this.repo = repo;
        }

        //GET api/values
        [HttpGet]
        public IActionResult Get([FromHeader] string Authorization)
        {
            if (_Auth.Check(Authorization)==true)
            {
                try
                {
                    var result = repo.Read(0);
                    return Ok(result);
                }
                catch (RepoException ex)
                {
                    switch (ex.RepoExceptionType)
                    {
                        case RepoException.ExceptionType.NOCONTENT:
                        case RepoException.ExceptionType.NOTFOUND:
                            return StatusCode(StatusCodes.Status204NoContent);
                        case RepoException.ExceptionType.ERROR:
                            return StatusCode(StatusCodes.Status409Conflict);
                        case RepoException.ExceptionType.INVALIDARGUMENT:
                            return StatusCode(StatusCodes.Status400BadRequest);
                        case RepoException.ExceptionType.SQLERROR:
                            return StatusCode(StatusCodes.Status409Conflict);
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }
        //GET api/value
        [HttpGet("{Id}")]
        public IActionResult GetCompanyById(int id)
        {
            try
            {
                var result = repo.Read(id);
                return Ok(result);
            }
            catch (RepoException ex)
            {
                switch (ex.RepoExceptionType)
                {
                    case RepoException.ExceptionType.NOCONTENT:
                    case RepoException.ExceptionType.NOTFOUND:
                        return StatusCode(StatusCodes.Status204NoContent);
                    case RepoException.ExceptionType.INVALIDARGUMENT:
                        return StatusCode(StatusCodes.Status400BadRequest);
                    case RepoException.ExceptionType.ERROR:
                    case RepoException.ExceptionType.SQLERROR:
                        return StatusCode(StatusCodes.Status409Conflict);
                }
            }
         
            return Ok();
        }

        //POST api/values
        [HttpPost]

        public IActionResult Create([FromBody] CompanyAPI.Model.Company newCompany)
        {
             return StatusCode(StatusCodes.Status200OK, repo.Create(newCompany));
        }

        //PUT api/values

        [HttpPut]
        public IActionResult Put(int id, [FromBody] CompanyAPI.Model.Company updateCompany)
        {
            updateCompany.Id = id;
            var retval = repo.Update(updateCompany);

            return StatusCode(StatusCodes.Status200OK, retval);
        }

        //Delete api/values
        [HttpDelete]
        public IActionResult Delete([FromBody] Company DeleteCompany)
        {

            var retval = repo.Delete(DeleteCompany.Id);

            return StatusCode(StatusCodes.Status200OK, retval);
        }
    }
}
