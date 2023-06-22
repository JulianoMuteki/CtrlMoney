using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CtrlMoney.UI.Web.Controllers
{
    public class B3Controller : Controller
    {

        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        private readonly IPositionService _positionService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        
        private IDictionary<string, string> _keyTickerValuePairs = new Dictionary<string, string>();
        public B3Controller(IXLWorkbookService xLWorkbookService, IBrokerageHistoryService brokerageHistoryService,
                            IPositionService positionService, IEarningService earningService, IMovimentService movementService)
        {
            _xLWorkbookService = xLWorkbookService;
            _brokerageHistoryService1 = brokerageHistoryService;
            _positionService = positionService;
            _earningService = earningService;
            _movementService = movementService;
            

            ReadCategoryTickers();
        }

        private void ReadCategoryTickers()
        {
            var brokeragesHistory = _brokerageHistoryService1.GetAll().Select(x=> new KeyValuePair<string, string>(x.TicketCode, x.Category)).Distinct();
            var earnings = _earningService.GetAll().Select(x => new KeyValuePair<string, string>(x.TicketCode, x.Category)).Distinct();

            foreach (var item in brokeragesHistory)
            {
                SetTickersValuePairs(item);
            }

            foreach (var item in earnings)
            {
                SetTickersValuePairs(item);
            }
        }

        private void SetTickersValuePairs(KeyValuePair<string, string> keyValuePair)
        {
            if (!_keyTickerValuePairs.ContainsKey(keyValuePair.Key))
            {
                _keyTickerValuePairs.Add(keyValuePair.Key, keyValuePair.Value);
            }
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
                var brokerageHistories = _xLWorkbookService.ImportTransactionsB3Sheet(fullfileName, _keyTickerValuePairs);
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
                var earnings = _xLWorkbookService.ImportEarningsB3Sheet(fullfileName, _keyTickerValuePairs);

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

        public IActionResult MovementIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerMovements()
        {
            var positions = _movementService.GetAll();

            return Json(new
            {
                aaData = positions,
                success = true
            });
        }

        public IActionResult UploadMovement()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadMovement(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                var fullfileName = CreateFile(model);
                var moviments = _xLWorkbookService.ImportMovimentsB3Sheet(fullfileName, _keyTickerValuePairs);
                _movementService.AddRange(moviments);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("MovementIndex");
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
            FileInfo fInfo = new FileInfo(fullfileName);
            if (!fInfo.Exists)
            {
                throw new Exception($"Error in copy file: {model.FileName} with fullname: {fullfileName}");
            }

            return fullfileName;
        }

        public IActionResult EditMovementValue(Guid ticketId)
        {
            var model = _movementService.GetById(ticketId);
            MovementVM movementVM = new MovementVM()
            {
                TicketId = model.Id,
                TicketCode = model.TicketCode,
                StockBroker = model.StockBroker,
                Date = model.Date,
                InputOutput = model.InputOutput,
                Quantity = model.Quantity,
                MovimentType = model.MovimentType,
                UnitPrice = model.UnitPrice
            };

            return View(movementVM);
        }

        [HttpPost]
        public IActionResult EditMovementValue(MovementVM model)
        {
            if (ModelState.IsValid)
            {
                var movement = _movementService.GetById(model.TicketId);
                movement.UnitPrice = model.UnitPrice;
                movement.Quantity = model.Quantity;
                movement.TransactionValue = movement.Quantity * movement.UnitPrice;
                _movementService.Update(movement);
            }
            return RedirectToAction("MovementIndex");
        }
    }
}
