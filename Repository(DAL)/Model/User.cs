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

        public string? Cart { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public DateTime LastSeen { get; set; } = DateTime.Now;
    }
}
