using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyHospital.Web.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется имя")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}