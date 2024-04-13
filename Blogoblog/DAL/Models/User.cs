using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Blogoblog.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", Prompt = "Ваш логин")]
        [StringLength(50)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Хотя бы 3 символа, пожалуйста", MinimumLength = 3)]
        public string? Password { get; set; }

        public int Role_Id { get; set; } = 3;

        [ForeignKey("User_Id")]
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}