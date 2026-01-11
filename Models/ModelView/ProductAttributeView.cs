using CatalogServiceAPI_Electric_Store.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class ProductAttributeView
    {
        public int id { get; set; } = 0;

        [Required]
        public int product_id { get; set; } = 0;

        [Required]
        public int attribute_id { get; set; } = 0;

        public string? value_text { get; set; } = null;


        [ValidateNever]
        public decimal? value_decimal { get; set; } = null;

        public int? value_int { get; set; } = null;

        public int? attribute_value_id { get; set; } = null;


        [ValidateNever]
        public  AttributeView? attribute { get; set; } = null!;

      //  public  ProductView? product { get; set; } = new ProductView();
    }
}
