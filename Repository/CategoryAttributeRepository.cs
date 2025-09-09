using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{



    public class CategoryAttributeRepository : IRepository<CategoryAttributeView>

    {
        private readonly CatalogAPIContext _context;
        public CategoryAttributeRepository(CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(CategoryAttributeView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CategoryAttributeView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryAttributeView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryAttributeView> GetAll()
        {
           
            try
            {
                var categoryAttributes = _context.CategoryAttributes
                    .Select(ca => new CategoryAttributeView
                    {
                        id = ca.Id,
                        category_id = ca.CategoryId,
                        attribute_id = ca.AttributeId,
                        is_filterable = ca.IsFilterable,
                        is_variant_level = ca.IsVariantLevel,
                        is_required = ca.IsRequired
                    }).ToHashSet();
                return categoryAttributes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new HashSet<CategoryAttributeView>();
            }
        }

        public bool Update(CategoryAttributeView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<CategoryAttributeView> GetByCategoryId(int categoryId)
        {

            try
            {
                var categoryAttributes = _context.CategoryAttributes
                    .Where(ca => ca.CategoryId == categoryId)
                    .Select(ca => new CategoryAttributeView
                    {
                        id = ca.Id,
                        category_id = ca.CategoryId,
                        attribute_id = ca.AttributeId,
                        is_filterable = ca.IsFilterable,
                        is_variant_level = ca.IsVariantLevel,
                        is_required = ca.IsRequired
                    }).ToHashSet();
                return categoryAttributes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                return new HashSet<CategoryAttributeView>();
            }

        }
    }
}
