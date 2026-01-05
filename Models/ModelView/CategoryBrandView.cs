namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class CategoryBrandView
    {
        public int id { get; set; }

        public int brand_id { get; set; }

        public int category_id { get; set; }

        public CategoryView? category { get; set; }

        public BrandView? brand { get; set; }  
    }
}
