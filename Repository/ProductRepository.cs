using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;
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
                var en = _context.Products.Include(c => c.Category).Include(c => c.Brand).FirstOrDefault(c=>c.Id==id);

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
                        level = en.Category.Level

                    }
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

        public bool Update(ProductView entity)
        {
            throw new NotImplementedException();
        }
    }
}
