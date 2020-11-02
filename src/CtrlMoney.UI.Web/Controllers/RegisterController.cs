using System;
using System.Collections.Generic;
using System.Linq;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Helpers;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CtrlMoney.UI.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IBankAppService _bankAppService;
        private readonly IRegisterAppService _registerAppService;
        public RegisterController(IBankAppService bankAppService, IRegisterAppService registerAppService)
        {
            _bankAppService = bankAppService;
            _registerAppService = registerAppService;
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

        [HttpPost]
        public IActionResult PostAjaxHandlerAddBanks(string[] claimsJSON)
        {
            Guid id;

            if (claimsJSON[0] != string.Empty)
            {
                JsonSerialize jsonS = new JsonSerialize();
                var banksVM = jsonS.JsonDeserializeObject<List<Selected2VM>>(claimsJSON[0]);

                IList<Bank> banks = new List<Bank>();
                foreach (var item in banksVM)
                {
                    banks.Add(new Bank()
                    {
                        Name = item.text,
                        BankCode = Convert.ToInt32(item.id)
                    });
                }

                _bankAppService.AddRange(banks);
                return Json(new { OK = "ok" });
            }
            else
            {
                return Json(new { OK = "Without user" });
            }
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerMyBanks()
        {
            var banks = _bankAppService.GetAll();

            var banksVM = banks.Select(x => new BankVM(x)).ToList();
            return Json(new
            {
                aaData = banksVM,            
                success = true
            });
        }

        public IActionResult MenusGroup()
        {
            return View();
        }

        public IActionResult TreeView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetNodes()
        {
            try
            {
                var trees = _registerAppService.GetAll();

                var result = ConvertTreeToNodes.ConvertToJson(trees);

                return Json(new
                {
                    aaData = result
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddTreeData(string nodeID, string titleTree, string description, string tag)
        {
            try
            {
                _registerAppService.AddTree(nodeID, titleTree, description, tag);
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
