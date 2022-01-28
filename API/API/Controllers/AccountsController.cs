using API.Base;
using API.Context;
using API.Models;
using API.Repository;
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
    public class AccountsController : BasesController<Account, AccountRepository, string>
    {
        public AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet("Login")]
        public ActionResult LoginVM(AccountVM accountVM)
        {
            var result = accountRepository.LoginVM(accountVM);
            if (result == 1)
            {
                return Ok(accountRepository.GetAccountData(accountVM));
            }
            else if (result == 2)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "LOGIN GAGAL! AKUN TIDAK DITEMUKAN" });
            }
            else if (result == 3)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "LOGIN GAGAL! PASSWORD SALAH" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "LOGIN GAGAL!" });
            }
        }
        [HttpPut("Forgot")]
        public ActionResult ForgotPassword(AccountVM accountVM)
        {
            var result = accountRepository.ForgotPassword(accountVM);
            if (result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "OTP TERKIRIM!" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "OTP GAGAL TERKIRIM! AKUN TIDAK DITEMUKAN" });
            }
        }
        [HttpPut("Change")]
        public ActionResult ChangePassword(AccountVM accountVM)
        {
            var result = accountRepository.ChangePassword(accountVM);
            if (result != 0)
            {
                if (result == 1)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "PASSWORD DIRUBAH!" });
                }
                else if (result == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "PASSWORD DAN CONFIRM PASSWORD TIDAK SAMA" });
                }
                else if (result == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP SUDAH DIGUNAKAN" });
                }
                else if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP EXPIRED" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "OTP SALAH!" });
                }
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.BadRequest, message = "EMAIL TIDAK DITEMUKAN!" });
            }
        }


    }
}
