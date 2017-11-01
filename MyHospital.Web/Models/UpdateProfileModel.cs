using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyHospital.Web.Models
{
    public class UpdateProfileModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Требуется имя")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}