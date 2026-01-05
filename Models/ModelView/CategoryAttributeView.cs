using CatalogServiceAPI_Electric_Store.Models.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogServiceAPI_Electric_Store.Repository;

public  class CategoryAttributeView
{
    public int id { get; set; } = 0;

    [Required]
    public int category_id { get; set; } = 0;

    [Required]  
    public int attribute_id { get; set; } = 0;

    public bool is_filterable { get; set; } = false;

    public bool is_variant_level { get; set; } = false;

    public bool is_required { get; set; } = false;


    public CategoryView? category { get; set; }= new CategoryView();

    public AttributeView? attribute { get; set; }  = new AttributeView();

}
