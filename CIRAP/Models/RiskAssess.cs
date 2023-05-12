using System.ComponentModel.DataAnnotations;

namespace CIRAP.Models
{
    public class RiskAssess
    {
        [Key]
        public int? idx { get; set; }
        public int? TaskID { get; set; }
        public string? PotentialHazard { get; set; }//淺在危害
        public string? PossibleSituation { get; set; }//可能狀況
        public int? RA_Possibility { get; set; }//可能性
        public int? RA_Severity { get; set; }//可能性
        public int? RA_RiskValue { get; set; }//風險值
        public int? RA_Level { get; set; }//風險等級

        public string? RiskCM { get; set; } //風險對策
        public string? CM_Persion { get; set; }//負責人員
        public DateTime? CreatedDate { get; set; }
    }
}

