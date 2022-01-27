using API.Context;
using API.Models;
using API.ViewModel;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        public int LoginVM(AccountVM accountVM)
        {
            var phone = (from g in context.Employees where g.Phone == accountVM.Phone select g).FirstOrDefault<Employee>();
            var email = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
            if (phone != null || email != null)
            {
                var employees = context.Employees;
                var accounts = context.Accounts;
                var result = (from emp in employees
                              join acc in accounts on emp.NIK equals acc.NIK
                              where emp.Phone == accountVM.Phone || emp.Email == accountVM.Email
                              select acc).FirstOrDefault();
                if (BCrypt.Net.BCrypt.Verify(accountVM.Password, result.password))
                {
                    return 1; //login
                }
                else
                {
                    return 3; //wrong pass
                }
        }
            else
            {
                return 2; //no acc
            }
        }
        public IEnumerable GetAccountData(AccountVM accountVM)
        {
            var employees = context.Employees;
            var accounts = context.Accounts;
            var profilings = context.Profilings;
            var educations = context.Educations;
            var universities = context.Universities;
           
            var result = (from emp in employees
                          join acc in accounts on emp.NIK equals acc.NIK
                          join prof in profilings on acc.NIK equals prof.NIK
                          join edu in educations on prof.Education_Id equals edu.Id
                          join univ in universities on edu.University_Id equals univ.Id
                          where emp.Phone == accountVM.Phone || emp.Email == accountVM.Email

                          select new
                          {
                              FullName = emp.FirstName + " " + emp.LastName,
                              Phone = emp.Phone,
                              BirthDate = emp.Birthdate,
                              Salary = emp.Salary,
                              Email = emp.Email,
                              Degree = edu.Degree,
                              GPA = edu.GPA,
                              UnivName = univ.Name
                          }).ToList();


            return result;
        }

        public string GetNumericOTP(AccountVM accountVM)
        {
            string numbers = "0123456789";
            Random random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int tempval = random.Next(0, numbers.Length);
                otp += tempval;
            }
            return otp;
        }


        public int ForgotPassword(AccountVM accountVM)
        {
            var emailNow = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
            if (emailNow != null)
            {
                
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string emailFromAddress = "ameliaisnanda99@gmail.com"; //Sender Email Address  
                string password = "31agustus1999"; //Sender Password  
                string emailToAddress = accountVM.Email; //Receiver Email Address  
                string subject = "Hello";
                string body = "Hello, This is Email sending test using gmail.";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ameliaisnanda99@gmail.com");
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            return 1;

        }
    }
}
