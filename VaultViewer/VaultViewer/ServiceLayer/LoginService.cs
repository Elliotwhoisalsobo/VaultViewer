using VaultViewer.Business;
using VaultViewer.DataAccessLayer;

namespace VaultViewer.ServiceLayer
{
    public class LoginService
    {
        private readonly EmployeeRepository employeeRepository = new();
        private readonly LoginRepository loginRepository = new();
        private readonly RoleRepository roleRepository = new();

        public enum CreateUserResult
        {
            Success,
            MissingUsername,
            MissingPassword,
            EmptyUsernameAndPassword,
            UsernameTooShort,
            PasswordTooShort,
            EmployeeNotFound,
            UserAlreadyExists
        }

        public bool TestConnection(out string? error)
        {
            try
            {
                loginRepository.TestConnection();
                error = null;
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public bool Authenticate(string username, string password, out List<string> roles)
        {
            roles = new List<string>();
            var login = loginRepository.GetByUsername(username);
            if (login == null || !BCrypt.Net.BCrypt.Verify(password, login.PasswordHash))
            {
                return false;
            }

            roles = roleRepository.GetForEmployee(login.EmployeeID).Select(role => role.Name).ToList();
            return true;
        }

        public CreateUserResult CreateUser(string username, string password)
        {
            var validationResult = ValidateUserInput(username, password);
            if (validationResult != CreateUserResult.Success)
            {
                return validationResult;
            }

            var employee = employeeRepository.GetByFirstName(username);
            if (employee == null)
            {
                return CreateUserResult.EmployeeNotFound;
            }

            var login = new Login(username, BCrypt.Net.BCrypt.HashPassword(password), employee.EmployeeID);
            return loginRepository.TryAdd(login)
                ? CreateUserResult.Success
                : CreateUserResult.UserAlreadyExists;
        }

        private static CreateUserResult ValidateUserInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            {
                return CreateUserResult.EmptyUsernameAndPassword;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                return CreateUserResult.MissingUsername;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return CreateUserResult.MissingPassword;
            }

            if (username.Length < 3)
            {
                return CreateUserResult.UsernameTooShort;
            }

            return password.Length < 6
                ? CreateUserResult.PasswordTooShort
                : CreateUserResult.Success;
        }
    }
}
