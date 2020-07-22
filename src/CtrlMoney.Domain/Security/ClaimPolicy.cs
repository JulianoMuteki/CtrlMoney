using System.ComponentModel.DataAnnotations;

namespace CtrlMoney.Domain.Security
{
    public enum ClaimPolicy
    {
        [Display(Name = "Create")]
        Create,    
        [Display(Name = "Read")]
        Read,
        [Display(Name = "Update")]
        Update,
        [Display(Name = "Delete")]
        Delete
    }
}
