using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class ProductImageRepository : IRepository<ProductImageView>
    {
        private readonly CatalogAPIContext _context;

        public ProductImageRepository(CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(ProductImageView entity)
        {

            try
            {
                var en = new ProductImage
                {
                    Id = entity.id,
                    ProductId = entity.product_id,
                    VariantId = entity.variant_id,
                    Url = entity.url,

                };
                _context.ProductImages.Add(en);
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
                var en = _context.ProductImages.FirstOrDefault(x => x.Id == id);
                if (en != null)
                {
                    _context.ProductImages.Remove(en);
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

        public ProductImageView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductImageView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductImageView> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductImageView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<ProductImageView> GetByVariantId(int variant_id)
        {
            try
            {
                var list = _context.ProductImages.Where(e=>e.VariantId == variant_id).ToList();
                return list.Select(e=>new ProductImageView
                {
                    id = e.Id,
                    product_id = e.ProductId,
                    variant_id = variant_id,
                    url = e.Url,
                }).ToHashSet();
            }
            catch (Exception ex) { 
             throw new NotImplementedException();
            }
        }
    }
}
