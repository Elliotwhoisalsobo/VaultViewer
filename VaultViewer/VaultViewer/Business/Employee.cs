using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Employee
	{
		public int EmployeeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string PostalCode { get; set; }
		public string PostalCity { get; set; }
		public string Country { get; set; }
		public DateTime EmploymentDate { get; set; }
		public decimal CurrentMonthlySalary { get; set; }

		public Employee(int employeeID, string firstName, string lastName, DateTime dateOfBirth,
						string addressLine1, string addressLine2, string postalCode, string postalCity,
						string country, DateTime employmentDate, decimal currentMonthlySalary)
		{
			EmployeeID = employeeID;
			FirstName = firstName;
			LastName = lastName;
			DateOfBirth = dateOfBirth;
			AddressLine1 = addressLine1;
			AddressLine2 = addressLine2;
			PostalCode = postalCode;
			PostalCity = postalCity;
			Country = country;
			EmploymentDate = employmentDate;
			CurrentMonthlySalary = currentMonthlySalary;
		}
	}
}
