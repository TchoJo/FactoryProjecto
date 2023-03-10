using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FactoryProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesBL _employeesBL;

        public EmployeesController(EmployeesBL employeesBL)
        {
            _employeesBL = employeesBL;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employees> GetEmployees()
        {
            return _employeesBL.GetEmployees();
        }

        // GET: api/Employees/5
        [HttpGet("{id}", Name = "GetEmployee")]
        public Employees Get(int id)
        {
            return _employeesBL.GetEmployee(id);
        }

        // POST: api/Employees
        [HttpPost]
        public string Post(Employees newEmployee)
        {
            _employeesBL.AddEmployee(newEmployee);
            return $"Employe ID {newEmployee.id} Added.";
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public string Put(int id, Employees EmployeeUpdate)
        {
            _employeesBL.UpdateEmployee(id, EmployeeUpdate);
            return $"{EmployeeUpdate.firstName} {EmployeeUpdate.lastName}, ID {id} Has Been Updated.";
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public string DeleteEmployee(int id)
        {
         var DeletedEmp = _employeesBL.DeleteEmployee(id);
         if(DeletedEmp == true)
         {
                     return $"Employee ID {id} Deleted.";
         }
         else 
         {
            return $"Could not delete Employee ID {id}";
         }
        }
    }
}
