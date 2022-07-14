using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using CtrlMoney.WorkSheet.Service;
using Microsoft.AspNetCore.Mvc;
using System;
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
                var fullfileName = CreateFile(model);
                var brokerageHistories = _xLWorkbookService.ImportTransactionsSheet(fullfileName);
                _brokerageHistoryService1.AddRange(brokerageHistories);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return View(model);
        }


        public IActionResult EarningIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerEarnings()
        {
            var brokeragesHistories = _brokerageHistoryService1.GetAll();

            return Json(new
            {
                aaData = brokeragesHistories,
                success = true
            });
        }

        public IActionResult UploadEarning()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadEarning(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                var fullfileName = CreateFile(model);
                var brokerageHistories = _xLWorkbookService.ImportEarningsSheet(fullfileName);
                // _brokerageHistoryService1.AddRange(brokerageHistories);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return View(model);
        }


        public IActionResult PositionIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerPositions()
        {
            var brokeragesHistories = _brokerageHistoryService1.GetAll();

            return Json(new
            {
                aaData = brokeragesHistories,
                success = true
            });
        }

        public IActionResult UploadPosition()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadPosition(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {             
                var fullfileName = CreateFile(model);
                var brokerageHistories = _xLWorkbookService.ImportPositionsSheet(fullfileName);
                // _brokerageHistoryService1.AddRange(brokerageHistories);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return View(model);
        }

        private string CreateFile(SingleFileModel model)
        {
            string fullfileName;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Guid idname = Guid.NewGuid();
            fullfileName = Path.Combine(path, $"{idname}.xlsx");
            using (var stream = new FileStream(fullfileName, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            return fullfileName;
        }
    }
}
