using AlDawarat_W_AlEngazat.Areas.Identity.Data;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Controllers {
    [Authorize(Roles = "Admin, Employee")]
    public class AchievementsController : Controller {
        private readonly ApplicationDbContext _context;

        public AchievementsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Achievements
        public async Task<IActionResult> Index(string searchTitle, DateTime? startDate, DateTime? endDate, int? pageNumber) {
            var achievements = _context.Achievements.AsQueryable();

            // Filter by title
            if (!string.IsNullOrEmpty(searchTitle)) {
                achievements = achievements.Where(a => a.Title.StartsWith(searchTitle));
            }

            // Filter by date range
            if (startDate.HasValue && endDate.HasValue) {
                achievements = achievements.Where(a => a.StartDate >= startDate && a.StartDate <= endDate);
            }

            int totalAchievements = await achievements.CountAsync();
            Console.WriteLine(totalAchievements);

            int pageSize = 10;
            var paginatedList = await PaginatedList<Achievement>.CreateAsync(achievements.AsNoTracking(), pageNumber ?? 1, pageSize);
            ViewBag.TotalAchievements = totalAchievements;

            return View(paginatedList);
        }

        // GET: Achievements/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var achievement = await _context.Achievements

                .FirstOrDefaultAsync(m => m.ID == id);
            if (achievement == null) {
                return NotFound();
            }

            return View(achievement);
        }

        // GET: Achievements/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Achievements/Create To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,StartDate,Description")] Achievement achievement) {
            if (ModelState.IsValid) {
                _context.Add(achievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(achievement);
        }
        [Authorize(Roles = "Admin")]
        // GET: Achievements/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null) {
                return NotFound();
            }
            return View(achievement);
        }

        // POST: Achievements/Edit/5 To protect from overposting attacks, enable the specific
        // properties you want to bind to. For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,StartDate,Description")] Achievement achievement) {
            if (id != achievement.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!AchievementExists(achievement.ID)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(achievement);
        }
        [Authorize(Roles = "Admin")]
        // GET: Achievements/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var achievement = await _context.Achievements
                .FirstOrDefaultAsync(m => m.ID == id);
            if (achievement == null) {
                return NotFound();
            }

            return View(achievement);
        }

        // POST: Achievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null) {
                _context.Achievements.Remove(achievement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AchievementExists(int id) {
            return _context.Achievements.Any(e => e.ID == id);
        }
    }
}
