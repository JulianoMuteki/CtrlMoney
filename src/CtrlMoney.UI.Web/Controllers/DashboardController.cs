using CtrlMoney.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

namespace CtrlMoney.UI.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IReportService _reportService;
        public DashboardController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCompositionTotalStocks()
        {
            return Json(new
            {
                aaData = _reportService.GetCompositionTotalStocks("all"),
                success = true
            });
        }
    }
}
