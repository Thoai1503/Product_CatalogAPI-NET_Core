using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class AttributeValueView
    {

        public int id { get; set; } = 0;


        public int attribute_id { get; set; } = 0;

        public string value { get; set; } = "";

    }
}
