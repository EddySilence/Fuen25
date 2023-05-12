using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CIRAP.Models;

namespace CIRAP.ViewModels
{
    public class ProjectInfoViewModel
    {
        public string? Project_ID { get; set; }
        public string? UserID { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [DisplayName("工程名稱")]
        
        public string? Project_Name { get; set; }

        [Required]
        [DisplayName("基地位置")]
        public string? Location { get; set; }
        [Required]
        public string? Organizer_Principal { get; set; }
        [Required]
        public string? Organizer_Addr { get; set; }
        [Required]
        public string? Organizer_PhoneNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "{0}只允許輸入Email格式")]
        public string? Organizer_Email { get; set; }
        [Required]
        public string? PCM_Principal { get; set; }
        [Required]
        public string? PCM_Addr { get; set; }
        [Required]
        public string? PCM_PhoneNo { get; set; }
        [Required]
        public string? PCM_Email { get; set; }

        [Required]
        public string? Designer_Principal { get; set; }
        [Required]
        public string? Designer_Addr { get; set; }
        [Required]
        public string? Designer_PhoneNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "{0}只允許輸入Email格式")]
        public string? Designer_Email { get; set; }
        [Required]
        public string? Supervision_Principal { get; set; }
        [Required]
        public string? Supervision_Addr { get; set; }
        [Required]
        public string? Supervision_PhoneNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "{0}只允許輸入Email格式")]
        public string? Supervision_Email { get; set; }
        [Required]
        public string? Construction_Principal { get; set; }
        [Required]
        public string? Construction_Addr { get; set; }
        [Required]
        public string? Construction_PhoneNo { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "{0}只允許輸入Email格式")]
        public string? Construction_Email { get; set; }

        public string? Structure { get; set; }
        public string? ConstructionCost { get; set; }
        public string? UseFor { get; set; }
        public string? Notes { get; set; }

        public static explicit operator ProjectInfoViewModel(List<ProjectInfo> v)
        {
            throw new NotImplementedException();
        }
    }
}
