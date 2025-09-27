using CatalogServiceAPI_Electric_Store.Models.Entities;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductImageView 
    {
        public int id { get; set; } = 0;

        public int product_id { get; set; }

        public int variant_id { get; set; }

        public string? url { get; set; } = null!;

        public virtual ProductVariantView? variant { get; set; } = null!;
    }
}
