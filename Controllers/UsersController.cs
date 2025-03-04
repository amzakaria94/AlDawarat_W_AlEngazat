using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Controllers
{
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UsersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Users
		public async Task<IActionResult> Index()
		{
			int userCount = await _context.Users.CountAsync();

			ViewBag.UserCount = userCount;
			return View();
		}

		// Get all users count
		public async Task<IActionResult> List()
		{
			// Get all users
			var users = await _context.Users
				.Include(u => u.Achievements)
				.ToListAsync();

			return View(users);
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users.Include(u => u.Achievements)
				.FirstOrDefaultAsync(m => m.ID == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// GET: Users/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Users/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Name,NationalID,Category,Department,Position,Certificate")] User user, string achievementTitle, DateTime achievementStartDate, DateTime achievementEndDate)
		{
			if (ModelState.IsValid)
			{
				_context.Add(user);
				await _context.SaveChangesAsync();
				if (!string.IsNullOrEmpty(achievementTitle))
				{
					var achievement = new Achievement {
						Title = achievementTitle,
						StartDate = achievementStartDate,
						EndDate = achievementEndDate,
						UserID = user.ID // Link the achievement to the user
					};
					_context.Add(achievement);
					await _context.SaveChangesAsync();
				}
				return RedirectToAction(nameof(Index));
			}

			// Log validation errors
			foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
			{
				Console.WriteLine(error.ErrorMessage);
			}

			return View(user);
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// Include Achievements when fetching the user
			var user = await _context.Users
				.Include(u => u.Achievements)
				.FirstOrDefaultAsync(u => u.ID == id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,NationalID,Category,Department,Position,Certificate,Achievements")] User user)
		{
			if (id != user.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Fetch the existing user from the database
					var existingUser = await _context.Users
						.Include(u => u.Achievements)
						.FirstOrDefaultAsync(u => u.ID == id);

					if (existingUser == null)
					{
						return NotFound();
					}

					// Update the user properties
					existingUser.Name = user.Name;
					existingUser.NationalID = user.NationalID;
					existingUser.Category = user.Category;
					existingUser.Department = user.Department;
					existingUser.Position = user.Position;
					existingUser.Certificate = user.Certificate;

					// Update or add achievements
					foreach (var achievement in user.Achievements)
					{
						if (achievement.ID == 0)
						{
							// New achievement
							achievement.UserID = user.ID;
							_context.Add(achievement);
						} else
						{
							// Existing achievement
							var existingAchievement = existingUser.Achievements
								.FirstOrDefault(a => a.ID == achievement.ID);

							if (existingAchievement != null)
							{
								// Update the existing achievement
								existingAchievement.Title = achievement.Title;
								existingAchievement.StartDate = achievement.StartDate;
								existingAchievement.EndDate = achievement.EndDate;
							} else
							{
								// Achievement not found (should not happen)
								_context.Update(achievement);
							}
						}
					}

					// Save changes to the database
					await _context.SaveChangesAsync();
				} catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.ID))
					{
						return NotFound();
					} else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(List));
			}

			// Log ModelState errors
			foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
			{
				Console.WriteLine(error.ErrorMessage);
			}

			return View(user);
		}

		// GET: Users/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.ID == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user != null)
			{
				_context.Users.Remove(user);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.ID == id);
		}
	}
}