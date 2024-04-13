using System.ComponentModel.DataAnnotations.Schema;


namespace Blogoblog.DAL.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Content { get; set; }

        [ForeignKey("Tag_Id")]
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}