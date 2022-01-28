using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }

        public int RegisterVM(RegisterVM registerVM)
        {
            var EmailExist = IsEmailExist(registerVM);
            var PhoneExist = IsPhoneExist(registerVM);
            if (EmailExist == false)
            {
                if (PhoneExist == false)
                {
                    var emp = new Employee()
                    {
                        NIK = NIK(),
                        FirstName = registerVM.FirstName,
                        LastName = registerVM.LastName,
                        Phone = registerVM.Phone,
                        Birthdate = registerVM.BirthDate,
                        Email = registerVM.Email,
                        Gender = (Models.Gender)registerVM.Gender,
                        Salary = registerVM.Salary,
                    };
                    context.Employees.Add(emp);

                    var ACC = new Account()
                    {
                        NIK = emp.NIK,
                        password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
                    };
                    context.Accounts.Add(ACC);

                    var AR = new AccountRole()
                    {
                        Account_Id = emp.NIK,
                        Role_Id = 1
                    };
                    context.AccountRoles.Add(AR);

                    var EDU = new Education()
                    {
                        Degree = registerVM.Degree,
                        GPA = registerVM.GPA,
                        University_Id = registerVM.University_Id
                    };
                    context.Educations.Add(EDU);
                    context.SaveChanges();
                    
                    var PROF = new Profiling()
                    {
                        NIK = emp.NIK,
                        Education_Id = EDU.Id
                    };
                    context.Profilings.Add(PROF);
                    return context.SaveChanges();
                }
                else
                {
                    return 5; //phone exist
                }
            }
            else if (EmailExist == true && PhoneExist == true)
            {
                return 6; //email & phone exist
            }
            else
            {
                return 4; //email exist
            }
        }



        public bool IsEmailExist(RegisterVM registerVM)
        {
            var cek = context.Employees.Where(s => s.Email == registerVM.Email).FirstOrDefault<Employee>();
            if (cek != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPhoneExist(RegisterVM registerVM)
        {
            var cek2 = context.Employees.Where(s => s.Phone == registerVM.Phone).FirstOrDefault<Employee>();
            if (cek2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string NIK()
        {
            var nikbaru = "";
            var year = DateTime.Now.ToString("yyyy");
            var i = context.Employees.ToList().Count();
            if (i != 0)
            {
                foreach (Employee e in Get())
                {
                    nikbaru = e.NIK;
                }
                nikbaru = Convert.ToString(int.Parse(nikbaru) + 1);
            }
            else
            {
                nikbaru = year + "001";
            }
            return nikbaru;
        }
        public IEnumerable GetRegisteredData()
        {
            var employees = context.Employees;
            var accounts = context.Accounts;
            var profilings = context.Profilings;
            var educations = context.Educations;
            var universities = context.Universities;
            var roles = context.Roles;
            var accountroles = context.AccountRoles;

            var result = (from emp in employees
                          join acc in accounts on emp.NIK equals acc.NIK
                          join prof in profilings on acc.NIK equals prof.NIK
                          join edu in educations on prof.Education_Id equals edu.Id
                          join univ in universities on edu.University_Id equals univ.Id
                          join ar in accountroles on acc.NIK equals ar.Account_Id
                          join role in roles on ar.Role_Id equals role.Id

                          select new
                          {
                              FullName = emp.FirstName + " " + emp.LastName,
                              Phone = emp.Phone,
                              BirthDate = emp.Birthdate,
                              Salary = emp.Salary,
                              Email = emp.Email,
                              Degree = edu.Degree,
                              GPA = edu.GPA,
                              UnivName = univ.Name,
                              Role = role.Name
                          }).ToList();


            return result;
        }
        
    }
}
