using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Invoice
	{
		public int InvoiceID { get; set; }
		public int EmployeeID { get; set; }
		public int CustomerID { get; set; }
		public string OurReference { get; set; }
		public string YourReference { get; set; }
		public DateTime InvoiceDate { get; set; }
		public DateTime PaymentDueDate { get; set; }

		public Invoice(int invoiceID, int employeeID, int customerID, string ourReference,
					   string yourReference, DateTime invoiceDate, DateTime paymentDueDate)
		{
			InvoiceID = invoiceID;
			EmployeeID = employeeID;
			CustomerID = customerID;
			OurReference = ourReference;
			YourReference = yourReference;
			InvoiceDate = invoiceDate;
			PaymentDueDate = paymentDueDate;
		}
	}

}
