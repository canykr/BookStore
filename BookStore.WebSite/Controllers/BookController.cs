using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebSite.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
