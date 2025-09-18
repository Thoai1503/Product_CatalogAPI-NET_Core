namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public int brand_id { get; set; } = 0;



        public decimal rating { get; set; } = decimal.Zero;

        public int status { get; set; } = 1;

        public CategoryView category { get; set; } = new CategoryView();
        public BrandView brand { get; set; } = new BrandView();
        public DateTime created_at { get; set; }

    }
}
