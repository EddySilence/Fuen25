using System.ComponentModel.DataAnnotations;

namespace CIRAP.Models
{
    public class User
    {
        [Key]
        public int? id { get; set; }
        public string? UserID { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? ValidateCode { get; set; }

    }
}
