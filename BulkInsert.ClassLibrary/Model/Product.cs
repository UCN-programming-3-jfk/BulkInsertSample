using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BulkInsert.ClassLibrary.Model;
public class Product 
{
    public int? Id { get; private set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int ExternalId { get; set; }
    public float? Amount { get; set; }
}
