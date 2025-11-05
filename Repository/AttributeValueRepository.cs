using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class AttributeValueRepository : IRepository<AttributeValueView>
    {
        private readonly CatalogAPIContext _context;
        public AttributeValueRepository(CatalogAPIContext context)
        {
            _context = context;
        }

        public bool Create(AttributeValueView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AttributeValueView FindById(int id)
        {

            try
            {

                var attributeValueEntity = _context.AttributeValues.Find(id);
                if (attributeValueEntity == null)
                {
                    return null!;
                }
                var attributeValueView = new AttributeValueView
                {
                    id = attributeValueEntity.Id,
                    attribute_id = attributeValueEntity.AttributeId,
                    value = attributeValueEntity.Value
                };
                return attributeValueView;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"An error occurred while finding AttributeValue by ID: {ex.Message}");
                return null!;
            }
        }

        public HashSet<AttributeValueView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<AttributeValueView> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(AttributeValueView entity)
        {
            throw new NotImplementedException();
        }
        public HashSet<AttributeValueView> GetByAttributeId(int attributeId)
        {
            var result = new HashSet<AttributeValueView>();
            try
            {
                var attributeValues = _context.AttributeValues
                    .Where(av => av.AttributeId == attributeId)
                    .ToList();
                foreach (var attributeValueEntity in attributeValues)
                {
                    var attributeValueView = new AttributeValueView
                    {
                        id = attributeValueEntity.Id,
                        attribute_id = attributeValueEntity.AttributeId,
                        value = attributeValueEntity.Value.Trim()
                    };
                    result.Add(attributeValueView);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"An error occurred while retrieving AttributeValues by Attribute ID: {ex.Message}");
            }
            return result;
        }
    }
}
