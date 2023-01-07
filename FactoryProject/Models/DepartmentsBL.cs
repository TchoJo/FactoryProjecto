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
				
				var employees = _context.Employees.Where(employee => employee.id == deparment.manager).First();
				DepartmentsWname newDepartment = new DepartmentsWname();
				newDepartment.id = deparment.id;
				newDepartment.departmentName = deparment.departmentName;
				newDepartment.manager = deparment.manager;
				newDepartment.managerName = $"{employees.firstName} {employees.lastName}";
				departments.Add(newDepartment);
			}
			return departments;
		}

		public DepartmentsWname getDepartment(int id) {
			DepartmentsWname departmentW = new DepartmentsWname();
			var department = _context.Departments.Where(department => department.id == id).First();
			departmentW.id = department.id;
			departmentW.departmentName = department.departmentName;
			departmentW.manager = department.manager;
			var employee = _context.Employees.Where(employee => employee.id == id).First();
			departmentW.managerName = $"{employee.firstName} {employee.lastName}";

			return departmentW;
		}


    }
}