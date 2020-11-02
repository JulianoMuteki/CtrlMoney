using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CtrlMoney.UI.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IBankAppService _bankAppService;
        private readonly IRegisterAppService _registerAppService;
        public TransactionController(IBankAppService bankAppService, IRegisterAppService registerAppService)
        {
            _bankAppService = bankAppService;
            _registerAppService = registerAppService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult AddTransaction()
        {
            var banks = _bankAppService.GetAll()
               .Select(bank => new SelectListItem
               {
                   Value = bank.BankCode.ToString(),
                   Text = bank.Name
               }).ToList();

            ViewData["ListBanks"] = banks;
            return View();
        }

        [HttpGet]
        public IActionResult GetAjaxHandlerMyBanks()
        {
            var banks = _bankAppService.GetAll();

            var banksVM = banks.Select(x => new BankVM(x)).ToList();
            return Json(new
            {
                aaData = banksVM
            });
        }
    }
}
