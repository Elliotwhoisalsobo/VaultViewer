using MySql.Data.MySqlClient;
using VaultViewer.Business;

namespace VaultViewer.DataAccessLayer
{
    public class CustomerRepository
    {
        public List<Customer> GetAll(int? limit = null)
        {
            var customers = new List<Customer>();
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();

            var sql = "SELECT CustomerID, Name, AddressLine1, AddressLine2, PostalCode, PostalCity, Country, ContactPerson, VATNumber, EmailAddress, PhoneNumber, IsCompany, IsActive FROM customer";
            if (limit.HasValue)
            {
                sql += " LIMIT @limit";
            }

            using var command = new MySqlCommand(sql, connection);
            if (limit.HasValue)
            {
                command.Parameters.AddWithValue("@limit", limit.Value);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer(
                    reader.GetInt32("CustomerID"),
                    reader.GetString("Name"),
                    reader.GetString("AddressLine1"),
                    reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString("AddressLine2"),
                    reader.GetString("PostalCode"),
                    reader.GetString("PostalCity"),
                    reader.GetString("Country"),
                    reader.GetString("ContactPerson"),
                    reader.IsDBNull(reader.GetOrdinal("VATNumber")) ? null : reader.GetString("VATNumber"),
                    reader.GetString("EmailAddress"),
                    reader.GetString("PhoneNumber"),
                    reader.GetBoolean("IsCompany"),
                    reader.GetBoolean("IsActive")));
            }

            return customers;
        }
    }
}
