namespace Blogoblog.DAL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTimeOffset Comment_Date { get; set; }
        public string? Content { get; set; }
        public int? User_Id { get; set; }
                
        public User User { get; set; }
        public int? Article_Id { get; set; }
        public Article Article { get; set; }
    }
}