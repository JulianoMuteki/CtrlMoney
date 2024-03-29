﻿using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CtrlMoney.UI.Web.Controllers
{
    public class ManualRegistrationController : Controller
    {
        private readonly ITicketConversionService _ticketConversionService;
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        private readonly IPositionService _positionService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        public ManualRegistrationController(ITicketConversionService ticketConversionService, IBrokerageHistoryService brokerageHistoryService,
                            IPositionService positionService, IEarningService earningService, IMovimentService movementService)
        {
            _ticketConversionService = ticketConversionService;
            _brokerageHistoryService1 = brokerageHistoryService;
            _positionService = positionService;
            _earningService = earningService;
            _movementService = movementService;
        }

        // GET: ManualRegistrationController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerConversions()
        {
            var conversions = _ticketConversionService.GetAll();
           
            return Json(new
            {
                aaData = conversions,
                success = true
            });
        }

        // GET: ManualRegistrationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManualRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManualRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketConversionVM ticketConversionVM)
        {
            try
            {
                _ticketConversionService.Add(new TicketConversion()
                {
                    TicketInput = ticketConversionVM.TicketInput,
                    TicketOutput = ticketConversionVM.TicketOutput,
                    Quantity = ticketConversionVM.Quantity,
                    UnitPrice = ticketConversionVM.UnitPrice,
                    StockBroker = ticketConversionVM.StockBroker,
                    Date = ticketConversionVM.Date
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: ManualRegistrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManualRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManualRegistrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManualRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult DeleteImport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteImport(string selectedDate)
        {
            string[] dates = selectedDate.Split('-');
            
            DateTime dtStart = DateTime.Parse(dates[0].Trim());
            DateTime dtEnd = DateTime.Parse(dates[1].Trim());

            _movementService.DeleteByRangeDate(dtStart, dtEnd);
            _earningService.DeleteByRangeDate(dtStart, dtEnd);
            _brokerageHistoryService1.DeleteByRangeDate(dtStart, dtEnd);

            ViewBag.Message = $"Deleted data from {dtStart} to {dtEnd}";
            return View();
        }
    }
}
