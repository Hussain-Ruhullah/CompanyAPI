using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Controllers.Helper;
using CompanyAPI.Model;
using CompanyAPI.Interfaces;
using CompanyAPI.Repository;
using CompanyAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace CompanyAPI.Controllers
{
    public class DepartmentController : Controller
    {
        Authentication _Auth = new Authentication();

        //DepartmentRepo repo = new DepartmentRepo();
        IDepartmentRepo repo;
        public DepartmentController(IDepartmentRepo repo)
        {
            this.repo = repo;
        }

        //GET api/values
        [HttpGet]
        public IActionResult Get([FromHeader] string Authorization)
        {
            //check or checkAccess
            if (_Auth.CheckToken(Authorization) == true)
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
        public IActionResult GetDepartmentById(int id)
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

        public IActionResult Create([FromBody] Model.Department newDepartment)
        {
            return StatusCode(StatusCodes.Status200OK, repo.Create(newDepartment));
        }

        //PUT api/values

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Model.Department updateDepartment)
        {
            updateDepartment.Id = id;
            var retval = repo.Update(updateDepartment);

            return StatusCode(StatusCodes.Status200OK, retval);
        }

        //Delete api/values
        [HttpDelete]
        public IActionResult Delete([FromBody] Department DeleteDepartment)
        {

            var retval = repo.Delete(DeleteDepartment.Id);

            return StatusCode(StatusCodes.Status200OK, retval);
        }

    }
}

