using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class BrandView
    {
        [Required]
        public string name { get; set; } = "";

        public string slug { get; set; } = "";

        public int status { get; set; } = 1;


        public int id { get; set; } = 0;
    }
}
