using Blogoblog.DAL.DB;
using Blogoblog.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Blogoblog.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(BlogoblogContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();
            Set = set;
        }

        public async Task Add(T item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await Set.FindAsync(id);
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            return await Set.ToListAsync();
        }

        public async Task Delete(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            var existingItem = await Set.FindAsync(GetKeyValue(item));

            if (existingItem != null)
            {
                _db.Entry(existingItem).CurrentValues.SetValues(item);
                await _db.SaveChangesAsync();
            }
        }

        private object GetKeyValue(T item)
        {
            var key = _db.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            return item.GetType().GetProperty(key.Name).GetValue(item);
        }

        //public async Task Update(T item, T newItem)
        //{
        //    item = newItem;
        //    Set.Update(item);
        //    await _db.SaveChangesAsync();
        //}
    }
}