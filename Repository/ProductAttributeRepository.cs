using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;


namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class ProductAttributeRepository : IRepository<ProductAttribute>
    {
        private readonly CatalogAPIContext _context;

       // protected DbSet<ProductAttribute> DbSet => _context.Set<ProductAttribute>();
        public ProductAttributeRepository (CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(ProductAttribute entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ProductAttribute FindById(int id)
        {
            try {
                var en = _context.ProductAttributes.FirstOrDefault(p => p.Id == id);
                if (en != null)
                {
                    return new ProductAttribute
                    {
                        Id = en.Id,
                        AttributeId = en.AttributeId,
                        ProductId = en.ProductId,
                        ValueDecimal = en.ValueDecimal,
                        ValueInt = en.ValueInt,
                        ValueText = en.ValueText,
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public HashSet<ProductAttribute> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductAttribute> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductAttribute entity)
        {


            try
            {
                var en = _context.ProductAttributes.FirstOrDefault(e=>e.Id == entity.Id);
                if (en != null)
                {
                    en.AttributeId = entity.AttributeId;
                    en.ProductId = entity.ProductId;
                    en.AttributeValueId = entity.AttributeValueId;
                    en.ValueInt = entity.ValueInt;
                    en.ValueText = entity.ValueText;
                    en.ValueDecimal = entity.ValueDecimal;
                    _context.ProductAttributes.Update(en);
                    _context.SaveChanges();
                    return true;
                }


                return false;
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
