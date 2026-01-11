using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogServiceAPI_Electric_Store.Models.ModelView
{
    public class CategoryView
    {
        public int id { get; set; } = 0;

        [Required]
        public string name { get; set; } = "";
        [Required]
        public string slug { get; set; } = "";
        public int parent_id { get; set; } = 0;

        public string path { get; set; } = "";
        public int level { get; set; } = 0;


        [BindNever]
        public HashSet< CategoryAttributeView>? category_attributes { get; set; } = new HashSet< CategoryAttributeView>();
        public DateTime created_at  { get; set; } = DateTime.Now;


    }
}

