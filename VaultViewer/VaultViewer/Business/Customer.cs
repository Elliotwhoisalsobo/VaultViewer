using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{

	public class Customer
	{
		public int CustomerID { get; set; }
		public string Name { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string PostalCode { get; set; }
		public string PostalCity { get; set; }
		public string Country { get; set; }
		public string ContactPerson { get; set; }
		public string VATNumber { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsCompany { get; set; }
		public bool IsActive { get; set; }

		public Customer(int customerID, string name, string addressLine1, string addressLine2,
						string postalCode, string postalCity, string country, string contactPerson,
						string vatNumber, string emailAddress, string phoneNumber, bool isCompany, bool isActive)
		{
			CustomerID = customerID;
			Name = name;
			AddressLine1 = addressLine1;
			AddressLine2 = addressLine2;
			PostalCode = postalCode;
			PostalCity = postalCity;
			Country = country;
			ContactPerson = contactPerson;
			VATNumber = vatNumber;
			EmailAddress = emailAddress;
			PhoneNumber = phoneNumber;
			IsCompany = isCompany;
			IsActive = isActive;
		}
	}
}
