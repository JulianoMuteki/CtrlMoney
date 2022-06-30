﻿using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using CtrlMoney.WorkSheet.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CtrlMoney.UI.Web.Controllers
{
    public class B3Controller : Controller
    {
        private readonly IXLWorkbookService _xLWorkbookService;
        public B3Controller(IXLWorkbookService xLWorkbookService)
        {
            _xLWorkbookService = xLWorkbookService;
        }

        public IActionResult Index()
        {
            return View();
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

                _xLWorkbookService.ImportSheet();
            }
            return View(model);
        }
    }
}