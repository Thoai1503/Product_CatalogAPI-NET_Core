using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository.RepoInterface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Repository
{
    public class BrandRepository : IRepository<BrandView>
    {
        private readonly CatalogAPIContext _catalogAPIContext;

        public BrandRepository(CatalogAPIContext catalogAPIContext)
        {
            _catalogAPIContext = catalogAPIContext;
        }

        public bool Create(BrandView entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public BrandView FindById(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<BrandView> FindByKeywork(string keywork)
        {
            throw new NotImplementedException();
        }

        public HashSet<BrandView> GetAll()
        {
            try
            {

                var list = _catalogAPIContext.Brands.Select(e=> new BrandView
                {
                    id = e.Id,
                    name = e.Name,
                    slug = e.Slug,
                    status = e.Status,

                }).ToHashSet();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update(BrandView entity)
        {
            throw new NotImplementedException();
        }
    }
}
