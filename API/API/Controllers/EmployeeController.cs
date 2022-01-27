using API.Context;
using API.Models;
using API.Repository;
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
    public class EmployeeController : ControllerBase
    {
        private OldEmployeeRepository employeeRepository;
        public EmployeeController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public ActionResult Post(Employee employees)
        {
            var hasil = employeeRepository.Insert(employees);
            if (hasil != 0)
            {
                if (hasil == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "INSERT GAGAL! Email Sudah Ada" });
                }
                else if (hasil == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "INSERT GAGAL! Phone Sudah Ada" });
                }
                else if (hasil == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "INSERT GAGAL! Email dan Phone Sudah Ada" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "INSERT BERHASIL" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "INSERT GAGAL" });
            }
        }

        [HttpPut]
        public ActionResult Put(Employee employees)
        {
            var hasil = employeeRepository.Update(employees);
            if (hasil == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "UPDATE GAGAL" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "UPDATE BERHASIL" });
            }
        }

        [HttpDelete]
        public ActionResult Delete(Employee employees)
        {
            var hasil = employeeRepository.Delete(employees);
            if (hasil == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "UPDATE GAGAL" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "DELETE BERHASIL" });
            }
        }

        [HttpGet("{NIK}")]
        public ActionResult Get (Employee employees)
        {
            var hasil = employeeRepository.Get(employees.NIK);

            if (hasil == null)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "DATA TIDAK DITEMUKAN" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, hasil, message = "DATA DITEMUKAN" });
            }           
        }
        [HttpGet]
        public ActionResult Get()
        {
            var a = employeeRepository.Get();
            var b = a.Count();
            if (b == 0)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "DATA TIDAK DITEMUKAN" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "DATA DITEMUKAN" });
            }

            
        }
    }
   


}
