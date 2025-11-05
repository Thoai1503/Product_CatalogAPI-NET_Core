namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class FilterState
    {
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;

        public  string sortBy { get; set; } = "created_at";

        public string order { get; set; } = "asc";



        public List<int> categoryIds { get; set; } = new List<int>();
        public List<int> brandIds { get; set; } = new List<int>();
        public Dictionary<string, List<string>> attributes { get; set; } = new Dictionary<string, List<string>>();
        public decimal? minPrice { get; set; } = null;
        public decimal? maxPrice { get; set; } = null;
    }
}
