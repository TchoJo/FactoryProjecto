using System;
using System.ComponentModel.DataAnnotations;

namespace FactoryProject.Models
{
	public class Departments
    { 
            [Key]
        public int id { get; set; }
        public string? deparmentName { get; set; }
        public int manager { get; set; }
    }
}


