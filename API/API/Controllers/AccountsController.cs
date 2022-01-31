using API.Base;
using API.Context;
using API.Models;
using API.Repository;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BasesController<Account, AccountRepository, string>
    {
        public readonly AccountRepository accountRepository;
        public IConfiguration _configuration;
        public MyContext context;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration, MyContext context) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
            this.context = context;
        }
        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return Ok("Test JWT Berhasil");
        }
        [HttpGet("Login")]
        public ActionResult LoginVM(AccountVM accountVM)
        {
            var result = accountRepository.LoginVM(accountVM);
            if (result == 1)
            {
                var getUserData = context.Employees.Where(e => e.Email == accountVM.Email
                || e.Phone == accountVM.Phone).FirstOrDefault(); //get email & role name
                //var account = context.Accounts.Where(a => a.NIK == getUserData.NIK).FirstOrDefault();
                //var role = context.AccountRoles.Where(a => a.Account_Id == account.NIK).FirstOrDefault();
                var getRole = context.Roles.Where(a => a.AccountRole.Any(ar => ar.Account.NIK == getUserData.NIK)).ToList();

                //var getRoleName = accountRepository.GetAccountData(accountVM);

                var claims = new List<Claim>
                {
                    new Claim("Email", getUserData.Email),
                    //new Claim("roles", role.Role.Name) //payload
                };

                foreach (var item in getRole)
                {
                    claims.Add(new Claim("roles", item.Name));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //header
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn
                    );
                var idtoken = new JwtSecurityTokenHandler().WriteToken(token); //Generate Token
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return Ok(new { status = HttpStatusCode.OK, idtoken, message = "Login Success!"});
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
