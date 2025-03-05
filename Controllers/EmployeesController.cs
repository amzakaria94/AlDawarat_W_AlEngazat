using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public EmployeesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Employees
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Employees.Include(e => e.Courses);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Courses)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location");
			return View();
		}

		// POST: Employees/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Name,Rank,Number,Category,Department,Position,Certificate,CourseID")] Employee employee)
		{
			if (ModelState.IsValid)
			{
				_context.Add(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// POST: Employees/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Rank,Number,Category,Department,Position,Certificate,CourseID")] Employee employee)
		{
			if (id != employee.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(employee);
					await _context.SaveChangesAsync();
				} catch (DbUpdateConcurrencyException)
				{
					if (!EmployeeExists(employee.ID))
					{
						return NotFound();
					} else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Courses)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.ID == id);
		}
	}
}