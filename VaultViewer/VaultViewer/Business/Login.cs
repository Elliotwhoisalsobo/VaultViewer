namespace VaultViewer.Business
{
	public class Login
	{
        public string UserName { get; set; }
		public string PasswordHash { get; set; }
		public int EmployeeID { get; set; }

		public Login(string userName, string passwordHash, int employeeID)
		{
			UserName = userName;
			PasswordHash = passwordHash;
			EmployeeID = employeeID;
		}
	}
}
