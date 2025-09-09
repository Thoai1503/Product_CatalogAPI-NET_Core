namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class AttributeView
    {

        public int id { get; set; }= 0;

        public string name { get; set; }= "";

        public string slug { get; set; } ="";

        public string data_type { get; set; } = "";

        public string unit { get; set; } = "";

        public int status { get; set; } = 1;
        public int is_selected { get; set; } = 0;
    }
}
