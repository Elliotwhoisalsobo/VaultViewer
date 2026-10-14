namespace VaultViewer.Business
{
	public class Login
	{
        public string UserName { get; set; }
		public string Password { get; set; }
		public int EmployeeID { get; set; }

		public Login(string userName, string password, int employeeID)
		{
			UserName = userName;
			EmployeeID = employeeID;
			Password = password;
		}
	}
}
