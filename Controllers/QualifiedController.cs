using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AlDawarat_W_AlEngazat.Controllers
{
    public class QualifiedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QualifiedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string department, string category, string position)
        {
            var employees = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(department))
            {
                employees = employees.Where(e => e.Department.Contains(department));
                ViewBag.DepartmentQuery = department;
                ViewBag.DepartmentSuggestions = _context.Employees
                    .Where(e => e.Department.Contains(department))
                    .Select(e => e.Department)
                    .Distinct()
                    .ToList();
            }

            if (!string.IsNullOrEmpty(category))
            {
                employees = employees.Where(e => e.Category == category);
                ViewBag.SelectedCategory = category;
            }

            if (!string.IsNullOrEmpty(position))
            {
                employees = employees.Where(e => e.Position.Contains(position));
                ViewBag.PositionQuery = position;
                ViewBag.PositionSuggestions = _context.Employees
                    .Where(e => e.Position.Contains(position))
                    .Select(e => e.Position)
                    .Distinct()
                    .ToList();
            }

            ViewBag.Categories = _context.Employees.Select(e => e.Category).Distinct().ToList();

            return View(employees.ToList());
        }

        [HttpGet]
        public JsonResult GetDepartmentSuggestions(string term)
        {
            var departments = _context.Employees
                .Where(e => e.Department.Contains(term))
                .Select(e => e.Department)
                .Distinct()
                .ToList();

            return Json(departments);
        }

        [HttpGet]
        public JsonResult GetPositionSuggestions(string term)
        {
            var positions = _context.Employees
                .Where(e => e.Position.Contains(term))
                .Select(e => e.Position)
                .Distinct()
                .ToList();

            return Json(positions);
        }
    }
}
