using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
// Lisättiin:
using Harjoitustyo4_lauri_pihlajamaki.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Harjoitustyo4_lauri_pihlajamaki
{
    public static class WebApiConfig
    {
          public static void Register(HttpConfiguration config)
    {
       
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<Product>("Products");

        // Uutta koodia:
        builder.Function("GetSalesTaxRate")
            .Returns<double>()
            .Parameter<int>("PostalCode");

        builder.EntitySet<Supplier>("Suppliers");
        config.MapODataServiceRoute("ODataRoute", null, builder.GetEdmModel());

        builder.Namespace = "ProductService";
        builder.EntityType<Product>().Collection
            .Function("MostExpensive")
            .Returns<double>();

    

    }
        // Tämä luo Entity Data Modelin (EDM) ja lisää reitin, joka kertoo Web API:lle
        // kuinka HTTP-kutsut menevät endpointtiin.

    }
}
