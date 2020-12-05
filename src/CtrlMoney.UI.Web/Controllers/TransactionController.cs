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
        private readonly IGrandChildTreeAppService _grandChildTreeAppService;
        private readonly IChildTreeAppService _childTreeAppService;
        private readonly IParentTreeAppService _parentTreeAppService;
        public TransactionController(IBankAppService bankAppService, IRegisterAppService registerAppService, IGrandChildTreeAppService grandChildTreeAppService, IChildTreeAppService childTreeAppService, IParentTreeAppService parentTreeAppService)
        {
            _childTreeAppService = childTreeAppService;
            _grandChildTreeAppService = grandChildTreeAppService;
            _parentTreeAppService = parentTreeAppService;
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

            var parentsTrees = _parentTreeAppService.GetAll()
                .Select(tree => new SelectListItem
               {
                   Value = tree.Id.ToString(),
                   Text = tree.Title
                }).ToList();

            ViewData["ParentsTrees"] = parentsTrees;

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

        [HttpGet]
        public IActionResult GetAjaxHandlerChildTrees(string parentTreeID)
        {
            if(!string.IsNullOrEmpty(parentTreeID)){
                var childTrees = _childTreeAppService.GetListByParentID(new Guid(parentTreeID))
                    .Select(tree => new SelectListItem
                    {
                        Value = tree.Id.ToString(),
                        Text = tree.Title
                    }).ToList();
                return Json(childTrees);
            }
            else
            {
                return Json(new
                {
                    aaData = "Can not find ChildTrees by parentTreeID",
                    isSuccess = false
                });
            }
        }

        [HttpGet]
        public IActionResult GetAjaxHandlerGrandChildTrees(string childTreeID)
        {
            if (!string.IsNullOrEmpty(childTreeID))
            {
                var childTrees = _grandChildTreeAppService.GetListByParentID(new Guid(childTreeID))
                    .Select(tree => new SelectListItem
                    {
                        Value = tree.Id.ToString(),
                        Text = tree.Title
                    }).ToList();
                return Json(childTrees);
            }
            else
            {
                return Json(new
                {
                    aaData = "Can not find GrandChildTrees by childTreeID",
                    isSuccess = false
                });
            }
        }
    }
}
