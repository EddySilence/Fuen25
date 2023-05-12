using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CIRAP.ViewModels
{
	public class RegisterVierwModel
	{
        public int? Id { get; set; }
        [Required]
        [DisplayName("帳號")]
        public string? Account { get; set; }
        [Required]
        [DisplayName("密碼")]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("信箱")]
        public string? Email { get; set; }
    }
}
