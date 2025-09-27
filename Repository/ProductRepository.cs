using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogServiceAPI_Electric_Store.Repository

{
    public class ProductRepository : IRepository<ProductView>

    {
        private readonly CatalogAPIContext _context;

        public ProductRepository (CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(ProductView entity)
        {
            try
            {
                var en = new Product
                {
                    Name = entity.name,
                    Description = entity.description,
                    CategoryId = entity.category_id,
                    
                    BrandId = entity.brand_id,
                    Rating = entity.rating,
                    Status = entity.status,
                    CreatedAt = DateTime.Now,


                };
                _context.Products.Add(en);
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
            throw new NotImplementedException();
        }

        public ProductView FindById(int id)
        {
            try
            {
                var en = _context.Products.Include(n=>n.ProductAttributes).Include(p => p.Category).ThenInclude(c => c.CategoryAttributes)
                    .ThenInclude(ca => ca.Attribute).Include(c => c.Brand).Include(pa=>pa.ProductAttributes).ThenInclude(a=>a.Attribute).FirstOrDefault(c=>c.Id==id);

                var json = JsonSerializer.Serialize(en, new JsonSerializerOptions
                {
                    WriteIndented = true, // format đẹp dễ đọc
                    ReferenceHandler = ReferenceHandler.IgnoreCycles // tránh vòng lặp giữa navigation properties
                });

                Console.WriteLine(json);
                return new ProductView
                {
                    id = en.Id,
                    name = en.Name,
                    description = en.Description,
                    category_id = en.CategoryId,
                    brand_id = (int)en.BrandId,
                    rating = en.Rating,
                    status = en.Status,
                    brand = new BrandView
                    {
                        id = en.Brand.Id,
                        name = en.Brand.Name,
                        slug = en.Brand.Slug

                    },
                    category = new CategoryView
                    {
                        id = en.Category.Id,
                        name = en.Category.Name,
                        slug = en.Category.Slug,
                        parent_id = (int)en.Category.ParentId,
                        path = en.Category.Path,
                        level = en.Category.Level,
                        category_attributes = en.Category.CategoryAttributes?.Select(ca=>new CategoryAttributeView
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
                                 status = ca.Attribute.Status
                             }
                         }).ToHashSet() ?? new HashSet<CategoryAttributeView>(),
                         
                    },
                    product_attribute = en.ProductAttributes?.Select(e=> new ProductAttributeView
                    {
                        id = e.Id,
                        product_id = e.ProductId,
                        attribute_id=e.Attribute.Id,
                        value_decimal = e.ValueDecimal,
                        value_int = e.ValueInt,
                        value_text = e.ValueText,
                        attribute = new AttributeView
                        {
                            id = e.Attribute.Id,
                            name=e.Attribute.Name,
                            slug = e.Attribute.Slug,
                            data_type = e.Attribute.DataType, unit = e.Attribute.Unit,
                            status = e.Attribute.Status

                        }


                    }).ToHashSet() ?? new HashSet<ProductAttributeView>(),
                };
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public HashSet<ProductView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductView> GetAll()
        {


            return _context.Products.Include(c=>c.Brand).Include(c=>c.Category)
             
               .Select(en => new ProductView
               {
                   id = en.Id,
                   name = en.Name,
                   description = en.Description,
                   category_id = en.CategoryId,
                   brand_id =(int) en.BrandId,
                   rating = en.Rating,
                   status = en.Status,
                   brand = new BrandView
                   {
                       id = en.Brand.Id,
                       name = en.Brand.Name,
                       slug =en.Brand.Slug

                   },
                   category =new CategoryView
                   {
                       id = en.Category.Id,
                       name = en.Category.Name,
                       slug = en.Category.Slug,
                       parent_id =(int) en.Category.ParentId,
                       path =en.Category.Path,
                       level= en.Category.Level
                      
                   }
                 
               })
               .ToHashSet();
        }

        public ProductView CreateAndReturn(ProductView product)
        {
            try
            {
                var en = new Product
                {
                    Name= product.name,
                    Description= product.description,
                    BrandId= product.brand_id,
                    CategoryId= product.category_id,
                    Slug =product.slug,
                    Rating=product.rating,
                    Status=product.status,
                };
                _context.Products.Add(en);
                _context
                    .SaveChanges();
                var json = JsonSerializer.Serialize(en, new JsonSerializerOptions
                {
                    WriteIndented = true, // format đẹp dễ đọc
                    ReferenceHandler = ReferenceHandler.IgnoreCycles // tránh vòng lặp giữa navigation properties
                });
                var entity = new ProductView
                {
                    id= product.id,
                    name= product.name,
                    slug= product.slug,
                    brand_id= product.brand_id,
                    category_id= product.category_id,

                };
                var result = HandleProductInsert(en);
         
                if(!result) return null;
                return entity;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public bool Update(ProductView entity)
        {
            throw new NotImplementedException();
        }
        private bool HandleProductInsert(Product product)
        {
            try
            {
                var productId = product.Id;
                var categoryId = product.CategoryId;
                var categoryAttributes = _context.CategoryAttributes
            .Where(ca => ca.CategoryId == categoryId && ca.IsVariantLevel == false)
            .Select(ca => ca.AttributeId)
            .ToList();
                var existingPairs = _context.ProductAttributes
                .Where(pa => pa.ProductId == productId && categoryAttributes.Contains(pa.AttributeId))
                .Select(pa => pa.AttributeId)
                .ToHashSet();
                var newProductAttributes = new List<ProductAttribute>();
                foreach (var attrId in categoryAttributes)
                {
                    if (!existingPairs.Contains(attrId))
                    {
                        newProductAttributes.Add(new ProductAttribute
                        {
                            ProductId = productId,
                            AttributeId = attrId
                        });
                    }
                }
                if (newProductAttributes.Any())
                {
                    _context.ProductAttributes.AddRange(newProductAttributes);

                    var result = JsonSerializer.Serialize(newProductAttributes, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    });
                    Console.WriteLine("[DEBUG] Created ProductAttributes: " + result);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
