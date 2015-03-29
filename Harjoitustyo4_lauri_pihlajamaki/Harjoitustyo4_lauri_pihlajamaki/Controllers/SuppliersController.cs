using Harjoitustyo4_lauri_pihlajamaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Harjoitustyo4_lauri_pihlajamaki.Models;

/*
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.OData;
using System.Web.Http;

*/
namespace Harjoitustyo4_lauri_pihlajamaki.Controllers
{
    public class SuppliersController : ODataController
    {
        // Add a Suppliers Controller:
        ProductsContext db = new ProductsContext();

        // GET /Suppliers(1)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Suppliers.Where(m => m.Id.Equals(key)).SelectMany(m => m.Products);
        }

        private bool SupplierExists(int key)
        {
            return db.Suppliers.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key,
               [FromODataUri] string relatedKey, string navigationProperty)
        {
            var supplier = await db.Suppliers.SingleOrDefaultAsync(p => p.Id == key);
            if (supplier == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            switch (navigationProperty)
            {
                case "Products":
                    var productId = Convert.ToInt32(relatedKey);
                    var product = await db.Products.SingleOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.Supplier = null;
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);

            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Päästiin tämän tutoriaalisivun loppuun:
        // http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/entity-relations-in-odata-v4

    }
}