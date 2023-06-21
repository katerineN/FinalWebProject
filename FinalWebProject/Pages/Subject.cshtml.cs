using FinalWebProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalWebProject.Pages;

public class SubjectModel : PageModel
{
    private readonly ILogger<SubjectModel> _logger;
    private readonly LabContext _dbContext;
    public List<Models.Subject> Subjects { get; set; }

    public SubjectModel(ILogger<SubjectModel> logger, LabContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    [Authorize]
    public IActionResult OnGet()
    {
        Subjects = _dbContext.Subjects.ToList();
        return Page();
    }
    [Authorize]
    public IActionResult OnPostCreateSubject()
    {
        return RedirectToPage("Subjects");
    }
    [Authorize]
    public IActionResult OnPostDeleteSubject(int id)
    {
        return RedirectToPage("Subjects");
    }
}