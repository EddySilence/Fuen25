using System.ComponentModel.DataAnnotations;

namespace CIRAP.Models
{
    public class ProjectTasks
    {
        [Key]
        public int? id { get; set; }
        public string? Project_ID { get; set; }
        public string? SubProjectID { get; set; }
        public string? SubProjectName { get; set; }
        public string? Task1Step { get; set; }
        public string? Task2Step { get; set; }
        public string? TaskContent { get; set; }
        public string? Stage { get; set; }
    }
}

