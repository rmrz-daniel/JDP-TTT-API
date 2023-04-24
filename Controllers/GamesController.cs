using Microsoft.AspNetCore.Mvc;

namespace JDP_TTT_API.Controllers {


    public class GamesController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
