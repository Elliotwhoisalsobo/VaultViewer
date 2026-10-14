using MySql.Data.MySqlClient;
using VaultViewer.Business;

namespace VaultViewer.DataAccessLayer
{
    public class ProductRepository
    {
        public List<Product> GetAll(int? limit = null)
        {
            var products = new List<Product>();
            using var connection = DatabaseConfig.GetConnection();
            connection.Open();

            var sql = "SELECT ProductID, Name, Description, Price FROM product";
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
                products.Add(new Product(
                    reader.GetInt32("ProductID"),
                    reader.GetString("Name"),
                    reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                    reader.GetDecimal("Price")));
            }

            return products;
        }
    }
}
