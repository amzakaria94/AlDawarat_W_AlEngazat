using AlDawarat_W_AlEngazat.Areas.Identity.Data;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Linq;

namespace AlDawarat_W_AlEngazat.Controllers {
    [Authorize(Roles = "Admin")]
    public class InevitabilityController : Controller {
        private readonly ApplicationDbContext _context;

        public InevitabilityController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index(string Position, string rank, string certificate) {
            var employees = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(Position)) {
                employees = employees.Where(e => e.Position.Contains(Position));
            }

            if (!string.IsNullOrEmpty(rank)) {
                employees = employees.Where(e => e.Rank.Contains(rank));
            }

            if (!string.IsNullOrEmpty(certificate)) {
                if (certificate == "مؤهل")
                    employees = employees.Where(e => e.Certificate == "غير مؤهل");
                else if (certificate == "غير مؤهل")
                    employees = employees.Where(e => e.Certificate == "تأسيسة" || e.Certificate == "متقدمة");
            }

            ViewBag.Position = Position;
            ViewBag.Rank = rank;
            ViewBag.Certificate = certificate;

            return View(employees.ToList());
        }
    }
}
