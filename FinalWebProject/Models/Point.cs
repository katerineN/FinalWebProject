using System.ComponentModel.DataAnnotations;

namespace FinalWebProject.Models;

public class PointS
{
    [Key]
    public int? Id { get; set; }
    public int? student_id { get; set; }
    public int? subject_id { get; set; }
    public int? value { get; set; }
    
    public virtual Student? Student { get; set; }
    public virtual Subject? Subject { get; set; }
}