﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace CompanyAPI.Controllers
   {
    [Route("api/Address")]
    public class AddressController : Controller
    {
        Authentication _Auth = new Authentication();

        //AddressRepo repo = new AddressRepo();
        IAddressRepo repo;
        public AddressController(IAddressRepo repo)
        {
            this.repo = repo;
        }

        //GET api/values
        [Authorize(Roles ="1")]
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
        public IActionResult GetAddressById(int id)
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

        public IActionResult Create([FromBody] Model.Address newAddress)
        {
            return StatusCode(StatusCodes.Status200OK, repo.Create(newAddress));
        }

        //PUT api/values

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Model.Address updateAddress)
        {
            updateAddress.Id = id;
            var retval = repo.Update(updateAddress);

            return StatusCode(StatusCodes.Status200OK, retval);
        }

        //Delete api/values
        [HttpDelete]
        public IActionResult Delete([FromBody] Address DeleteAddress)
        {

            var retval = repo.Delete(DeleteAddress.Id);

            return StatusCode(StatusCodes.Status200OK, retval);
        }

    }
}

