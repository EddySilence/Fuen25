using System.ComponentModel.DataAnnotations;

namespace CIRAP.Models
{
    public class ProjectInfo
    {
        public string? UserID { get; set; }
        [Key]
        public string? Project_ID { get; set; }
        public string? Project_Name { get; set; }
        public string? Location { get; set; }
        public string? Organizer_Principal { get; set; }
        public string? Organizer_Addr { get; set; }
        public string? Organizer_PhoneNo { get; set; }
        public string? Organizer_Email { get; set; }
        public string? PCM_Principal { get; set; }
        public string? PCM_Addr { get; set; }
        public string? PCM_PhoneNo { get; set; }
        public string? PCM_Email { get; set; }
        public string? Designer_Principal { get; set; }
        public string? Designer_Addr { get; set; }
        public string? Designer_PhoneNo { get; set; }
        public string? Designer_Email { get; set; }
        public string? Supervision_Principal { get; set; }
        public string? Supervision_Addr { get; set; }
        public string? Supervision_PhoneNo { get; set; }
        public string? Supervision_Email { get; set; }
        public string? Construction_Principal { get; set; }
        public string? Construction_Addr { get; set; }
        public string? Construction_PhoneNo { get; set; }
        public string? Construction_Email { get; set; }
        public string? Structure { get; set; }
        public string? ConstructionCost { get; set; }
        public string? UseFor { get; set; }
        public string? Notes { get; set; }
        public string? Environment { get; set; }
        public string? Stage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
