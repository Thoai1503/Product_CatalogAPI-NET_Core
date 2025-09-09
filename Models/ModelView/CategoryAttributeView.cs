using System;
using System.Collections.Generic;

namespace CatalogServiceAPI_Electric_Store.Repository;

public  class CategoryAttributeView
{
    public int id { get; set; } = 0;

    public int category_id { get; set; } = 0;

    public int attribute_id { get; set; } = 0;

    public bool is_filterable { get; set; } = false;

    public bool is_variant_level { get; set; } = false;

    public bool is_required { get; set; } = false;

   
}
