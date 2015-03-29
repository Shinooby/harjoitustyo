using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsApp
{
    // Use the Service Proxy to Call the OData Service:
    class Program
    {
        // Haetaan kokonainen entiteetti setti.
        static void ListAllProducts(Default.Container container)
        {
            foreach (var p in container.Products)
            {
                Console.WriteLine("{0} {1} {2}", p.Name, p.Price, p.Category);
            }
        }

        static void AddProduct(Default.Container container,  Harjoitustyo4_lauri_pihlajamaki.Models.Product product)
        {
            container.AddToProducts(product);
            var serviceResponse = container.SaveChanges();
            foreach (var operationResponse in serviceResponse)
            {
                Console.WriteLine("Response: {0}", operationResponse.StatusCode);
            }
        }

        static void Main(string[] args)
        {
            // Laitetaan tähän oma URI.
            string serviceUri = "http://localhost:60061/";
            var container = new Default.Container(new Uri(serviceUri));

            var product = new Harjoitustyo4_lauri_pihlajamaki.Models.Product()
            {
                Name = "Yo-yo",
                Category = "Toys",
                Price = 4.95M
            };

            AddProduct(container, product);
            ListAllProducts(container);
        }
        // Saavutettiin tämän tutoriaalin sivun loppu:
        // http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-client-app
    }
}
