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
    private static string _connectionString = "Server=.;Database=TilbudsAvisData;Integrated Security=true;TrustServerCertificate=True";
    public static void BulkInsert(List<Product> products)
    {
        using (var connection = new SqlConnection(_connectionString))
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

    public static void BulkInsertAndSetIds(List<Product> products)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Begin a transaction
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Create a temporary table
                    string createTempTableQuery = @"
                    CREATE TABLE #TempProducts (
                        Name NVARCHAR(255),
                        ImageUrl NVARCHAR(500),
                        Description NVARCHAR(MAX),
                        ExternalId INT,
                        Amount FLOAT
                    );";

                    using (var createTableCommand = new SqlCommand(createTempTableQuery, connection, transaction))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    // Use SqlBulkCopy to insert into the temporary table
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "#TempProducts";

                        var table = new DataTable();
                        table.Columns.Add("Name", typeof(string));
                        table.Columns.Add("ImageUrl", typeof(string));
                        table.Columns.Add("Description", typeof(string));
                        table.Columns.Add("ExternalId", typeof(int));
                        table.Columns.Add("Amount", typeof(float));

                        foreach (var product in products)
                        {
                            table.Rows.Add(
                                product.Name ?? (object)DBNull.Value,
                                product.ImageUrl ?? (object)DBNull.Value,
                                product.Description ?? (object)DBNull.Value,
                                product.ExternalId,
                                product.Amount.HasValue ? (object)product.Amount.Value : DBNull.Value
                            );
                        }

                        bulkCopy.WriteToServer(table);
                    }

                    // Use OUTPUT clause to get generated IDs and set them on Product objects
                    string mergeQuery = @"
                    INSERT INTO dbo.Product (Name, ImageUrl, Description, ExternalId, Amount)
                    OUTPUT INSERTED.Id
                    SELECT Name, ImageUrl, Description, ExternalId, Amount FROM #TempProducts;";

                    using (var mergeCommand = new SqlCommand(mergeQuery, connection, transaction))
                    {
                        using (var reader = mergeCommand.ExecuteReader())
                        {
                            int index = 0;
                            while (reader.Read())
                            {
                                int generatedId = reader.GetInt32(0);
                                products[index].Id = generatedId;
                                index++;
                            }
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }


}
