using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class OldEmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;
        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }
        public int Employees { get; private set; }

        public int Delete(Employee employee)
        {
            var emp = Get(employee.NIK);
            if (emp != null)
            {
                context.Remove(emp);
            }  
            
            var result = context.SaveChanges();
            return result;
        }
        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();

        }
        public Employee Get(string NIK)
        {
            var get = context.Employees.Find(NIK);
            return get;
        }
        
        public bool IsEmailExist(Employee employee)
        {
            var cek = context.Employees.Where(s => s.Email == employee.Email).FirstOrDefault<Employee>();
            if (cek != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPhoneExist(Employee employee)
        {
            var cek2 = context.Employees.Where(s => s.Phone == employee.Phone).FirstOrDefault<Employee>();
            if (cek2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Insert(Employee employee)
        {
            var idbaru = "";
            var year = DateTime.Now.ToString("yyyy");
            var i = context.Employees.ToList().Count();
            if (i != 0)
            {
                foreach (Employee e in Get())
                {
                    idbaru = e.NIK;
                }
                idbaru = Convert.ToString(int.Parse(idbaru)+1);
            }
            else
            {
                idbaru = year + "001";
            }
            var EmailExist = IsEmailExist(employee);
            var PhoneExist = IsPhoneExist(employee);
            if (EmailExist == false)
            {
                if (PhoneExist == false)
                {
                    employee.NIK = idbaru;
                    context.Employees.Add(employee);
                    var result = context.SaveChanges();
                    return result;
                }
                else
                {
                    return 3; //phone exist
                }
            }
            else if (EmailExist==true && PhoneExist==true)
            {
                return 4; //email & phone exist
            }
            else
            {
                return 2; //email exist
            }

            /*var cek = context.Employees.Where(s => s.Email == employee.Email).FirstOrDefault<Employee>();
            var cek2 = context.Employees.Where(s => s.Phone == employee.Phone).FirstOrDefault<Employee>();
            if (cek == null && cek2 == null)
            {
                employee.NIK = idbaru;
                context.Employees.Add(employee);
            }
            var result = context.SaveChanges();            
            return result;*/
        }
        public int Update(Employee employee)
        {            
            var emp = Get(employee.NIK);
            if (emp != null)
            {
                var EmailExist = IsEmailExist(employee);
                var PhoneExist = IsPhoneExist(employee);
                if (EmailExist == false)
                {
                    if (PhoneExist == false)
                    {
                        if (employee.FirstName != null) { emp.FirstName = employee.FirstName; }
                        if (employee.LastName != null) { emp.LastName = employee.LastName; }
                        if (employee.Phone != null) { emp.Phone = employee.Phone; }
                        if (employee.Birthdate.ToString("yyyy") != "0001") { emp.Birthdate = employee.Birthdate; }
                        if (employee.Salary != 0) { emp.Salary = employee.Salary; }
                        if (employee.Email != null) { emp.Email = employee.Email; }
                        context.Entry(emp).State = EntityState.Modified;
                        /*var result = context.SaveChanges();
                        return result;*/
                    }
                    else
                    {
                        return 3; //phone exist
                    }
                }
                else if (EmailExist == true && PhoneExist == true)
                {
                    return 4; //email & phone exist
                }
                else
                {
                    return 2; //email exist
                }
                /*var cek = context.Employees.Where(s => s.Email == employee.Email).FirstOrDefault<Employee>();
                var cek2 = context.Employees.Where(s => s.Phone == employee.Phone).FirstOrDefault<Employee>();
                if (cek == null && cek2 == null)
                {
                    if (employee.FirstName != null) { emp.FirstName = employee.FirstName; }
                    if (employee.LastName != null) { emp.LastName = employee.LastName; }
                    if (employee.Phone != null) { emp.Phone = employee.Phone; }
                    if (employee.Birthdate.ToString("yyyy") != "0001") { emp.Birthdate = employee.Birthdate; } 
                    if (employee.Salary != 0) { emp.Salary = employee.Salary; }
                    if (employee.Email != null) { emp.Email = employee.Email; }
                    context.Entry(emp).State = EntityState.Modified;
                } */               
            }
            var result = context.SaveChanges();
            return result;
        }
    }

}
