using FinalWebProject.Data;
using FinalWebProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalWebProject.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly LabContext _dbContext;
    public List<PointS> Points { get; set; }
    public List<Student> Students { get; set; }

    public List<Subject> Subjects { get; set; }
    public IndexModel(ILogger<IndexModel> logger, LabContext db)
    {
        _logger = logger;
        _dbContext = db;
    }

    public IActionResult OnGet()
    {
        Points = _dbContext.Points.ToList();
        Students = _dbContext.Students.ToList();
        Subjects = _dbContext.Subjects.ToList();
        return Page();
    }

    public IActionResult OnPostCreatePoint()
    {
        return RedirectToPage("Points");
    }

    public IActionResult OnPostDeletePoint(int id)
    {
        return RedirectToPage("Points");
    }
}