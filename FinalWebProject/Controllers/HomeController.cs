using System.Diagnostics;
using FinalWebProject.Data;
using FinalWebProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalWebProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly LabContext db;

    public HomeController(ILogger<HomeController> logger, LabContext _db)
    {
        _logger = logger;
        db = _db;
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(Login));
        }

        var students = db.Students.ToList();
        var subjects = db.Subjects.ToList();
        var points = db.Points.ToList();
        ViewBag.Students = students;
        ViewBag.Subjects = subjects;
        return View(points);
    }

    public IActionResult Student()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(Login));
        }

        var students = db.Students.ToList();
        return View(students);
    }

    public IActionResult Subject()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(Login));
        }

        var subjects = db.Subjects.ToList();
        return View(subjects);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPointS(int studentId, int subjectId, int value)
    {
        if (ModelState.IsValid)
        {
            PointS newPoint = new PointS();
            newPoint.Id = db.Points.Max(x => x.Id) + 1;
            newPoint.student_id = studentId;
            newPoint.subject_id = subjectId;
            newPoint.value = value;

            db.Points.Add(newPoint);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<IActionResult> DeletePointS(int Id)
    {
        if (ModelState.IsValid)
        {
            var points = db.Points.ToList();
            if (points.Any(s => s.Id == Id))
            {
                PointS point = points.Find(s => s.Id == Id);
                db.Points.Remove(point);
                db.SaveChanges();
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStudent(string ln, string fn, int c, int g)
    {
        if (ModelState.IsValid)
        {
            Student newStudent = new Student();
            newStudent.Id = db.Students.Max(x => x.Id) + 1;
            newStudent.first_name = fn;
            newStudent.last_name = ln;
            newStudent.course = c;
            newStudent.group = g;
            db.Students.Add(newStudent);
            db.SaveChanges();
        }

        return RedirectToAction(nameof(Student));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (ModelState.IsValid)
        {
            var students = db.Students.ToList();
            var points = db.Points.ToList();
            if (students.Any(s => s.Id == id))
            {
                points.ForEach(o =>
                {
                    if (o.student_id == id)
                    {
                        db.Points.Remove(o);
                    }
                });
                Student student = students.Find(s => s.Id == id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
        }

        return RedirectToAction(nameof(Student));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubject(string name, string teacher)
    {
        if (ModelState.IsValid)
        {
            Subject newSubject = new Subject();
            newSubject.Id = db.Subjects.Max(x => x.Id) + 1;
            newSubject.name = name;
            newSubject.teacher_name = teacher;
            db.Subjects.Add(newSubject);
            db.SaveChanges();
        }
        return RedirectToAction(nameof(Subject));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        if (ModelState.IsValid)
        {
            var subjects = db.Subjects.ToList();
            var points = db.Points.ToList();
            if (subjects.Any(s => s.Id == id))
            {
                points.ForEach(p =>
                {
                    if (p.subject_id == id)
                    {
                        db.Points.Remove(p);
                    }
                });
                Subject subject = subjects.Find(s => s.Id == id);
                db.Subjects.Remove(subject);
                db.SaveChanges();
            }
        }

        return RedirectToAction(nameof(Subject));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}