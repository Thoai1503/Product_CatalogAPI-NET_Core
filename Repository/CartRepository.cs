using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class CartRepository : IRepository<CartView>
    {
        private CatalogAPIContext _context;

        public CartRepository(CatalogAPIContext context)
        {
            _context = context;
        }

        public bool Create(CartView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CartView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<CartView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<CartView> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(CartView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<CartView> FindByUserId(int userId)
        {
            try
            {
                var list = _context.Carts.Include(c => c.Variant).ThenInclude(e => e.ProductImages).AsQueryable();

                return list.Where(e => e.UserId == userId).Select(c => new CartView
                {
                        id = c.Id,
                        user_id = (int) c.UserId,
                        variant_id = (int)c.VariantId,
                        quantity = (int)c.Quantity,
                        variant = new ProductVariantView
                        {
                            id = c.Variant.Id,
                            name =c.Variant.Name,
                            price = c.Variant.Price,
                            product_images = c.Variant.ProductImages.Select(e=>new ProductImageView
                            {
                                id =  e.Id,
                                product_id = e.ProductId,
                                variant_id=e.Variant.Id,
                                url = e.Url,
                            }).ToHashSet(),
                        }
                }).ToHashSet();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
