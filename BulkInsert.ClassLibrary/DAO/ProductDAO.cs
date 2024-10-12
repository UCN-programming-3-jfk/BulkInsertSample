using BulkInsert.ClassLibrary.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert.ClassLibrary.DAO;
public class ProductDAO
{
    public static void BulkInsert(List<Product> products)
    {
        using (var connection = new SqlConnection("Server=.;Database=TilbudsAvisData;Integrated Security=true;TrustServerCertificate=True"))
        {
            connection.Open();
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "dbo.Product";

                // Create a DataTable with the updated columns from the Product class
                var table = new DataTable();
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("ImageUrl", typeof(string));
                table.Columns.Add("Description", typeof(string));
                table.Columns.Add("ExternalId", typeof(int));
                table.Columns.Add("Amount", typeof(float));

                // Populate the DataTable with the product data
                foreach (var product in products)
                {
                    table.Rows.Add(
                        product.Id.HasValue ? (object)product.Id.Value : DBNull.Value,
                        product.Name ?? (object)DBNull.Value,
                        product.ImageUrl ?? (object)DBNull.Value,
                        product.Description ?? (object)DBNull.Value,
                        product.ExternalId,
                        product.Amount.HasValue ? (object)product.Amount.Value : DBNull.Value
                    );
                }

                // Perform bulk insert
                bulkCopy.WriteToServer(table);
            }
        }
    }
}
