using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlDawarat_W_AlEngazat.Controllers {
    [Authorize(Roles = "Admin")]

    public class SearchController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
