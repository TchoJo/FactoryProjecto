using System;
using FactoryProject.Data;
namespace FactoryProject.Models
{
	public class DepartmentsBL
	{
		private readonly DataContext _context;
		public DepartmentsBL(DataContext context)
		{
			_context = context;
		}


        public List<DepartmentsWname> getDepartments() {
			List<DepartmentsWname> departments = new List<DepartmentsWname>();

			foreach (Departments deparment in _context.Departments) 
			{
				
				var employees = _context.Employees.Where(employee => employee.id == deparment.manager).FirstOrDefault();
				DepartmentsWname newDepartment = new DepartmentsWname();
				newDepartment.id = deparment.id;
				newDepartment.departmentName = deparment.departmentName;
				if(deparment.manager != null) 
				{
					newDepartment.manager = deparment.manager;
					newDepartment.managerName = $"{employees.firstName} {employees.lastName}";
				}
				departments.Add(newDepartment);
			}
			return departments;
		}

		public DepartmentsWname getDepartment(int id) {
				DepartmentsWname departmentW = new DepartmentsWname();
				var department = _context.Departments.Where(department => department.id == id).First();
				departmentW.id = department.id;
				departmentW.departmentName = department.departmentName;
				if(departmentW.manager != null)
				{
					departmentW.manager = department.manager;
					var employee = _context.Employees.Where(employee => employee.id == id).First();
					departmentW.managerName = $"{employee.firstName} {employee.lastName}";
				}
				return departmentW;
		}

		public void AddDepartment(Departments newDepartment) 
		{
		    var dep = _context.Departments.Add(newDepartment);
			_context.SaveChanges();
			ChangeManagerDep((int)newDepartment.manager, dep.Entity.id);
		}


		public void ChangeManagerDep (int id, int depId)
		{
			var emp = _context.Employees.Where(emp => emp.id == id).First();
			if(emp != null)
			{
				emp.departmentID = depId;
				_context.Entry(emp).Property("departmentID").IsModified = true;
			}
			_context.SaveChanges();
		}


		public void EditDepartment(int id, Departments DepartmentEdit) 
		{
			var oldDepartment = _context.Departments.Where(department => department.id == id).First();
			oldDepartment.departmentName = DepartmentEdit.departmentName;
			if(oldDepartment.manager != null)
			{
				oldDepartment.manager = DepartmentEdit.manager;
				Employees managerUpdate = _context.Employees.Where(emp => emp.id == DepartmentEdit.manager).First();
				if(managerUpdate.departmentID != DepartmentEdit.id)
				{
					managerUpdate.departmentID = oldDepartment.id;
					_context.Entry(managerUpdate).Property("departmentID").IsModified = true;
				}
			}
			_context.SaveChanges();
			}
		
	
		public bool DeleteDepartment(int id) 
		{
			var DeleteDepartment = _context.Departments.Where(department => department.id == id).First();
			bool EmployeesLeft = _context.Employees.Where(emp => emp.departmentID == id).Any();
			if(EmployeesLeft == false)
			{
				_context.Departments.Remove(DeleteDepartment);
				_context.SaveChanges();
				return true;
			}
			else 
			{
				return false;
			}
		}
    }
}