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
        [HttpGet("Forgot")]
        public ActionResult ForgotPassword(AccountVM accountVM)
        {
            var result = accountRepository.ForgotPassword(accountVM);
            return Ok();
        }

    }
}
