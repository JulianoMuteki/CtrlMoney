using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CtrlMoney.UI.Web.Models
{
    public class SingleFileModel : ReponseModel
    {
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
}
