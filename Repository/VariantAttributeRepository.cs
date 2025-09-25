using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class VariantAttributeRepository : IRepository<VariantAttributeView>
    {

        private readonly CatalogAPIContext _context;

        public VariantAttributeRepository(CatalogAPIContext context)
        {
            _context = context;
        }
        public bool Create(VariantAttributeView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VariantAttributeView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<VariantAttributeView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<VariantAttributeView> GetAll()
        {

            try
            {
                return _context.VariantAttributes.Select(x => new VariantAttributeView {
                
                    id = x.Id,
                    attribute_id = x.AttributeId,
                    variant_id = x.VariantId,
                    value_decimal = x.ValueDecimal,
                    value_int = x.ValueInt,
                    value_text = x.ValueText,
                    

                }).ToHashSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Update(VariantAttributeView entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFromList(List<VariantAttributeView> list)
        {
            try {
            var updateList = list.Select(x => new VariantAttribute {
               Id =x.id,
               AttributeId= x.attribute_id,
               VariantId= x.variant_id,
               ValueInt= x.value_int,
               ValueText= x.value_text,
               ValueDecimal= x.value_decimal,

            }).ToList();
                _context.VariantAttributes.UpdateRange(updateList);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) { 
             throw ex;
            }
        }
    }
}
