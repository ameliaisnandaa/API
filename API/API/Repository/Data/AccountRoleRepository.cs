using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, int>
    {
        private MyContext context;
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }

        public int SignManager(AccountRole accountRole)
        {

            var cekNIK = context.AccountRoles.Where(e => e.Account_Id == accountRole.Account_Id).FirstOrDefault();
            if (cekNIK != null)
            {
                var AR = new AccountRole()
                {
                    Account_Id = accountRole.Account_Id,
                    Role_Id = 2
                };
                context.AccountRoles.Add(AR);
                
            }
            var result = context.SaveChanges();
            return result;
        }              
    }
}
