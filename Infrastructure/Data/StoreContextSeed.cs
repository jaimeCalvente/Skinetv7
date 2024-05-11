using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.ProductBrands.Any())
            {
                // Read all the Json file
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                // Converts the Json file into a C# object(ProductBrand) List
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                // Add the ProductBrands into the DB
                context.ProductBrands.AddRange(brands);
            }

            if(!context.ProductTypes.Any())
            {
                // Read all the Json file
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                // Converts the Json file into a C# object(ProductType) List
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                // Add the ProductType into the DB
                context.ProductTypes.AddRange(types);
            }

            if(!context.Products.Any())
            {
                // Read all the Json file
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                // Converts the Json file into a C# object(ProductBrand) List
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                // Add the ProductBrands into the DB
                context.Products.AddRange(products);
            }

            // Save the possible changes into the DB
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
           
        }
    }
}