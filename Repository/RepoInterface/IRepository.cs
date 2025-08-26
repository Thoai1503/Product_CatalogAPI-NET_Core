namespace CatalogServiceAPI_Electric_Store.Repository.RepoInterface
{
    internal interface IRepository<T> where T : class
    {
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
        HashSet<T> GetAll();
        T FindById(int id);
        HashSet<T> FindByKeywork(string keywork);
    }
}
