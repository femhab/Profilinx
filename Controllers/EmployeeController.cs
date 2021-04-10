using System;
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
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _CompanyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService CompanyService, IMapper mapper)
        {
            _CompanyService = CompanyService;
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
                var status = await _CompanyService.Create(request);
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

                var pepsite = await _CompanyService.GetById(model.Id);

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
                var status = await _CompanyService.Update(request);

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
                var data = await _CompanyService.GetAll();
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
                var data = await _CompanyService.GetById(id);
                if (data == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = "Record Not Found"
                    });
                }

                var status = await _CompanyService.Delete(id);
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