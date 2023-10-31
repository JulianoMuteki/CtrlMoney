using Microsoft.AspNetCore.Mvc;

namespace CtrlMoney.UI.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
