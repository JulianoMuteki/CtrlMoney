using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CtrlMoney.UI.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IBankAppService _bankAppService;
        public RegisterController(IBankAppService bankAppService)
        {
            _bankAppService = bankAppService;
        }
        public IActionResult Banks()
        {
            var banks = _bankAppService.GetAllBanks()
                                       .Select(bank => new SelectListItem
                                       {
                                           Value = bank.BankCode.ToString(),
                                           Text = bank.Name
                                       }).ToList();

            ViewData["ListBanks"] = banks;
            return View();
        }
    }
}
