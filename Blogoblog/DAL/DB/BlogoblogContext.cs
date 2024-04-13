using Blogoblog.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Blogoblog.DAL.DB
{
    public class BlogoblogContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogoblogContext(DbContextOptions<BlogoblogContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Article>().ToTable("Articles");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Comment>().ToTable("Comments");

            //Распределите возможности администратора, модератора и пользователя в системе.
            //Например, администратор может удалить любого пользователя, а пользователь может редактировать только свои статьи.
            //У администратора есть полные права на любые операции с бизнес-моделями, у модератора — только права на операции редактирования статей
            //и комментариев, у пользователя — только права на операции со своими созданными бизнес-моделями(статья, комментарий, тег).
            //Выполнить это можно при помощи клаймов.

            //modelBuilder.Entity<User>().ToTable("Users_Roles") // Устанавливаем нужное имя для промежуточной таблицы
            //    .HasMany(u => u.Roles)
            //    .WithMany(r => r.Users);


            // Другие настройки моделей

            //modelBuilder.Entity<Comment>()
            //    .HasOne(a => a.User)
            //    .WithMany(b => b.Comments)
            //    .HasForeignKey(c => c.User_Id)
            //    .HasPrincipalKey(d => d.Id)
            //    .IsRequired(false);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        ////    //optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
        ////    //optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-891I20FV\SQLEXPRESS;Database=BLOGOBLOG;Trusted_Connection=True;Trust Server Certificate=True;");
        ////    //optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Database=BLOGOBLOG;Trusted_Connection=True;Trust Server Certificate=True;");
        //    optionsBuilder.UseSqlite(@"Data Source=BLOGOBLOG.db");
        //}
    }
}
