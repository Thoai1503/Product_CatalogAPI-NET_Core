using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }

        public HashSet<ProductView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<ProductView> GetAll()
        {


            return _context.Products
             
               .Select(en => new ProductView
               {
                   id = en.Id,
                   name = en.Name,
                   description = en.Description,
                   category_id = en.CategoryId,
                   brand_id =(int) en.BrandId,
                   rating = en.Rating,
                   status = en.Status,
                 
               })
               .ToHashSet();
        }

        public bool Update(ProductView entity)
        {
            throw new NotImplementedException();
        }
    }
}
