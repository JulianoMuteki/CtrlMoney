using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using CtrlMoney.WorkSheet.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace CtrlMoney.UI.Web.Controllers
{
    public class B3Controller : Controller
    {
        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        public B3Controller(IXLWorkbookService xLWorkbookService, IBrokerageHistoryService brokerageHistoryService)
        {
            _xLWorkbookService = xLWorkbookService;
            _brokerageHistoryService1 = brokerageHistoryService;
        }

        public IActionResult Index()
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
                model.IsResponse = true;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, "b3.xlsx");
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                model.IsSuccess = true;
                model.Message = "File upload successfully";

               var brokerageHistories = _xLWorkbookService.ImportSheet();
                _brokerageHistoryService1.AddRange(brokerageHistories);
            }
            return View(model);
        }
    }
}
