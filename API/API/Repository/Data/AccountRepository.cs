using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
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

        /*public string GetNumericOTP()
        {
            string numbers = "0123456789";
            Random random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int tempval = random.Next(0, numbers.Length);
                otp += tempval;
            }
            return otp;
        }*/


        //tambah kolom di variable
        //DateTime.Now + Add.Minutes (create function)
        //di tittle tambah datetime biar ga kedetect spam
        //change yg dikirim otp, email, new password
        //implementasi jwt
        //authentication -> login
        //authorizatiion -> access
        //add model role untuk autorisasi
        //table acc to role (M to M) ada table normalisasi 
        //acc to acc_role (1 to many) - acc_role to role (
        //employee, manager, direktur, 
        //tambahan di register, setiap emplloyee yang daftar, rolenya auto employee

        public int ForgotPassword(AccountVM accountVM)
        {
            var emailNow = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
            if (emailNow != null)
            {
                string numbers = "123456789";
                Random random = new Random();
                string otp = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    int tempval = random.Next(0, numbers.Length);
                    otp += tempval;
                }
                var accountNow = (from g in context.Accounts where g.NIK == emailNow.NIK select g).FirstOrDefault<Account>();
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587; //port for gmail
                bool enableSSL = true;
                string emailFromAddress = "medebelly@gmail.com"; //Sender Email Address  
                string password = "webtooniwannabeyou"; //Sender Password  
                string emailToAddress = accountVM.Email; //Receiver Email Address  
                string subject = "OTP " + DateTime.Now;
                string body = "Hello, This is your OTP " + otp;
                accountNow.OTP = Convert.ToInt32(otp);
                accountNow.ExpiredToken = DateTime.Now.AddMinutes(5);
                accountNow.isUsed = false; //belum dipake otp-nya
                context.Entry(accountNow).State = EntityState.Modified; //insert data di account
                context.SaveChanges();                

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailFromAddress);
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
                return 1;
            }
            else
            {
                return 0; //email not found
            }        

        }
        public int ChangePassword(AccountVM accountVM)
        {
            var emailNow = (from g in context.Employees where g.Email == accountVM.Email select g).FirstOrDefault<Employee>();
            if (emailNow!=null)
            {
                var accountNow = (from g in context.Accounts where g.NIK == emailNow.NIK select g).FirstOrDefault<Account>();
                if (accountNow != null)
                {
                    if (accountNow.OTP == accountVM.OTP)
                    {
                        if (DateTime.Now < accountNow.ExpiredToken)
                        {
                            if (accountNow.isUsed == false)
                            {
                                if (accountVM.Password == accountVM.ConfirmPassword)
                                {
                                    accountNow.password = BCrypt.Net.BCrypt.HashPassword(accountVM.Password);
                                    accountNow.isUsed = true;
                                    context.Entry(accountNow).State = EntityState.Modified;
                                    context.SaveChanges();
                                    return 1; //berhasil
                                }
                                else
                                {
                                    return 2; //Password & ConfirmPass ga sama
                                }
                            }
                            return 3; //OTP udah dipakai
                        }
                        return 4; //OTP expired
                        
                    }
                    return 5; //OTP salah
                }

            }
            return 0;
        }
            
    }
}
