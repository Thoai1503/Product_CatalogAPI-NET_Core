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

        public DateTime created_at  { get; set; } = DateTime.Now;


    }
}
