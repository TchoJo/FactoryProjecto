using System;
using System.Linq;
using FactoryProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace FactoryProject.Models
{
	public class EmployeesBL
	{
		private readonly DataContext _context;
		public EmployeesBL(DataContext context)
		{
			_context = context;
		}


		public IEnumerable<Employees> GetEmployees()
		{
			return _context.Employees.ToList();
		}


		public Employees GetEmployee(int id)
		{
			return _context.Employees.Where(employee => employee.id == id).First();
		}


		public void AddEmployee(Employees newEmployee)
		{
			 _context.Employees.Add(newEmployee);
			_context.SaveChanges();
		}


		public void UpdateEmployee(int id, Employees EmployeeUpdate)
		{
			var oldEmployee = _context.Employees.Where(x => x.id == id).First();
			oldEmployee.firstName = EmployeeUpdate.firstName;
            oldEmployee.lastName = EmployeeUpdate.lastName;
			oldEmployee.startYear = EmployeeUpdate.startYear;
			oldEmployee.departmentID = EmployeeUpdate.departmentID;
			_context.SaveChanges(); 
        }

		public bool DeleteEmployee(int id)
		{
			var ByeEmployee = _context.Employees.Where(employee => employee.id == id).First();
			var CheckManager = _context.Departments.Where(dep => dep.manager == id).FirstOrDefault();
			var CheckShifts = _context.IDs.Where(ids => ids.employeeID == id);
			if(CheckManager != null && CheckManager.manager == ByeEmployee.id)
			{
				CheckManager.manager = null;
                _context.Entry(CheckManager).Property("manager").IsModified = true;
            }
			if( CheckShifts != null && CheckShifts.Any() )
			{
				foreach (var shift in CheckShifts)
				{
                    _context.Entry(shift).Entity.shiftID = 0;

                    _context.IDs.Remove(shift);

                }
            }
			_context.Remove(ByeEmployee);
            _context.SaveChanges();
			return true;
		}
	}
}
    

	// using(var transiction = _context.Database.BeginTransaction())
	// 	{
	// 		try {				_context.Employees.Remove(ByeEmployee);

	// 			IsDepartmentManager.manager = null;
	// 			_context.Entry(IsDepartmentManager).Property("manager").IsModified = true;
	// 		    _context.SaveChanges();
	// 			transiction.Commit();
	// 			return true;
				
	// 		}
	// 		catch (Exception)
	// 		{
	// 			transiction.Rollback();
	// 			return false;
	// 		}
	// 	}
	// 	}
	// 	else
	// 	{
	// 		_context.Remove(ByeEmployee);
	// 		_context.SaveChanges();
	// 		return true;
	// 	}



