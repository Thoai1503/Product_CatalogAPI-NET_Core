using CatalogServiceAPI_Electric_Store.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductVariantView
    {
        
        public int id { get; set; } = 1;
        [Required]
        public int product_id { get; set; } = 0;
        [Required]
        public string sku { get; set; } = null!;
        [Required]
        public int price { get; set; } = 0;

        public int status { get; set; } = 0;

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public  ProductView? product { get; set; } = null!;

        public virtual ICollection<ProductImageView>? product_images { get; set; } = null;

        public virtual ICollection<VariantAttributeView>? variant_attributes { get; set; } = null;
    }
}
