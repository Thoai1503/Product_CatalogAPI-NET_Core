using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }

        public HashSet<ProductVariantView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductVariantView> GetAll()
        {
            try {
                var list = _context.ProductVariants.Include(x=>x.ProductImages).Include(x=>x.VariantAttributes).ThenInclude(c=>c.Attribute).ToHashSet();

                return list.Select(x => new ProductVariantView {
                  id = x.Id,
                  product_id = x.ProductId,
                  name = x.Name,
                  price = x.Price,
                  sku = x.Sku,
                  product_images = x.ProductImages.Select(e =>new ProductImageView
                  {
                      id = e.Id,
                      product_id = e.ProductId,
                      variant_id = e.VariantId,
                      url = e.Url,
                  }
                 ).ToHashSet(),
                  variant_attributes = x.VariantAttributes.Select(e=> new VariantAttributeView
                  {
                      id=e.Id,
                      variant_id=e.VariantId,
                      attribute_id =e .AttributeId,
                      value_decimal =e .ValueDecimal,
                      value_int =e .ValueInt,
                      value_text =e .ValueText,
                      attribute = new AttributeView { 
                        id=e.Attribute.Id,
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

                var en = _context.ProductVariants.Include(e=>e.VariantAttributes).ThenInclude(c=>c.Attribute).Where(a=>a.ProductId==id).ToHashSet(); 

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
                       attribute = new AttributeView
                       {
                           id =en.Attribute.Id,
                           name=en.Attribute.Name,
                           data_type=en.Attribute.DataType,
                           unit=en.Attribute.Unit,
                           slug=en.Attribute.Slug,
                           status=en.Attribute.Status,
                       }

                    }).ToHashSet() ?? new HashSet<VariantAttributeView>(),


                }).ToHashSet();
            
            }
            catch (Exception e) {
                return null;
            }
        } 
    }
}
