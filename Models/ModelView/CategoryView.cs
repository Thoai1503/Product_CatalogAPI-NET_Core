using CatalogServiceAPI_Electric_Store.Repository;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class CategoryView
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public int parent_id { get; set; } = 0;

        public string path { get; set; } = "";
        public int level { get; set; } = 0;

        public HashSet< CategoryAttributeView> category_attributes { get; set; } = new HashSet< CategoryAttributeView>();
        public DateTime created_at  { get; set; } = DateTime.Now;


    }
}

