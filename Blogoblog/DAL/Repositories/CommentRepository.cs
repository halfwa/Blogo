using Blogoblog.DAL.DB;
using Blogoblog.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Blogoblog.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        protected DbContext _db;

        public DbSet<Comment> Set { get; private set; }

        public CommentRepository(BlogoblogContext db)
        {
            _db = db;
            var set = _db.Set<Comment>();
            set.Load();
            Set = set;
        }

        public async Task Add(Comment item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Comment> Get(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await Set.ToListAsync();
        }

        public async Task Delete(Comment item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Comment item)
        {
            var existingItem = await Set.FindAsync(GetKeyValue(item));

            if (existingItem != null)
            {
                _db.Entry(existingItem).CurrentValues.SetValues(item);
                await _db.SaveChangesAsync();
            }
        }

        private object GetKeyValue(Comment item)
        {
            var key = _db.Model.FindEntityType(typeof(Comment)).FindPrimaryKey().Properties.FirstOrDefault();
            return item.GetType().GetProperty(key.Name).GetValue(item);
        }
    }
}