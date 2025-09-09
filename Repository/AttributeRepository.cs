using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using CatalogServiceAPI_Electric_Store.Models.ModelView;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class AttributeRepository : IRepository<AttributeView>
    {
        private readonly CatalogAPIContext _context;
        public AttributeRepository(CatalogAPIContext context)
        {
            _context = context;
        }

        public bool Create(AttributeView entity)
        {

            
            try
            {
                var attribute = new Models.Entities.Attribute
                {
                    Name = entity.name,
                    Slug = entity.slug,
                    DataType = entity.data_type,
                    Unit = entity.unit,
                    Status = entity.status,
                  
                };
                _context.Attributes.Add(attribute);
                _context.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AttributeView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<AttributeView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<AttributeView> GetAll()
        {
            try { 
            
                var attributes = _context.Attributes.Select(a => new AttributeView
                {
                    id = a.Id,
                    name = a.Name,
                    slug = a.Slug,
                    data_type = a.DataType,
                    unit = a.Unit,
                    status = a.Status
                }).ToHashSet();
                return attributes;
            }
            catch (Exception ex) { 
            
                throw ex;

            }
        }

        public bool Update(AttributeView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<AttributeView> GetByCategoryId(int categoryId)
        {
            try
            {
                var attrs = _context.Attributes.Select(a => new AttributeView
                {
                    id = a.Id,
                    name = a.Name,
                    slug = a.Slug,
                    data_type = a.DataType,
                    unit = a.Unit,
                    status = a.Status,
                    is_selected = _context.CategoryAttributes.Any(ca => ca.CategoryId == categoryId && ca.AttributeId == a.Id) ? 1 : 0


                }).ToHashSet();
     
                return attrs;
            }
            catch (Exception ex)
            {
                return new HashSet<AttributeView>();
            }
        }
        }
}
