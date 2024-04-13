using System.ComponentModel.DataAnnotations.Schema;


namespace Blogoblog.DAL.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Role_Name { get; set; }
        public string Role_Description { get; set; } = "";

        [ForeignKey("Role_Id")]
        public List<User> Users { get; set; } = new List<User>();
    }
}