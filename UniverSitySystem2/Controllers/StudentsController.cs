using Microsoft.AspNetCore.Mvc;

namespace UniverSity.Api.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
