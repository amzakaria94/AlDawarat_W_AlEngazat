﻿using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Controllers
{
	public class CoursesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CoursesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Courses
		public async Task<IActionResult> Index()
		{
			var courses = await _context.Courses
							  .Include(c => c.Employees) // Ensure Employees are loaded
							  .ToListAsync();
			return View(courses);
		}

		// GET: Courses/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses
				.FirstOrDefaultAsync(m => m.ID == id);
			if (course == null)
			{
				return NotFound();
			}

			return View(course);
		}

		// GET: Courses/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Courses/Create To protect from overposting attacks, enable the specific properties
		// you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Name,Location,StartDate,EndDate")] Course course)
		{
			if (ModelState.IsValid)
			{
				_context.Add(course);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(course);
		}

		// GET: Courses/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(id);
			if (course == null)
			{
				return NotFound();
			}
			return View(course);
		}

		// POST: Courses/Edit/5 To protect from overposting attacks, enable the specific properties
		// you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Location,StartDate,EndDate")] Course course)
		{
			if (id != course.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(course);
					await _context.SaveChangesAsync();
				} catch (DbUpdateConcurrencyException)
				{
					if (!CourseExists(course.ID))
					{
						return NotFound();
					} else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(course);
		}

		// GET: Courses/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses
				.FirstOrDefaultAsync(m => m.ID == id);
			if (course == null)
			{
				return NotFound();
			}

			return View(course);
		}

		// POST: Courses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course != null)
			{
				_context.Courses.Remove(course);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Courses/AssignEmployees/5
		public async Task<IActionResult> AssignEmployee(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(id);
			if (course == null)
			{
				return NotFound();
			}

			ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "Name");
			return View(course);
		}

		// POST: Courses/AssignEmployees/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssignEmployee(int id, int EmployeeID)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(EmployeeID);
			if (employee != null)
			{
				employee.CourseID = id;  // Assign the employee to this course
				_context.Update(employee);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Details), new { id = course.ID });
		}

		private bool CourseExists(int id)
		{
			return _context.Courses.Any(e => e.ID == id);
		}
	}
}