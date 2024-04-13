namespace Blogoblog.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {              
        Task Add(T item);
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();        
        Task Update(T item);
        Task Delete(T item);
    }
}