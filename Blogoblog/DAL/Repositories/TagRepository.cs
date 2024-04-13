using Blogoblog.DAL.DB;
using Blogoblog.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Blogoblog.DAL.Repositories
{
    public class TagRepository : IRepository<Tag>
    {
        protected DbContext _db;

        public DbSet<Tag> Set { get; private set; }

        public TagRepository(BlogoblogContext db)
        {
            _db = db;
            var set = _db.Set<Tag>();
            set.Load();
            Set = set;
        }

        public async Task Add(Tag item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Tag> Get(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await Set.ToListAsync();
        }

        public async Task Delete(Tag item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Tag item)
        {
            var existingItem = await Set.FindAsync(GetKeyValue(item));

            if (existingItem != null)
            {
                _db.Entry(existingItem).CurrentValues.SetValues(item);
                await _db.SaveChangesAsync();
            }
        }

        private object GetKeyValue(Tag item)
        {
            var key = _db.Model.FindEntityType(typeof(Tag)).FindPrimaryKey().Properties.FirstOrDefault();
            return item.GetType().GetProperty(key.Name).GetValue(item);
        }
    }
}