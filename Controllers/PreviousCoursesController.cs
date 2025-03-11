using AlDawarat_W_AlEngazat.Areas.Identity.Data;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlDawarat_W_AlEngazat.Controllers {
    [Authorize(Roles = "Admin")]
    public class PreviousCoursesController : Controller {
        private readonly ApplicationDbContext _context;

        public PreviousCoursesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: PreviousCourses
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.PreviousCourses.Include(p => p.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PreviousCourses/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var previousCourse = await _context.PreviousCourses
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (previousCourse == null) {
                return NotFound();
            }

            return View(previousCourse);
        }

        /*// GET: PreviousCourses/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "Category");
            return View();
        }

        // POST: PreviousCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CourseName,CompletionDate,Location,EmployeeID")] PreviousCourse previousCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(previousCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "Category", previousCourse.EmployeeID);
            return View(previousCourse);
        }*/

        // GET: PreviousCourses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var previousCourse = await _context.PreviousCourses.FindAsync(id);
            if (previousCourse == null) {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "Name", previousCourse.EmployeeID);
            return View(previousCourse);
        }

        // POST: PreviousCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CourseName,CompletionDate,Location,EmployeeID")] PreviousCourse previousCourse) {
            if (id != previousCourse.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(previousCourse);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!PreviousCourseExists(previousCourse.ID)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "Category", previousCourse.EmployeeID);
            return View(previousCourse);
        }

        // GET: PreviousCourses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var previousCourse = await _context.PreviousCourses
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (previousCourse == null) {
                return NotFound();
            }

            return View(previousCourse);
        }

        // POST: PreviousCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var previousCourse = await _context.PreviousCourses.FindAsync(id);
            if (previousCourse != null) {
                _context.PreviousCourses.Remove(previousCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreviousCourseExists(int id) {
            return _context.PreviousCourses.Any(e => e.ID == id);
        }
    }
}
