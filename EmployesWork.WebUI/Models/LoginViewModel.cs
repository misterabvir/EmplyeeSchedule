using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployesWork.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Требуется ввести логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Требуется ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}