using AlDawarat_W_AlEngazat.Areas.Identity.Data;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class AchievementsSearchController : Controller
{
    private readonly ApplicationDbContext _context;

    public AchievementsSearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(DateTime fromDate1, DateTime toDate1, DateTime fromDate2, DateTime toDate2)
    {
        // Retrieve achievements within the selected date ranges
        var achievements1 = _context.Achievements
                            .Where(a => a.StartDate >= fromDate1 && a.StartDate <= toDate1)
                            .ToList();

        var achievements2 = _context.Achievements
                            .Where(a => a.StartDate >= fromDate2 && a.StartDate <= toDate2)
                            .ToList();

        // Store data in ViewBag
        ViewBag.Achievements1 = achievements1;
        ViewBag.Achievements2 = achievements2;
        ViewBag.Count1 = achievements1.Count;
        ViewBag.Count2 = achievements2.Count;
        ViewBag.FromDate1 = fromDate1.ToShortDateString();
        ViewBag.ToDate1 = toDate1.ToShortDateString();
        ViewBag.FromDate2 = fromDate2.ToShortDateString();
        ViewBag.ToDate2 = toDate2.ToShortDateString();

        return View();
    }
}
