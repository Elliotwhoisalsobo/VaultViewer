using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Product
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public Product(int productID, string name, string description, decimal price)
		{
			ProductID = productID;
			Name = name;
			Description = description;
			Price = price;
		}
	}
}
