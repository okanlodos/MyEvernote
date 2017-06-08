using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Common;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class UserManager : ManagerBase<EvernoteUser>
    {
        public BusinessLayerResult<EvernoteUser> FindUsername(RegisterViewModel reg_user)
        { 

            BusinessLayerResult<EvernoteUser> LayerResult = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Username == reg_user.Username || x.Email == reg_user.Email);
            if (user != null)
            {
                if (user.Username == reg_user.Username)
                {
                    LayerResult.Errors.Add("Kullanıcı Adı Kullanılıyor.");
                }
                if (user.Email == reg_user.Email)
                {
                    LayerResult.Errors.Add("Email Kullanılıyor.");
                }
            }
            else
            {
                int dbResult = Insert(new EvernoteUser() {
                    Username = reg_user.Username,
                    Password = reg_user.Password,
                    Email = reg_user.Email,
                    ProfilImagePath = "user.png",
                    ActivateGuid = Guid.NewGuid(),
                    isActive = false,
                    IsAdmin = false,
                });
                if (dbResult > 0)
                {
                    LayerResult.Result = Find(x => x.Email == reg_user.Email && x.Username == reg_user.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUri = $"{siteUri}/Home/UserActivate/{LayerResult.Result.ActivateGuid}";
                    MailHelper.SendMail($"Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.", LayerResult.Result.Email, "Hesap Activate");
                }
               
            }
            return LayerResult;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        { 
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);
            if (res.Result != null)
            {
                if (!res.Result.isActive)
                {
                    res.Errors.Add("Üyelik aktifleştirilmemiştir. Lütfen E-posta kutunuzu kontrol ediniz.");
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid data)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == data);
            if (res.Result != null)
            {
                if (res.Result.isActive)
                {
                    res.Errors.Add("Hesap zaten aktif.");
                }
                else
                {
                    res.Result.isActive = true;
                    Update(res.Result);
                }
            }
            else
            {
                res.Errors.Add("Aktivikasyon koduna ait bir kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> LayerResult = new BusinessLayerResult<EvernoteUser>();
            LayerResult.Result = Find(x => x.Id == id);
            if (LayerResult.Result == null)
            {
                LayerResult.Errors.Add("Kullanıcı Bulunamadı.");
            }
            return LayerResult;
        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser model)
        {
            BusinessLayerResult<EvernoteUser> LayerResult = new BusinessLayerResult<EvernoteUser>();
            LayerResult.Result = Find(x => x.Username == model.Username || x.Email == model.Email);
            if (LayerResult.Result != null && LayerResult.Result.Id != model.Id)
            {
                if (LayerResult.Result.Username == model.Username)
                {
                    LayerResult.Errors.Add("Kullanıcı adı kayıtlı.");
                }
                if (LayerResult.Result.Email == model.Email)
                {
                    LayerResult.Errors.Add("Email kayıtlı.");
                }
            }
            LayerResult.Result = Find(x => x.Id == model.Id);
            LayerResult.Result.Email = model.Email;
            LayerResult.Result.Name = model.Name;
            LayerResult.Result.Surname = model.Surname;
            LayerResult.Result.Password = model.Password;
            LayerResult.Result.Username = model.Username;
            //LayerResult.Result.isActive = model.isActive;
            //LayerResult.Result.IsAdmin = model.IsAdmin;
            LayerResult.Result.ProfilImagePath = model.ProfilImagePath;
            if (Update(LayerResult.Result) == 0)
            {
                LayerResult.Errors.Add("Profil güncellenemedi.");
            }
            return LayerResult;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);
            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.Errors.Add("Kullanıcı Silinemedi.");
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı Bulunamadı.");
            }
            return res;
        }
    }
}
