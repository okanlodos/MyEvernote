using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter.")]
        public string Username { get; set; }

        [DisplayName("Email"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter."),
            EmailAddress(ErrorMessage ="Geçerli bir {0} adresi giriniz.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} Max. {1} Karakter."),
            Compare("Password", ErrorMessage ="Şifre ile şifre tekrar uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}