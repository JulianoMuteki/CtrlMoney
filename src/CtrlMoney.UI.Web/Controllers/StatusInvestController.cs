using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Extensions;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CtrlMoney.UI.Web.Controllers
{
    public class StatusInvestController : Controller
    {
        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        private readonly IEarningService _earningService;
        public StatusInvestController(IXLWorkbookService xLWorkbookService, IBrokerageHistoryService brokerageHistoryService,
                            IEarningService earningService)
        {
            _xLWorkbookService = xLWorkbookService;
            _brokerageHistoryService1 = brokerageHistoryService;
            _earningService = earningService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BrokeragesHistories()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerBrokeragesHistories()
        {
            var brokeragesHistories = _brokerageHistoryService1.GetAll();

            return Json(new
            {
                aaData = brokeragesHistories,
                success = true
            });
        }

        public IActionResult UploadTransaction()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadTransaction(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                var fullfileName = this.CreateFile(model);
                var brokerageHistories = _xLWorkbookService.ImportSITransactionsSheet(fullfileName);
                _brokerageHistoryService1.AddRange(brokerageHistories);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("BrokeragesHistories");
        }
    }
}
