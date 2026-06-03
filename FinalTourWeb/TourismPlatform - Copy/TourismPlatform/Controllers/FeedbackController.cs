using Microsoft.AspNetCore.Mvc;

namespace TourismPlatform.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
