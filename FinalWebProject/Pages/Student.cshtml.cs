using FinalWebProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalWebProject.Pages;

public class StudentModel : PageModel
{
    private readonly ILogger<StudentModel> _logger;
    private readonly LabContext _dbContext;
    public List<Models.Student> Students { get; set; }

    public StudentModel(ILogger<StudentModel> logger, LabContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public IActionResult OnGet()
    {
        Students = _dbContext.Students.ToList();
        return Page();
    }

    public IActionResult OnPostCreateStudent()
    {
        return RedirectToPage("Students");
    }

    public IActionResult OnPostDeleteStudent(int id)
    {
        return RedirectToPage("Students");
    }
    
}