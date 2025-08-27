using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class CategoryRepository : IRepository<CategoryView>
    {
        private static CategoryRepository _instance;
            private readonly CatalogAPIContext _context;

            private CategoryRepository()
        {
            _context = new CatalogAPIContext();
        }
        public static CategoryRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoryRepository();
                }
                return _instance;
            }
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

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CategoryView FindById(int id)
        {
            throw new NotImplementedException();
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

        public bool Update(CategoryView entity)
        {
            throw new NotImplementedException();
        }
    }
}
