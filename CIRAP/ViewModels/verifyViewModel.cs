using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CIRAP.ViewModels
{
    public class verifyViewModel
    {
        public int? Id { get; set; }
        [Required]
        [DisplayName("驗證碼")]
        public string? ValidateCode { get; set; }
    }
}
