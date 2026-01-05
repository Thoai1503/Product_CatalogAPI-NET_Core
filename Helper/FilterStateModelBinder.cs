
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
namespace CatalogServiceAPI_Electric_Store.Helper
{
    public class FilterStateModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.HttpContext.Request.Query;
            var model = new FilterState();

            // Các giá trị cơ bản
            if (query.TryGetValue("skip", out var skipVal) && int.TryParse(skipVal, out var skip))
                model.skip = skip;
            if (query.TryGetValue("take", out var takeVal) && int.TryParse(takeVal, out var take))
                model.take = take;
            if (query.TryGetValue("sortBy", out var sortBy))
                model.sortBy = sortBy;
            if (query.TryGetValue("order", out var order))
                model.order = order;
            if (query.TryGetValue("category", out var category))
                model.category = category;

            if (query.TryGetValue("minPrice", out var minPrice) && decimal.TryParse(minPrice, out var min))
                model.minPrice = min;
            if (query.TryGetValue("maxPrice", out var maxPrice) && decimal.TryParse(maxPrice, out var max))
                model.maxPrice = max;

            // categoryIds[]
            if (query.TryGetValue("categoryIds", out var catIds))
                model.categoryIds = catIds
                    .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => int.TryParse(x, out var id) ? id : 0)
                    .Where(x => x > 0)
                    .ToList();

            // brandIds[]
            if (query.TryGetValue("brandIds", out var brandVals))
                model.brandIds = brandVals
                    .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => int.TryParse(x, out var id) ? id : 0)
                    .Where(x => x > 0)
                    .ToList();

            // attributes.*
            foreach (var kv in query)
            {
                 var k = kv.Key;
                if (kv.Key.StartsWith("attributes.", StringComparison.OrdinalIgnoreCase))
                {
                    var keyName = kv.Key.Replace("attributes.", "");
                    var values = kv.Value
                        .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        .ToList();
                    model.attributes[keyName] = values;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
