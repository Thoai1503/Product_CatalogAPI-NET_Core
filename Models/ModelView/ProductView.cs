namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductView
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public int category_id { get; set; } = 0;
        public int brand_id { get; set; } = 0;


        public string slug { get; set; } = string.Empty;
        public decimal rating { get; set; } = decimal.Zero;

        public int status { get; set; } = 0;

        public CategoryView category { get; set; } = new CategoryView();
        public BrandView brand { get; set; } = new BrandView();
         
        public  HashSet <ProductAttributeView> product_attribute { get; set; } =new HashSet<ProductAttributeView>();

        public HashSet<ProductVariantView>? product_variant { get; set; } = new HashSet<ProductVariantView>();
        public DateTime created_at { get; set; } = DateTime.Now;

    }
}
