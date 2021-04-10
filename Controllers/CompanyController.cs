﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROFILINX.Business.Interphases;
using PROFILINX.Data.Entity;
using PROFILINX.Viewmodel.Models;

namespace PROFILINX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService EmployeeService, IMapper mapper)
        {
            _EmployeeService = EmployeeService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Company model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationError();
                }

                //let data layer be interpreted to view model layer
                var request = _mapper.Map<Company>(model);
                var status = await _EmployeeService.Create(request);
                return Ok(new
                {
                    status = status,
                    message = status ? "Created Successfully" : "Record creation Failed"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromBody] Company model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationError();
                }

                var pepsite = await _EmployeeService.GetById(model.Id);

                if (pepsite == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Record Not Found"
                    });
                }
                ////
                var request = _mapper.Map<Company>(model);
                var status = await _EmployeeService.Update(request);

                return Ok(new
                {
                    status = status,
                    message = status ? "Created Successfully" : "Record creation Failed"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _EmployeeService.GetAll();
                if (data != null)
                {
                    var model = _mapper.Map<IEnumerable<Company>>(data);
                    return Ok(model);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var data = await _EmployeeService.GetById(id);
                if (data == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Record Not Found"
                    });
                }

                var status = await _EmployeeService.Delete(id);
                return Ok(new
                {
                    status,
                    message = (status) ? "Deleted Successfully!" : "Delete Failed!"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}