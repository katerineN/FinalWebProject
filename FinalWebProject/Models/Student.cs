using System.ComponentModel.DataAnnotations;

namespace FinalWebProject.Models;

public class Student
{
    [Key]
    public int? Id { get; set; }
    public string? last_name { get; set; }
    public string? first_name { get; set; }
    public int? course { get; set; }
    public int? group { get; set; }
    
    public virtual ICollection<PointS> Points { get; set; } = new List<PointS>();
}