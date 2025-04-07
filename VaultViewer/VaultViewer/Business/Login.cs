using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Login
	{
        public string UserName { get; set; }
		public string Password { get; set; } // Hashtable/map???
		public int EmployeeID { get; set; }

		public Login(string userName, string password, int employeeID)
		{
			UserName = userName;
			EmployeeID = employeeID;
			Password = password;
		}
	}
}
