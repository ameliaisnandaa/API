using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BasesController<Employee, EmployeeRepository, string>
    {
        public EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost("{RegisterVM}")]
        public ActionResult RegisterVM(RegisterVM registerVM)
        {
            var result = employeeRepository.RegisterVM(registerVM);
            if (result != 0)
            {
                if (result == 1)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "REGISTER BERHASIL!" });
                }
                else if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! Email Sudah Ada" });
                }
                else if (result == 5)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! Phone Sudah Ada" });
                }
                else if (result == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! Email dan Phone Sudah Ada" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "REGISTER GAGAL! kalo nol" });
            }
        }
        [Route("GetRegisteredData")]
        [HttpGet]
        public ActionResult<RegisterVM> GetRegisteredData()
        {
            var result = employeeRepository.GetRegisteredData();
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "DATA BERHASIL DITEMUKAN" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, result, message = "DATA TIDAK DITEMUKAN" });
            }
        }
    }
}
