using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Login
	{
		public string UserName { get; set; }
		public int EmployeeID { get; set; }

		public Login(string userName, int employeeID)
		{
			UserName = userName;
			EmployeeID = employeeID;
		}
	}
}
