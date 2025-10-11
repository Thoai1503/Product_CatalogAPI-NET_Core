namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class CartView
    {
        public int id { get; set; } =0;
        
        public int user_id { get; set; } = 0;
        public int variant_id { get; set; } = 0;
        public int quantity { get; set; } = 0;
        public ProductVariantView? variant { get; set; } = null;
    }
}
