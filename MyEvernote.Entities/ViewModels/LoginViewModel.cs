using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter.")]
        public string Username { get; set; }
        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter.")]
        public string Password { get; set; }
    }
}