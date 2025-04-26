using System.ComponentModel.DataAnnotations;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمة المرور مطلوبة لإكمال العملية")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "يجب انت يتراوح طول كلمة المرور من 6 إلي 100")]
        public string NewPassword { get; set; }


        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة مع تأكييد كلمة المرور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "تأكييد كلمة المرور مطلوب لإكمال العملية")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "يجب انت يتراوح طول كلمة المرور من 6 إلي 100")]
        public string ConfirmPassword { get; set; }

    }
}
