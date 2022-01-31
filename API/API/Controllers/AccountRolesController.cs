using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
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
    public class AccountRolesController : BasesController<AccountRole, AccountRoleRepository, int>
    {
        public readonly AccountRoleRepository accountRoleRepository;
        public AccountRolesController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }

        [Authorize(Roles = "Director")]
        [HttpPost("SignManager")]
        public ActionResult<AccountRole> SignManager(AccountRole accountRole)
        {
            var result = accountRoleRepository.SignManager(accountRole);
            if (result == 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Role Telah Diupdate ke Manager" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Gagal Update Role" });
            }
        }
    }
}
