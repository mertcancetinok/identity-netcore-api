using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string UserExists = "Böyle bir kullanıcı var.";
        public static string UserSuccess = "Başarıyla üye olundu.";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordIncorrect = "Girdiğiniz şifre hatalı";
        public static string EmailNotConfirmed = "Lütfen hesabınızı doğrulayınız.Size bir e-posta gönderdik";
        public static string LoginSuccess = "Giriş başarılı";
        public static string LoginNotSuccess = "Giriş başarılı değil.";
    }
}
