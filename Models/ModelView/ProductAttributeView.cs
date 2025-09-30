using CatalogServiceAPI_Electric_Store.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductAttributeView
    {
        public int id { get; set; } = 0;

        [Required]
        public int product_id { get; set; } = 0;

        [Required]
        public int attribute_id { get; set; } = 0;

        public string? value_text { get; set; } = string.Empty;

        public decimal? value_decimal { get; set; } = decimal.Zero;

        public int? value_int { get; set; } = 0;

        public  AttributeView? attribute { get; set; } = null!;

      //  public  ProductView? product { get; set; } = new ProductView();
    }
}
