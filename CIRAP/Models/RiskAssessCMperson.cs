using System.ComponentModel.DataAnnotations;

namespace CIRAP.Models
{
    public class RiskAssessCMperson
    {
        [Key]
        public int? id { get; set; }
        public string? Project_ID { get; set; }
        public string? CM_Name { get; set; }

    }
}

