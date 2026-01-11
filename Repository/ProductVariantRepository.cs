using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class ProductVariantRepository : IRepository<ProductVariantView>
    {
        private readonly CatalogAPIContext _context;


        public ProductVariantRepository(CatalogAPIContext context)
        {
            _context = context;
 
        }

        public bool Create(ProductVariantView entity)
        {

            try {
                var en = new ProductVariant
                {
                   
                    ProductId = entity.product_id,
                    Name = entity.name,
                    Sku = entity.sku,
                    Price = entity.price,
                    Status = entity.status,
                    CreatedAt = DateTime.Now,



                };
                _context.ProductVariants.Add(en);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var en = _context.ProductVariants.FirstOrDefault(x => x.Id == id);
                if (en != null)
                {
                    _context.ProductVariants.Remove(en);
                    _context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ProductVariantView FindById(int id)
        {
            try
            {
                var x = _context.ProductVariants
                    .Include(pv => pv.Product).ThenInclude(p => p.Category)
                    .Include(pv => pv.Product).ThenInclude(p => p.Brand)
                    .Include(pv => pv.ProductImages)
                    .Include(pv => pv.VariantAttributes).ThenInclude(va => va.AttributeValue).ThenInclude(av => av.Attribute)
                    .Include(pv => pv.VariantAttributes).ThenInclude(va => va.Attribute)
                    .Include(pv => pv.Product).ThenInclude(p => p.ProductAttributes).ThenInclude(pa => pa.Attribute)
                    .FirstOrDefault(pv => pv.Id == id);

                if (x == null) return null;

                return new ProductVariantView
                {
                    id = x.Id,
                    product_id = x.ProductId,
                    name = x.Name,
                    price = x.Price,
                    sku = x.Sku,
                    status = x.Status,
                    created_at = x.CreatedAt,
                    product = x.Product != null ? new ProductView
                    {
                        id = x.Product.Id,
                        category_id = x.Product.CategoryId,
                        brand_id =(int) x.Product.BrandId,
                        name = x.Product.Name,
                        slug = x.Product.Slug,
                        description = x.Product.Description,
                        status = x.Product.Status,
                        created_at = x.Product.CreatedAt,
                        product_variant = x.Product.ProductVariants != null
                            ? x.Product.ProductVariants.Select(v => new ProductVariantView
                            {
                                id = v.Id,
                                product_id = v.ProductId,
                                name = v.Name,
                                price = v.Price,
                                sku = v.Sku,
                                status = v.Status,
                                created_at = v.CreatedAt
                            }).ToHashSet()
                            : new HashSet<ProductVariantView>(),
                        product_attribute = x.Product.ProductAttributes != null
                            ? x.Product.ProductAttributes.Select(e => new ProductAttributeView
                            {
                                id = e.Id,
                                product_id = e.ProductId,
                                attribute_id = e.AttributeId,
                                value_decimal = e.ValueDecimal,
                                value_int = e.ValueInt,
                                value_text = e.ValueText,
                                attribute = e.Attribute != null ? new AttributeView
                                {
                                    id = e.Attribute.Id,
                                    name = e.Attribute.Name,
                                    slug = e.Attribute.Slug,
                                    data_type = e.Attribute.DataType,
                                    unit = e.Attribute.Unit,
                                    status = e.Attribute.Status
                                } : null
                            }).ToHashSet()
                            : new HashSet<ProductAttributeView>()
                    } : null,
                    product_images = x.ProductImages != null
                        ? x.ProductImages.Select(e => new ProductImageView
                        {
                            id = e.Id,
                            product_id = e.ProductId,
                            variant_id = e.VariantId,
                            url = e.Url
                        }).ToHashSet()
                        : new HashSet<ProductImageView>(),
                    variant_attributes = x.VariantAttributes != null
                        ? x.VariantAttributes.Select(e => new VariantAttributeView
                        {
                            id = e.Id,
                            variant_id = e.VariantId,
                            attribute_id = e.AttributeId,
                            value_decimal = e.ValueDecimal,
                            value_int = e.ValueInt,
                            value_text = e.ValueText,
                            attribute_value_id = e.AttributeValueId,
                            attribute_value = e.AttributeValue != null ? new AttributeValueView
                            {
                                id = e.AttributeValue.Id,
                                attribute_id = e.AttributeValue.AttributeId,
                                value = e.AttributeValue.Value
                            } : null,
                            attribute = e.Attribute != null ? new AttributeView
                            {
                                id = e.Attribute.Id,
                                name = e.Attribute.Name,
                                slug = e.Attribute.Slug,
                                data_type = e.Attribute.DataType,
                                unit = e.Attribute.Unit,
                                status = e.Attribute.Status
                            } : null
                        }).ToHashSet()
                        : new HashSet<VariantAttributeView>()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public HashSet<ProductVariantView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductVariantView> GetAll(FilterState st)
        {
            try
            {
                // Start with IQueryable for database-level filtering
                IQueryable<ProductVariant> query = _context.ProductVariants
                    .Include(c => c.Product)
                        .ThenInclude(c => c.Category)
                    .Include(x => x.ProductImages)
                    .Include(x => x.VariantAttributes)
                        .ThenInclude(c => c.AttributeValue)
                            .ThenInclude(c => c.Attribute)
                    .Include(c => c.Product)
                        .ThenInclude(c => c.ProductAttributes)
                            .ThenInclude(c => c.Attribute);

                // Apply category filter
                if (!string.IsNullOrEmpty(st.category))
                {
                    query = query.Where(e => e.Product.Category.Slug.ToLower().Trim() == st.category.Trim().ToLower());
                }

                // Apply price range filter
                if (st.minPrice.HasValue)
                {
                    query = query.Where(e => e.Price >= st.minPrice.Value);
                }
                if (st.maxPrice.HasValue)
                {
                    query = query.Where(e => e.Price <= st.maxPrice.Value);
                }

                // Apply category IDs filter
                if (st.categoryIds != null && st.categoryIds.Count > 0)
                {
                    query = query.Where(e => st.categoryIds.Contains(e.Product.CategoryId));
                }

      
                if (!string.IsNullOrEmpty(st.sortBy) && !string.IsNullOrEmpty(st.order))
                {
                    query = st.sortBy.ToLower() switch
                    {
                        "created_at" => st.order.ToLower() == "asc"
                            ? query.OrderBy(e => e.CreatedAt)
                            : query.OrderByDescending(e => e.CreatedAt),
                        "price" => st.order.ToLower() == "asc"
                            ? query.OrderBy(e => e.Price)
                            : query.OrderByDescending(e => e.Price),
                        _ => query.OrderBy(e => e.CreatedAt) // Default sorting
                    };
                }

                // Execute query and get results
                var list = query.ToList();

                  var brandList=  st.attributes.FirstOrDefault(at => at.Key == "0");

                if (brandList.Key!= null && brandList.Value!=null) {
                    var va = brandList.Value;
                
                        list = list.Where(
                            variant=> va.Any(v=>variant.Product.BrandId== (int.TryParse(v.Trim(), out var attrValId) ? attrValId : 0))
                            
                            
                            ).ToList();
                    

                    st.attributes.Remove("0");
                }
                if (st.attributes != null && st.attributes.Count > 0)
                {

                    foreach (var attr in st.attributes)
                    {
                        if (!int.TryParse(attr.Key, out var attributeId))
                            continue;
                

                        list = list.Where(variant =>
                            attr.Value.Any(value =>
                                variant.VariantAttributes.Any(va =>
                                    va.AttributeId == attributeId &&
                                    (
                                        (va.ValueText != null && va.ValueText.Trim().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)) ||
                                        (va.ValueInt != null && va.ValueInt.ToString() == value.Trim()) ||
                                        (va.ValueDecimal != null && va.ValueDecimal.ToString() == value.Trim()) ||
                                        (va.AttributeValueId != null && va.AttributeValueId == (int.TryParse(value.Trim(), out var attrValId) ? attrValId : 0))
                                    )
                                )
                            )
                        ).ToList();
                    }
                }

             
                var totalCount = list.Count;
                var paginatedList = list.Skip(st.skip).Take(st.take).ToList();

                
                return paginatedList.Select(x => new ProductVariantView
                {
                    id = x.Id,
                    product_id = x.ProductId,
                    name = x.Name,
                    price = x.Price,
                    sku = x.Sku,
                    created_at = x.CreatedAt,
                    product = new ProductView
                    {
                        id = x.Product.Id,
                        name = x.Product.Name,
                        slug = x.Product.Slug,
                        description = x.Product.Description,
                        status = x.Product.Status,
                        category = new CategoryView
                        {
                            id = x.Product.Category.Id,
                            name = x.Product.Category.Name.Trim(),
                            slug = x.Product.Category.Slug.ToLower().Trim(),
                        },
                        created_at = x.Product.CreatedAt,
                        product_attribute = x.Product.ProductAttributes.Select(e => new ProductAttributeView
                        {
                            id = e.Id,
                            product_id = e.ProductId,
                            attribute_id = e.AttributeId,
                            value_decimal = e.ValueDecimal,
                            value_int = e.ValueInt,
                            value_text = e.ValueText,
                            attribute = new AttributeView
                            {
                                id = e.Attribute.Id,
                                name = e.Attribute.Name,
                                slug = e.Attribute.Slug,
                                data_type = e.Attribute.DataType,
                                unit = e.Attribute.Unit,
                                status = e.Attribute.Status,
                            }
                        }).ToHashSet()
                    },
                    product_images = x.ProductImages.Select(e => new ProductImageView
                    {
                        id = e.Id,
                        product_id = e.ProductId,
                        variant_id = e.VariantId,
                        url = e.Url,
                    }).ToHashSet(),
                    variant_attributes = x.VariantAttributes.Select(e => new VariantAttributeView
                    {
                        id = e.Id,
                        variant_id = e.VariantId,
                        attribute_id = e.AttributeId,
                        value_decimal = e.ValueDecimal,
                        value_int = e.ValueInt,
                        value_text = e.ValueText,
                        attribute_value_id = e.AttributeValueId,
                        attribute_value = e.AttributeValue != null ? new AttributeValueView
                        {
                            id = e.AttributeValue.Id,
                            attribute_id = e.AttributeValue.AttributeId,
                            value = e.AttributeValue.Value ?? string.Empty
                        } : null,
                        attribute = e.Attribute != null ? new AttributeView
                        {
                            id = e.Attribute.Id,
                            name = e.Attribute.Name,
                            slug = e.Attribute.Slug,
                            data_type = e.Attribute.DataType,
                            unit = e.Attribute.Unit,
                            status = e.Attribute.Status,
                        } : null
                    }).ToHashSet()
                }).ToHashSet();
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error: " + ex.Message, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductVariantPaginated GetPaginationData(FilterState st)
        {
            try
            {
                // Start with IQueryable for database-level filtering
                IQueryable<ProductVariant> query = _context.ProductVariants
                    .Include(c => c.Product)
                        .ThenInclude(c => c.Category)
                    .Include(x => x.ProductImages)
                    .Include(x => x.VariantAttributes)
                        .ThenInclude(c => c.AttributeValue)
                            .ThenInclude(c => c.Attribute)
                    .Include(c => c.Product)
                        .ThenInclude(c => c.ProductAttributes)
                            .ThenInclude(c => c.Attribute);

                var maxPriceInDb = query.Max(e => e.Price);
                var minPriceInDb = query.Min(e => e.Price);

                // Apply category filter
                if (!string.IsNullOrEmpty(st.category))
                {
                    query = query.Where(e => e.Product.Category.Slug.ToLower().Trim() == st.category.Trim().ToLower());
                }

                // Apply price range filter
                if (st.minPrice.HasValue)
                {
                    query = query.Where(e => e.Price >= st.minPrice.Value);
                }
                if (st.maxPrice.HasValue)
                {
                    query = query.Where(e => e.Price <= st.maxPrice.Value);
                }

                // Apply category IDs filter
                if (st.categoryIds != null && st.categoryIds.Count > 0)
                {
                    query = query.Where(e => st.categoryIds.Contains(e.Product.CategoryId));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(st.sortBy) && !string.IsNullOrEmpty(st.order))
                {
                    query = st.sortBy.ToLower() switch
                    {
                        "created_at" => st.order.ToLower() == "asc"
                            ? query.OrderBy(e => e.CreatedAt)
                            : query.OrderByDescending(e => e.CreatedAt),
                        "price" => st.order.ToLower() == "asc"
                            ? query.OrderBy(e => e.Price)
                            : query.OrderByDescending(e => e.Price),
                        _ => query.OrderBy(e => e.CreatedAt) // Default sorting
                    };
                }

                // Execute query and get results
                var list = query.ToList();

                // Filter by Brand (attribute key "0")
                var brandList = st.attributes.FirstOrDefault(at => at.Key == "0");
                if (brandList.Key != null && brandList.Value != null)
                {
                    var va = brandList.Value;
                    list = list.Where(
                        variant => va.Any(v => variant.Product.BrandId == (int.TryParse(v.Trim(), out var attrValId) ? attrValId : 0))
                    ).ToList();
                    st.attributes.Remove("0");
                }

                // Filter by attributes (both VariantAttributes and ProductAttributes)
                if (st.attributes != null && st.attributes.Count > 0)
                {
                    foreach (var attr in st.attributes)
                    {
                        if (!int.TryParse(attr.Key, out var attributeId))
                            continue;

                        list = list.Where(variant =>
                            attr.Value.Any(value =>
                            {
                                // Check in VariantAttributes
                                bool matchInVariantAttr = variant.VariantAttributes.Any(va =>
                                    va.AttributeId == attributeId &&
                                    (
                                        (va.ValueText != null && va.ValueText.Trim().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)) ||
                                        (va.ValueInt != null && va.ValueInt.ToString() == value.Trim()) ||
                                        (va.ValueDecimal != null && va.ValueDecimal.ToString() == value.Trim()) ||
                                        (va.AttributeValueId != null && va.AttributeValueId == (int.TryParse(value.Trim(), out var attrValId) ? attrValId : 0))
                                    )
                                );

                                // Check in ProductAttributes
                                bool matchInProductAttr = variant.Product.ProductAttributes.Any(pa =>
                                    pa.AttributeId == attributeId &&
                                    (
                                        (pa.ValueText != null && pa.ValueText.Trim().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)) ||
                                        (pa.ValueInt != null && pa.ValueInt.ToString() == value.Trim()) ||
                                        (pa.ValueDecimal != null && pa.ValueDecimal.ToString() == value.Trim()) ||
                                        (pa.AttributeValueId != null && pa.AttributeValueId == (int.TryParse(value.Trim(), out var attrValId) ? attrValId : 0))
                                    )
                                );

                                // Return true if match found in either VariantAttributes or ProductAttributes
                                return matchInVariantAttr || matchInProductAttr;
                            })
                        ).ToList();
                    }
                }

                var totalCount = list.Count;
                var paginatedList = list.Skip(st.skip).Take(st.take).ToList();

                return new ProductVariantPaginated
                {
                    data = paginatedList.Select(x => new ProductVariantView
                    {
                        id = x.Id,
                        product_id = x.ProductId,
                        name = x.Name,
                        price = x.Price,
                        sku = x.Sku,
                        created_at = x.CreatedAt,
                        product = new ProductView
                        {
                            id = x.Product.Id,
                            name = x.Product.Name,
                            slug = x.Product.Slug,
                            description = x.Product.Description,
                            status = x.Product.Status,
                            category = new CategoryView
                            {
                                id = x.Product.Category.Id,
                                name = x.Product.Category.Name.Trim(),
                                slug = x.Product.Category.Slug.ToLower().Trim(),
                            },
                            created_at = x.Product.CreatedAt,
                            product_attribute = x.Product.ProductAttributes.Select(e => new ProductAttributeView
                            {
                                id = e.Id,
                                product_id = e.ProductId,
                                attribute_id = e.AttributeId,
                                value_decimal = e.ValueDecimal,
                                value_int = e.ValueInt,
                                value_text = e.ValueText,
                                attribute = new AttributeView
                                {
                                    id = e.Attribute.Id,
                                    name = e.Attribute.Name,
                                    slug = e.Attribute.Slug,
                                    data_type = e.Attribute.DataType,
                                    unit = e.Attribute.Unit,
                                    status = e.Attribute.Status,
                                }
                            }).ToHashSet()
                        },
                        product_images = x.ProductImages.Select(e => new ProductImageView
                        {
                            id = e.Id,
                            product_id = e.ProductId,
                            variant_id = e.VariantId,
                            url = e.Url,
                        }).ToHashSet(),
                        variant_attributes = x.VariantAttributes.Select(e => new VariantAttributeView
                        {
                            id = e.Id,
                            variant_id = e.VariantId,
                            attribute_id = e.AttributeId,
                            value_decimal = e.ValueDecimal,
                            value_int = e.ValueInt,
                            value_text = e.ValueText,
                            attribute_value_id = e.AttributeValueId,
                            attribute_value = e.AttributeValue != null ? new AttributeValueView
                            {
                                id = e.AttributeValue.Id,
                                attribute_id = e.AttributeValue.AttributeId,
                                value = e.AttributeValue.Value ?? string.Empty
                            } : null,
                            attribute = e.Attribute != null ? new AttributeView
                            {
                                id = e.Attribute.Id,
                                name = e.Attribute.Name,
                                slug = e.Attribute.Slug,
                                data_type = e.Attribute.DataType,
                                unit = e.Attribute.Unit,
                                status = e.Attribute.Status,
                            } : null
                        }).ToHashSet()
                    }).ToHashSet(),
                    count = totalCount,
                    max = maxPriceInDb,
                    min = 0
                };
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error: " + ex.Message, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(ProductVariantView entity)
        {
            try
            {
                var en = _context.ProductVariants.FirstOrDefault(e => e.Id == entity.id);
                if (en != null)
                {
                    en.Name = entity.name;
                    en.Price = entity.price;
                    en.Sku = entity.sku;
                    _context.ProductVariants.Update(en);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HashSet<ProductVariantView> FindByProductId(int id) {
            try {

                var en = _context.ProductVariants.Include(x => x.ProductImages).Include(c=>c.Product).ThenInclude(c=>c.Brand).Include(x => x.VariantAttributes).ThenInclude(c => c.AttributeValue).ThenInclude(c => c.Attribute).Include(c => c.Product).ThenInclude(c => c.ProductAttributes).ThenInclude(c => c.Attribute).Where(a=>a.ProductId==id).ToHashSet(); 

                return en.Select(e=>new ProductVariantView
                {
                    id= e.Id,
                    price= e.Price,
                    name= e.Name,
                    product_id= e.ProductId,
                    sku = e.Sku,
                    status= e.Status,
                    created_at= e.CreatedAt,
                    variant_attributes = e.VariantAttributes.Select(en=> new VariantAttributeView
                    {
                       id= en.Id,
                       attribute_id = en.AttributeId,
                       variant_id =en.VariantId,
                       value_decimal= en.ValueDecimal,
                       value_int= en.ValueInt,
                       value_text= en.ValueText,
                       attribute_value_id= en.AttributeValueId,
                        attribute_value =en.AttributeValue !=null?  new AttributeValueView
                        {
                            id = en.AttributeValue.Id,
                            attribute_id = en.AttributeValue.AttributeId,
                            value = en.AttributeValue.Value
                        }:null,

                        attribute =   en.Attribute !=null? new AttributeView
                        {
                            id = en.Attribute.Id,
                            name = en.Attribute.Name,
                            data_type = en.Attribute.DataType,
                            unit = en.Attribute.Unit,
                            slug = en.Attribute.Slug,
                            status = en.Attribute.Status,
                        }  :null

                    }).ToHashSet() ?? new HashSet<VariantAttributeView>(),


                }).ToHashSet();
            
            }
            catch (Exception e) {
                return null;
            }
        }

        public HashSet<ProductVariantView> GetAll()
        {
            try
            {
                var list = _context.ProductVariants.Include(x => x.ProductImages).Include(x => x.VariantAttributes).ThenInclude(c => c.Attribute).ToHashSet();

                return list.Select(x => new ProductVariantView
                {
                    id = x.Id,
                    product_id = x.ProductId,
                    name = x.Name,
                    price = x.Price,
                    sku = x.Sku,
                    product_images = x.ProductImages.Select(e => new ProductImageView
                    {
                        id = e.Id,
                        product_id = e.ProductId,
                        variant_id = e.VariantId,
                        url = e.Url,
                    }
                 ).ToHashSet(),
                    variant_attributes = x.VariantAttributes.Select(e => new VariantAttributeView
                    {
                        id = e.Id,
                        variant_id = e.VariantId,
                        attribute_id = e.AttributeId,
                        value_decimal = e.ValueDecimal,
                        value_int = e.ValueInt,
                        value_text = e.ValueText,
                        attribute = new AttributeView
                        {
                            id = e.Attribute.Id,
                            name = e.Attribute.Name,
                            slug = e.Attribute.Slug,
                            data_type = e.Attribute.DataType,
                            unit = e.Attribute.Unit,
                            status = e.Attribute.Status,
                        }

                    }).ToHashSet()


                }).ToHashSet();
            }
            catch (SqlException ex)
            {
                // xử lý riêng cho lỗi SQL
                throw new Exception("Database error: " + ex.Message, ex);
            }
            catch (Exception e)
            {
                // xử lý cho các lỗi khác
                throw;
            }

        }
    }
}
