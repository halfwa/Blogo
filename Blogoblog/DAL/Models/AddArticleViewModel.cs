using System.ComponentModel.DataAnnotations;


namespace Blogoblog.DAL.Models
{
    public class AddArticleViewModel
    {
        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название статьи")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Поле Содержание обязательно для заполнения")]
        [DataType(DataType.Text)]
        [StringLength(1000, ErrorMessage = "Статья должна иметь наполнение...", MinimumLength = 3)]
        [Display(Name = "Текст", Prompt = "Введите текст статьи")]
        public string? Content { get; set; }

        public IList<Tag>? Tags { get; set; }
    }
}