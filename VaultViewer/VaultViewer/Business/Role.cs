using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultViewer.Business
{
	public class Role
	{
		public int RoleID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DepartmentName { get; set; }

		public Role(int roleID, string name, string description, string departmentName)
		{
			RoleID = roleID;
			Name = name;
			Description = description;
			DepartmentName = departmentName;
		}
	}
}
