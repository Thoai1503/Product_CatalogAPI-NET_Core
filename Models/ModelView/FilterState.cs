using Microsoft.AspNetCore.Mvc;
using CatalogServiceAPI_Electric_Store.Helper;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{

    [ModelBinder(BinderType = typeof(FilterStateModelBinder))]
    public class FilterState
    {
        public int skip { get; set; } = 0;
        public int take { get; set; } = 10;

        public  string sortBy { get; set; } = "created_at";

        public string order { get; set; } = "asc";

        public string category { get; set; } = "";

        public Dictionary<string, List<string>> attributes { get; set; } = new();

        public List<int> categoryIds { get; set; } = new List<int>();
        public List<int> brandIds { get; set; } = new List<int>();
   
        public decimal? minPrice { get; set; } = null;
        public decimal? maxPrice { get; set; } = null;
    }
}
