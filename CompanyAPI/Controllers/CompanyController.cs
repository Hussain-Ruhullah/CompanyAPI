using CompanyAPI.Repository;
using CompanyAPI.Controllers.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyAPI.Model;
using CompanyAPI.Interfaces;
using CompanyAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TobitLogger.Core;
using TobitLogger.Core.Models;
using TobitWebApiExtensions.Extensions;


namespace CompanyAPI.Controllers
{
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        Authentication _Auth = new Authentication();
        private readonly ILogger<CompanyController> _logger;
        ICompanyRepo repo;
        public CompanyController(ICompanyRepo repo, ILoggerFactory loggerFactory)
        {
            this.repo = repo;
            _logger = loggerFactory.CreateLogger<CompanyController>();
        }

        //GET api/values
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult Get([FromHeader] string Authorization)
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

        public IActionResult Create([FromBody] Company newCompany)
        {
             return StatusCode(StatusCodes.Status200OK, repo.Create(newCompany));
        }

        //PUT api/values

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Company updateCompany)
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
