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
        private readonly IPositionService _positionService;
        private readonly IEarningService _earningService;
        public B3Controller(IXLWorkbookService xLWorkbookService, IBrokerageHistoryService brokerageHistoryService, IPositionService positionService, IEarningService earningService)
        {
            _xLWorkbookService = xLWorkbookService;
            _brokerageHistoryService1 = brokerageHistoryService;
            _positionService = positionService;
            _earningService = earningService;
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
            return RedirectToAction("Index");
        }


        public IActionResult EarningIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerEarnings()
        {
            var earnings = _earningService.GetAll();

            return Json(new
            {
                aaData = earnings,
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
                var earnings = _xLWorkbookService.ImportEarningsSheet(fullfileName);
                _earningService.AddRange(earnings);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("EarningIndex");
        }


        public IActionResult PositionIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerPositions()
        {
            var positions = _positionService.GetAll();

            return Json(new
            {
                aaData = positions,
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
                var positions = _xLWorkbookService.ImportPositionsSheet(fullfileName, DateTime.Parse(model.FileName));
                _positionService.AddRange(positions);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("PositionIndex");
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
