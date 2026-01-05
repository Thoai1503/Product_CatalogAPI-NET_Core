namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductVariantPaginated
    {
        public HashSet<ProductVariantView> data { get; set; }
         
        public int count { get; set; }
    }
}
