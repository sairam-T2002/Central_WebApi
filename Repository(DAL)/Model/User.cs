using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Usr_Nam { get; set; }
        public string Pwd { get; set; }
        public string E_Mail { get; set; }
        public string? RefreshToken { get; set; }
    }
}
