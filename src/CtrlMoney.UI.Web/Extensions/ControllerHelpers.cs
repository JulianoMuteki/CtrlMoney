using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace CtrlMoney.UI.Web.Extensions
{
    public static class ControllerHelpers
    {
        public static string CreateFile(this Controller controller, SingleFileModel model)
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
