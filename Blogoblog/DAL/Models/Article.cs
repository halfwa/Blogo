using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Blogoblog.DAL.Models
{
    public class Article
    {
        public int Id { get; set; }
        public DateTimeOffset Article_Date { get; set; }

        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название статьи")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Поле Содержание обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(1000, ErrorMessage = "Статья должна иметь наполнение...", MinimumLength = 3)]
        [Display(Name = "Текст", Prompt = "Введите текст статьи")]
        public string? Content { get; set; }

        public int User_Id { get; set; }
        public User User { get; set; }

        [ForeignKey("Article_Id")]
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}