using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;

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
            try
            {

                var en = new CategoryAttribute
                {
                    AttributeId = entity.attribute_id,
                    CategoryId = entity.category_id,
                    IsFilterable = false,
                    IsVariantLevel = false,
                    IsRequired = false,
                };
                _context.Add(en);
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool Delete(int id)
        {

            try
            {
                var en = _context.CategoryAttributes.FirstOrDefault(x => x.Id == id);
                if (en != null)
                {
                    _context.CategoryAttributes.Remove(en);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public CategoryAttributeView FindById(int id)
        {
            
            try
            {
                var ca = _context.CategoryAttributes.Include
                    (ca => ca.Attribute)
                    .FirstOrDefault(ca => ca.Id == id);
                if (ca == null)
                {
                    return null; // Entity not found
                }
                var categoryAttributeView = new CategoryAttributeView
                {
                    id = ca.Id,
                    category_id = ca.CategoryId,
                    attribute_id = ca.AttributeId,
                   
                    is_filterable = ca.IsFilterable,
                    is_variant_level = ca.IsVariantLevel,
                    is_required = ca.IsRequired,
                    attribute = new AttributeView
                    {
                        id = ca.Attribute.Id,
                        name = ca.Attribute.Name,
                        slug = ca.Attribute.Slug,
                        data_type = ca.Attribute.DataType,
                        unit = ca.Attribute.Unit,
                        status = ca.Attribute.Status,
                    }
                };
                return categoryAttributeView;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null; // Error occurred
            }

        }

        public HashSet<CategoryAttributeView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryAttributeView> GetAll()
        {
           
            try
            {
                var categoryAttributes = _context.CategoryAttributes.Include
                    (ca => ca.Attribute)
                    .Select(ca => new CategoryAttributeView
                    {
                        id = ca.Id,
                        category_id = ca.CategoryId,
                        attribute_id = ca.AttributeId,
                       
                        is_filterable = ca.IsFilterable,
                        is_variant_level = ca.IsVariantLevel,
                        is_required = ca.IsRequired,
                        attribute = new AttributeView
                        {
                            id = ca.Attribute.Id,
                            name = ca.Attribute.Name,
                            slug = ca.Attribute.Slug,
                            data_type = ca.Attribute.DataType,
                            unit = ca.Attribute.Unit,
                            status = ca.Attribute.Status,
                        }
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
            try
            {

                var ca = _context.CategoryAttributes.FirstOrDefault(ca => ca.Id == entity.id);
                if (ca == null)
                {
                    return false; // Entity not found
                }
                // Update fields
                ca.CategoryId = entity.category_id;
                ca.AttributeId = entity.attribute_id;
                ca.IsFilterable = entity.is_filterable;
                ca.IsVariantLevel = entity.is_variant_level;
                ca.IsRequired = entity.is_required;
                _context.CategoryAttributes.Update(ca);
                _context.SaveChanges();
                return true; // Update successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Error occurred
            }

        }
        public HashSet<CategoryAttributeView> GetByCategoryId(int categoryId)
        {

            try
            {
                var categoryAttributes = _context.CategoryAttributes.Include
                    (ca => ca.Attribute)
                    .Where(ca => ca.CategoryId == categoryId)
                    .Select(ca => new CategoryAttributeView
                    {
                        id = ca.Id,
                        category_id = ca.CategoryId,
                        attribute_id = ca.AttributeId,
                    
                        is_filterable = ca.IsFilterable,
                        is_variant_level = ca.IsVariantLevel,
                        is_required = ca.IsRequired,
                        attribute = new AttributeView
                        {
                            id = ca.Attribute.Id,
                            name = ca.Attribute.Name,
                            slug = ca.Attribute.Slug,
                            data_type = ca.Attribute.DataType,
                            unit = ca.Attribute.Unit,
                            status = ca.Attribute.Status,

                        }
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
