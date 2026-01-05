using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class CategoryBrandRepository : IRepository<CategoryBrandView>
    {
        private readonly CatalogAPIContext _context;

        public CategoryBrandRepository(CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(CategoryBrandView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CategoryBrandView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryBrandView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryBrandView> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(CategoryBrandView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<CategoryBrandView> GetByCategory(string category)
        {
            try
            {
                var list = _context.CategoryBrands.Include(c=>c.Category).Include(c=>c.Brand).Where(c=>c.Category.Slug == category);
                return list.Select(a=>new CategoryBrandView
                {
                    id = a.Id,
                    category_id = a.CategoryId,
                    brand_id = a.BrandId,
                    category = new CategoryView
                    {
                        id = a.CategoryId,
                        name = a.Category.Name.Trim(),
                        slug = a.Category.Slug.Trim(),

                    },
                    brand = new BrandView
                    {
                        id = a.BrandId,
                        name = a.Brand.Name.Trim(), slug = a.Brand.Slug.Trim(),

                    }
                }).ToHashSet();

            }
            catch (Exception e) {
                throw e;
            }

        }
    }
}
