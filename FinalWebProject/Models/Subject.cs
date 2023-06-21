using System.ComponentModel.DataAnnotations;

namespace FinalWebProject.Models;

public class Subject
{
    [Key]
    public int? Id { get; set; }
    public string? name { get; set; }
    public string? teacher_name { get; set; }
    
    public virtual ICollection<PointS> Points { get; set; } = new List<PointS>();

}