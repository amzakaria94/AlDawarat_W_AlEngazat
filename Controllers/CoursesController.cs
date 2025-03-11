using AlDawarat_W_AlEngazat.Areas.Identity.Data;
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
        public async Task<IActionResult> Index(string searchQuery, string searchLocation, string searchStartDate, string searchEndDate, string employeeCategory, int? pageNumber)
        {
            var courses = _context.Courses
                .Include(c => c.Employees)  // Fix: Ensure Employees are included
                .AsQueryable();

            ViewBag.EmployeeCategories = await _context.Employees
               .Select(e => e.Category)
               .Distinct()
               .ToListAsync();

            var totalCourses = await _context.Courses.CountAsync();
            ViewData["totalCourses"] = totalCourses;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                courses = courses.Where(c => c.Name.Contains(searchQuery));
            }

            // Filter by search parameters
            if (!string.IsNullOrEmpty(searchLocation))
            {
                courses = courses.Where(c => c.Location.Contains(searchLocation));
            }

            if (!string.IsNullOrEmpty(searchStartDate) && DateTime.TryParse(searchStartDate, out DateTime startDate))
            {
                courses = courses.Where(c => c.StartDate >= startDate);
            }

            if (!string.IsNullOrEmpty(searchEndDate) && DateTime.TryParse(searchEndDate, out DateTime endDate))
            {
                courses = courses.Where(c => c.EndDate <= endDate);
            }

            if (!string.IsNullOrEmpty(employeeCategory))
            {
                courses = courses.Where(c => c.Employees.Any(e => e.Category == employeeCategory));
                ViewBag.SelectedCategory = employeeCategory;
            }

            int pageSize = 10;
            return View(await PaginatedList<Course>.CreateAsync(courses.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.ID))
                    {
                        return NotFound();
                    }
                    else
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
            Console.WriteLine($"id is {id}");
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
        public async Task<IActionResult> AssignEmployee(int id, int EmployeeID, IFormCollection form)
        {
            Console.WriteLine($"Form Data Received: {string.Join(", ", form.Keys)}");
            Console.WriteLine($"EmployeeID from Form: {form["EmployeeID"]}");
            Console.WriteLine($"Parsed EmployeeID: {EmployeeID}");

            if (EmployeeID == 0)
            {
                ModelState.AddModelError("", "Invalid Employee selection.");
                return RedirectToAction("AssignEmployee", new { id });
            }

            var employee = await _context.Employees.FindAsync(EmployeeID);
            if (employee != null)
            {
                employee.CourseID = id;
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }

        //new method (retrieves a course and its associated employees from the database and allows filtering employees by name.)
        public async Task<IActionResult> CourseEmployees(int id, string searchQuery)
        {
            var course = await _context.Courses
                .Include(c => c.Employees) // Include employees
                .FirstOrDefaultAsync(c => c.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                course.Employees = course.Employees
                    .Where(e => e.Name.Contains(searchQuery))
                    .ToList();
            }

            ViewData["SearchQuery"] = searchQuery; // Keep the search value in the input
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeNames(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new List<string>());
            }

            var employeeNames = await _context.Employees
                .Where(e => e.Name.Contains(query))
                .Select(e => new { e.ID, e.Name })
                .ToListAsync();

            return Json(employeeNames);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEmployeeFromCourse(int courseId, int employeeId)
        {
            var course = await _context.Courses
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.ID == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var employee = course.Employees.FirstOrDefault(e => e.ID == employeeId);
            if (employee != null)
            {
                course.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        [HttpGet]
        public JsonResult GetCourseNames(string query)
        {
            var courseNames = _context.Courses
                .Where(c => c.Name.Contains(query))
                .Select(c => new { id = c.ID, name = c.Name })
                .ToList();

            return Json(courseNames);
        }

        [HttpGet]
        public JsonResult GetCourseLocations(string query)
        {
            var courseLocations = _context.Courses
                .Where(c => c.Location.Contains(query))
                .Select(c => new { name = c.Location })
                .Distinct()
                .ToList();

            return Json(courseLocations);
        }
    }
}
