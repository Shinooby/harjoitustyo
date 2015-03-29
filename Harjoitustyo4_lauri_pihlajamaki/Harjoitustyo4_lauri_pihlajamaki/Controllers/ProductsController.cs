using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Harjoitustyo4_lauri_pihlajamaki.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.OData;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Harjoitustyo4_lauri_pihlajamaki.Controllers
{
    // Kontrolleri käyttää ProductsContext luokkaa päästäkseen käsiksi 
    // tietokantaan Entity Frameworkin avulla
    public class ProductsController : ODataController
    {


        // Metodi jolla lisätään yhteys Productiin
        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateRef([FromODataUri] int key,
            string navigationProperty, [FromBody] Uri link)
        {
            var product = await db.Products.SingleOrDefaultAsync(p => p.Id == key);
            if (product == null)
            {
                return NotFound();
            }
            switch (navigationProperty)
            {
                case "Supplier":
                    //Huom: Tänne lisätään GetKeyFromUri myöhemmin
                    var relatedKey = Helpers.GetKeyFromUri<int>(Request, link);
                    var supplier = await db.Suppliers.SingleOrDefaultAsync(f => f.Id == relatedKey);
                    if (supplier == null)
                    {
                        return NotFound();
                    }
                    product.Supplier = supplier;
                    break;

                default:
                    return StatusCode(HttpStatusCode.NotImplemented);
            }
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // Metodi jolla poistetaan yhteys Productin ja Supplierin väliltä
        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key,
            string navigationProperty, [FromBody] Uri link)
        {
            var product = db.Products.SingleOrDefault(p => p.Id == key);
            if (product == null)
            {
                return NotFound();
            }

            switch (navigationProperty)
            {
                case "Supplier":
                    product.Supplier = null;
                    break;

                default:
                    return StatusCode(HttpStatusCode.NotImplemented);
            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }


        // GET /Products(1)/Supplier
        [EnableQuery]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            var result = db.Products.Where(m => m.Id == key).Select(m => m.Supplier);
            return SingleResult.Create(result);
        }


        // Add the OData Controller
        ProductsContext db = new ProductsContext();
        private bool ProductExists(int key)
        {
            return db.Products.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return db.Products;
        }
        // Querying the Entity Set:
        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = db.Products.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }
        //EnableQuery mahdollistaa haun muokkaamisen.


        // Adding an Entity to the Entity Set:
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        }
        // Tämä mahdollistaa uusien tuotteiden lisäämisen tietokantaan.


        // Updating an Entity: PATCH ja PUT-metodit
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Products.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            product.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        //Deleting an Entity.
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await db.Products.FindAsync(key);
            if(product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // Päädyttiin tämän tutoriaalisivun loppuun:
        // http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint


            // Uutta koodia, Tutoriaalin neljäs sivu:
        [HttpPost]
        public async Task<IHttpActionResult> Rate([FromODataUri] int key, ODataActionParameters parameters)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int rating = (int)parameters["Rating"];
            db.Ratings.Add(new ProductRating
                {
                    ProductID = key,
                    Rating = rating
                });

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);

        }
            [HttpGet]
        public IHttpActionResult MostExpensive()
        {
            var product = db.Products.Max(x => x.Price);
            return Ok(product);
        }

            [HttpGet]
        [ODataRoute("GetSalesTaxRate(PostalCode={postalCode})")]
        public IHttpActionResult GetSalesTaxRate([FromODataUri] int postalCode)
        {
            double rate = 5.6;  // Use a fake number for the sample.
            return Ok(rate);
        }
        // Ja päästiin tämän tutoriaalisivun loppuun:
        // http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/odata-actions-and-functions


    }

}