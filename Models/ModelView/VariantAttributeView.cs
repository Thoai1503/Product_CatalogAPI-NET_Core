using CatalogServiceAPI_Electric_Store.Models.Entities;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class VariantAttributeView
    {
        public int id { get; set; }

        public int attribute_id { get; set; } = 0;

        public int variant_id { get; set; } = 0;

        public int? value_int { get; set; } = null;

        public string? value_text { get; set; } = null!;

        public decimal? value_decimal { get; set; } = null!;

        public int? attribute_value_id { get; set; } = null;

        public AttributeValueView? attribute_value { get; set; } = null!;
        public virtual AttributeView? attribute { get; set; } = null!;

        public virtual ProductVariantView? variant { get; set; } = null!;
    }
}
