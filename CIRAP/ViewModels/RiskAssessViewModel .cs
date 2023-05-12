using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CIRAP.Models;

namespace CIRAP.ViewModels
{
    public class RiskAssessViewModel
    {
        public int? idx { get; set; }
        public int? TaskID { get; set; }
        public string? Task1Step { get; set; }
        public string? Task2Step { get; set; }
        public string? TaskContent { get; set; }
        public string? SubProjectID { get; set; }

        public string? SubProjectName { get; set; }

        public string? PotentialHazard { get; set; }
        public string? PossibleSituation { get; set; }
        public int? RA_Possibility { get; set; }//可能性
        public int? RA_Severity { get; set; }//嚴重性
        public int? RA_RiskValue { get; set; }//風險值
        public int? RA_Level { get; set; }//風險等級

        public string? RiskCM { get; set; }
        public string? CMPersion { get; set; }
        public static explicit operator RiskAssessViewModel(List<ProjectInfo> v)
        {
            throw new NotImplementedException();
        }
    }
}
