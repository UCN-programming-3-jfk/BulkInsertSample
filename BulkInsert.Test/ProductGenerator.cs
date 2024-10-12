using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert.Test;

using BulkInsert.ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class ProductGenerator
{
    private static Random _random = new Random();
    private static List<string> _sampleNames = new List<string>
{
    "Apple", "Banana", "Orange", "Laptop", "Smartphone", "Tablet", "Headphones", "Keyboard", "Monitor",
    "Mango", "External Hard Drive", "USB Cable", "HDMI Cable", "Grapes", "Router", "Peach", "Cherry",
    "Strawberry", "Washing Machine", "Pineapple", "Blender", "Microwave", "Desk Chair", "Electric Kettle",
    "Blueberry"
};

    private static List<string> _sampleImageUrls = new List<string>
{
    "https://hardwarehub.com/images/laptop.jpg",
    "https://grocerystore.com/images/banana.jpg",
    "https://techworld.com/images/smartphone.jpg",
    "https://furnitureplus.com/images/deskchair.jpg",
    "https://electrostore.com/images/monitor.jpg",
    "https://grocerystore.com/images/orange.jpg",
    "https://appliancemart.com/images/washingmachine.jpg",
    "https://techgear.com/images/keyboard.jpg",
    "https://hardwarehub.com/images/router.jpg",
    "https://grocerystore.com/images/mango.jpg",
    "https://electrostore.com/images/headphones.jpg",
    "https://appliancemart.com/images/blender.jpg",
    "https://furnitureplus.com/images/electrickettle.jpg",
    "https://grocerystore.com/images/grapes.jpg",
    "https://hardwarehub.com/images/externalharddrive.jpg",
    "https://grocerystore.com/images/peach.jpg",
    "https://appliancemart.com/images/microwave.jpg",
    "https://techgear.com/images/usb-cable.jpg",
    "https://hardwarehub.com/images/hdmi-cable.jpg",
    "https://grocerystore.com/images/pineapple.jpg",
    "https://techworld.com/images/tablet.jpg",
    "https://grocerystore.com/images/cherry.jpg",
    "https://appliancemart.com/images/microwave.jpg",
    "https://grocerystore.com/images/strawberry.jpg",
    "https://techworld.com/images/blueberry.jpg"
};


    public static List<Product> GenerateRandomProducts(int numberOfProducts)
    {
        var products = new List<Product>();

        for (int i = 0; i < numberOfProducts; i++)
        {
            string name = _sampleNames[_random.Next(_sampleNames.Count)];
            string imageUrl = _sampleImageUrls[_random.Next(_sampleImageUrls.Count)];
            var product = new Product()
            { 
               Name = name,                        // Random name
               ImageUrl = imageUrl,                    // Random image URL
               Description = "This is a product description.", // Default description
               ExternalId = _random.Next(1000, 9999),    // Random ExternalId
                Amount = _random.Next(1, 100)         // Random amount between 1 and 100
            };

            products.Add(product);
        }

        return products;
    }
}

