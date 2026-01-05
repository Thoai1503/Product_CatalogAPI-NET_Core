namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductAddAndUpdateState
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public int category_id { get; set; } = 0;
        public int brand_id { get; set; } = 0;


        public string slug { get; set; } = string.Empty;
        public decimal rating { get; set; } = decimal.Zero;

        public int status { get; set; } = 0;

    }
}
