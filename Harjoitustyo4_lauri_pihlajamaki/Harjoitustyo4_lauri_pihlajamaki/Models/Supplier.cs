using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Harjoitustyo4_lauri_pihlajamaki.Models
{
    // Entity Relations in OData v4 Using ASP.NET Web API 2.2
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        // Uutta koodia:
        [ForeignKey("Product")]
        public int? productId { get; set; }
        public virtual Product Product { get; set; }
    }
}