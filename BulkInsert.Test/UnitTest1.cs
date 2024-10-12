using BulkInsert.ClassLibrary.DAO;

namespace BulkInsert.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [Ignore("")]
    public void Test1()
    {
        var products = ProductGenerator.GenerateRandomProducts(10000);
        Assert.IsTrue(products.Count == 1000, "not a 1000 products");
    }

    [Test]
    public void BulkInsert()
    {
        var products = ProductGenerator.GenerateRandomProducts(1_000_000);
        var now = DateTime.Now;
        ProductDAO.BulkInsert(products);
        var elapsed = DateTime.Now - now;
        Console.WriteLine(  elapsed.Seconds);
        Assert.IsTrue(elapsed.TotalSeconds < 10);
    }
}