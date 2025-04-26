using System.ComponentModel.DataAnnotations;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "البريد الالكتروني مطلوب لإكمال العملية")]
        [EmailAddress(ErrorMessage = "هذا البريد الالكتروني غير صالح")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "كلمة السر مطلوبة لإكمال العملية")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "يجب انت يتراوح طول كلمة المرور من 6 إلي 100")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // خاصية حفظ تسجيل الدخول
    }
}