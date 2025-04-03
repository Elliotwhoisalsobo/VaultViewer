using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class InvoiceLine
	{
		public int InvoiceLineID { get; set; }
		public int InvoiceID { get; set; }
		public int ProductID { get; set; }
		public decimal Amount { get; set; }

		public InvoiceLine(int invoiceLineID, int invoiceID, int productID, decimal amount)
		{
			InvoiceLineID = invoiceLineID;
			InvoiceID = invoiceID;
			ProductID = productID;
			Amount = amount;
		}
	}
}
