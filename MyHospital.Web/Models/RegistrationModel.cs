using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyHospital.Web.Models
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется имя")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется пароль")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Role { get; set; }
        public string Specialization { get; set; }
        public int? TaxCode { get; set; }
    }
}