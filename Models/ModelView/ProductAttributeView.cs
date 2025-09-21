using CatalogServiceAPI_Electric_Store.Models.Entities;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductAttributeView
    {
        public int id { get; set; } = 0;

        public int product_id { get; set; } = 0;

        public int attribute_id { get; set; } = 0;

        public string? value_text { get; set; } = string.Empty;

        public decimal? value_decimal { get; set; } = decimal.Zero;

        public int? value_int { get; set; } = 0;

        public  AttributeView? attribute { get; set; } = null!;

      //  public  ProductView? product { get; set; } = new ProductView();
    }
}
