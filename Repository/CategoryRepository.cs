using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class CategoryRepository : IRepository<CategoryView>
    {
        private readonly CatalogAPIContext _context;

        public CategoryRepository(CatalogAPIContext context)
        {
            _context = context;
        }
 
        public bool Create(CategoryView entity)
        {
            try
            {
                var category = new Category
                {
                    Name= entity.name,
                    Slug = entity.slug,
                    ParentId= entity.parent_id,
                    Path= entity.path,
                    Level= entity.level!=0?entity.level:0,
                    CreatedAt= DateTime.Now,
                };
                _context.Categories.Add(category);
                _context.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public CategoryView CreateAndReturn(CategoryView entity)
        {
            try
            {
                var category = new Category
                {
                    Name = entity.name,
                    Slug = entity.slug,
                    ParentId = entity.parent_id,
                    Path = entity.path,
                    Level = entity.level != 0 ? entity.level : 0,
                    CreatedAt = DateTime.Now,
                };

                _context.Categories.Add(category);
                _context.SaveChanges();

                return new CategoryView
                {
                    id = category.Id,
                    name = category.Name,
                    slug = category.Slug,
                    parent_id =(int) category.ParentId,
                    path = category.Path,
                    level = category.Level
                };
            }
            catch
            {
                return null;
            }
        }



        public bool Delete(int id)
        {
            try
            {
                var entity = _context.Categories.SingleOrDefault(c => c.Id == id);

                if (entity == null)
                {
                    return false; // Nothing to delete
                }

                _context.Categories.Remove(entity);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public CategoryView FindById(int id)
        {

            try
            {
                var category = _context.Categories.SingleOrDefault(
                    c => c.Id == id);
                if (category == null)
                {
                    return null;
                }
                return new CategoryView
                {
                    id = category.Id,
                    name = category.Name,
                    slug = category.Slug,
                    parent_id = (int)category.ParentId,
                    path = category.Path,
                    level = category.Level
                };

            }
            catch
            {
                return null ;
            }
        }
        public Category GetEntityById(int id)
        {
            return _context.Categories.SingleOrDefault(c => c.Id == id);
        }
        public HashSet<CategoryView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<CategoryView> GetAll()
        {
            try {
                var cate = _context.Categories.Select(c => new CategoryView
                {
                    id = c.Id,
                    name = c.Name,
                    slug = c.Slug,
                    parent_id = c.ParentId ?? 0,
                    level = c.Level,
                    path = c.Path,


                }).ToHashSet();
                return cate;
                 }
            catch (Exception ex)
            {
                return new HashSet<CategoryView>();
              
            }


}

        public bool Update(Category entity)
        {
            try
            {
                _context.Categories.Update(entity); // entity đã được tracking
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(CategoryView entity)
        {
            throw new NotImplementedException();
        }
    }
}
