using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Controllers {
	public class EmployeesController : Controller {
		private readonly ApplicationDbContext _context;

		public EmployeesController(ApplicationDbContext context) {
			_context = context;
		}

		// GET: Employees
		public async Task<IActionResult> Index(string searchName, string searchRank, string searchNumber, string searchDepartment, string searchPosition, string searchCertificate, string searchCourseName, string searchCategory, int? pageNumber) {
			var employees = _context.Employees
				.Include(e => e.Course)
				.AsQueryable();

			ViewBag.EmployeeCategories = await _context.Employees
				.Select(e => e.Category)
				.Distinct()
				.ToListAsync();

			var totalEmployees = await _context.Employees.CountAsync();
			ViewData["TotalEmployees"] = totalEmployees;

			if (!string.IsNullOrEmpty(searchName)) {
				employees = employees.Where(e => e.Name.Contains(searchName));
			}

			if (!string.IsNullOrEmpty(searchRank)) {
				employees = employees.Where(e => e.Rank.Contains(searchRank));
			}

			if (!string.IsNullOrEmpty(searchNumber)) {
				employees = employees.Where(e => e.Number.Contains(searchNumber));
			}

			if (!string.IsNullOrEmpty(searchDepartment)) {
				employees = employees.Where(e => e.Department.Contains(searchDepartment));
			}

			if (!string.IsNullOrEmpty(searchPosition)) {
				employees = employees.Where(e => e.Position.Contains(searchPosition));
			}

			if (!string.IsNullOrEmpty(searchCertificate)) {
				employees = employees.Where(e => e.Certificate.Contains(searchCertificate));
			}

			if (!string.IsNullOrEmpty(searchCourseName)) {
				employees = employees.Where(e => e.Course.Name.Contains(searchCourseName));
			}

			if (!string.IsNullOrEmpty(searchCategory)) {
				employees = employees.Where(e => e.Category == searchCategory);
				ViewBag.SelectedCategory = searchCategory;
			}

			int pageSize = 20;
			return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
		}

		// Other actions...

		[HttpGet]
		public async Task<IActionResult> GetEmployeeNames(string query) {
			Console.WriteLine("Query: " + query);
			if (string.IsNullOrWhiteSpace(query)) {
				return Json(new List<object>()); // Return empty list if query is empty
			}

			var employeeNames = await _context.Employees
				.Where(e => e.Name.Contains(query))
				.Select(e => new { name = e.Name, id = e.ID })
				.ToListAsync();

			return Json(employeeNames);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeeRanks(string query) {
			var employeeRanks = await _context.Employees
				.Where(e => e.Rank.Contains(query))
				.Select(e => new { name = e.Rank })
				.ToListAsync();

			return Json(employeeRanks);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeeNumbers(string query) {
			var employeeNumbers = await _context.Employees
				.Where(e => e.Number.Contains(query))
				.Select(e => new { name = e.Number })
				.ToListAsync();

			return Json(employeeNumbers);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeeDepartments(string query) {
			var employeeDepartments = await _context.Employees
				.Where(e => e.Department.Contains(query))
				.Select(e => new { name = e.Department })
				.ToListAsync();

			return Json(employeeDepartments);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeePositions(string query) {
			var employeePositions = await _context.Employees
				.Where(e => e.Position.Contains(query))
				.Select(e => new { name = e.Position })
				.ToListAsync();

			return Json(employeePositions);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeeCertificates(string query) {
			var employeeCertificates = await _context.Employees
				.Where(e => e.Certificate.Contains(query))
				.Select(e => new { name = e.Certificate })
				.ToListAsync();

			return Json(employeeCertificates);
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Course) // FIX: Use 'Course' instead of 'Courses'
				.Include(e => e.PreviousCourses)
				.FirstOrDefaultAsync(m => m.ID == id);

			if (employee == null) {
				return NotFound();
			}

			ViewBag.Courses = _context.Courses.ToList();

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create() {
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location");
			return View();
		}

		// POST: Employees/Create To protect from overposting attacks, enable the specific
		// properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Name,Rank,Number,Category,Department,Position,Certificate,CourseID")] Employee employee) {
			if (ModelState.IsValid) {
				_context.Add(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null) {
				return NotFound();
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// POST: Employees/Edit/5 To protect from overposting attacks, enable the specific
		// properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Rank,Number,Category,Department,Position,Certificate,CourseID")] Employee employee) {
			if (id != employee.ID) {
				return NotFound();
			}

			if (ModelState.IsValid) {
				try {
					var existingEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
					if (existingEmployee == null) {
						return NotFound();
					}

					employee.CourseID = existingEmployee.CourseID;
					_context.Update(employee);
					await _context.SaveChangesAsync();
				} catch (DbUpdateConcurrencyException) {
					if (!EmployeeExists(employee.ID)) {
						return NotFound();
					} else {
						throw;
					}
				}
				return RedirectToAction(nameof(Details), new { id = employee.ID });
			}
			ViewData["CourseID"] = new SelectList(_context.Courses, "ID", "Location", employee.CourseID);
			return View(employee);
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Course) // Use Course (not Courses)
				.FirstOrDefaultAsync(m => m.ID == id);

			if (employee == null) {
				return NotFound();
			}

			return View(employee);
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null) {
				_context.Employees.Remove(employee);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Employees/AssignEmployeeToCourse/5
		public async Task<IActionResult> AssignEmployeeToCourse(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.ID == id);

			if (employee == null) {
				return NotFound();
			}

			ViewBag.Courses = await _context.Courses.ToListAsync();
			return View(employee);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssignEmployeeToCourse(int employeeId, int courseId) {
			var employee = await _context.Employees.FindAsync(employeeId);
			if (employee == null) {
				return NotFound();
			}

			var course = await _context.Courses.FindAsync(courseId);
			if (course == null) {
				return NotFound();
			}

			employee.CourseID = courseId;
			_context.Update(employee);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Details), new { id = employeeId });
		}

		// GET: Employees/AddPreviousCourse/5
		public async Task<IActionResult> AddPreviousCourse(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.ID == id);

			if (employee == null) {
				return NotFound();
			}

			var previousCourse = new PreviousCourse {
				EmployeeID = employee.ID,
				CompletionDate = DateTime.Today, // Set a default date
				StartDate = DateTime.Today
			};

			ViewBag.EmployeeName = employee.Name; // Add employee name to ViewBag for display
			return View(previousCourse);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddPreviousCourse([Bind("CourseName,CompletionDate,StartDate,Location,EmployeeID")] PreviousCourse previousCourse) {
			// Remove ID from the binding to let the database generate it automatically

			if (ModelState.IsValid) {
				try {
					// Verify the employee exists
					var employee = await _context.Employees.FindAsync(previousCourse.EmployeeID);
					if (employee == null) {
						ModelState.AddModelError("EmployeeID", "Employee not found");
						return View(previousCourse);
					}

					// Add the previous course
					_context.PreviousCourses.Add(previousCourse);
					await _context.SaveChangesAsync();

					// Redirect to the employee details page or another appropriate page
					return RedirectToAction("Details", new { id = previousCourse.EmployeeID });
				} catch (Exception ex) {
					// Log the error
					Console.WriteLine($"Error adding previous course: {ex.Message}");
					ModelState.AddModelError("", "An error occurred while saving the previous course. Please try again.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(previousCourse);
		}
		// GET: Employees/ViewPreviousCourses/5
		public async Task<IActionResult> ViewPreviousCourses(int? id) {
			if (id == null) {
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.PreviousCourses)
				.FirstOrDefaultAsync(m => m.ID == id);

			if (employee == null) {
				return NotFound();
			}

			return View(employee);
		}

		private bool EmployeeExists(int id) {
			return _context.Employees.Any(e => e.ID == id);
		}
	}
}
