using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class CategoryAttributeUpdateState
    {
        public int id { get; set; } = 0;

        [Required]
        public int category_id { get; set; } = 0;

        [Required]
        public int attribute_id { get; set; } = 0;

        public bool is_filterable { get; set; } = false;

        public bool is_variant_level { get; set; } = false;

        public bool is_required { get; set; } = false;
    }
}
