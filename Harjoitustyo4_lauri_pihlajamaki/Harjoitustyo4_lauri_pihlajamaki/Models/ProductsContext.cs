using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Harjoitustyo4_lauri_pihlajamaki.Models
{
    public class ProductsContext : DbContext
    {
        public ProductsContext()
                : base("name=ProductsContext")
        { // Konstruktorissa "name=ProductsContext" antaa yhteys-stringin nimen
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<ProductRating> Ratings { get; set; }
    }
}