using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.ViewModels;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        // GET: Category
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cat = categoryManager.Find(x => x.Id == id.Value);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> eu = userManager.LoginUser(model);
                if (eu.Errors.Count > 0)
                {
                    eu.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                else
                {
                    Session["login"] = eu.Result;
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser>  result = userManager.FindUsername(model);
                if (result.Errors.Count > 0)
                {
                    foreach (string item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                    return View(model);
                }
                OkViewModel notofy_ok = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/home/login"
                };
                notofy_ok.Item.Add("Lütfen emailinize gelen aktivasyon linkine tıklayarak üyeliğinizi aktif ediniz.");

                return View("Ok", notofy_ok);
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            if (id != null)
            {
                BusinessLayerResult<EvernoteUser> res = userManager.ActivateUser(id);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel err = new ErrorViewModel()
                    {
                        Title = "Geçersiz işlem.",
                        Item = res.Errors
                    };
                    return View("Error", err);
                }
            }
            OkViewModel ok = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi.",
                RedirectingUrl = "/home/login",
            };
            ok.Item.Add("Hesabınız Aktifleştirildi. Artık not paylaşabilirsiniz.");
            return View("Ok", ok);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ShowProfile()
        {
            // TODO : Bunda eksik var. -- Tamamlandı işlem bitti
            if (Session["login"] != null)
            {
                EvernoteUser user = Session["login"] as EvernoteUser;
                BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(user.Id);
                if (res.Errors.Count > 0)
                {
                     ErrorViewModel err = new ErrorViewModel()
                    {
                        Title = "Hay aksi..",
                        Item = res.Errors
                    };
                    return View("Error", err);
                }
                return View(res.Result);
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditProfile()
        {
            if (Session["login"] != null)
            {
                EvernoteUser user = Session["login"] as EvernoteUser;
                BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(user.Id);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel err = new ErrorViewModel()
                    {
                        Title = "Hay aksi..",
                        Item = res.Errors
                    };
                    return View("Error", err);
                }
                return View(res.Result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfilImagePath = filename;
                }
                BusinessLayerResult<EvernoteUser> res = userManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Item = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.

                Session["login"] = res.Result;

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        public ActionResult DeleteProfile()
        {
            if (Session["login"] != null)
            {
                EvernoteUser user = Session["login"] as EvernoteUser;
                BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(user.Id);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel err = new ErrorViewModel()
                    {
                        Title = "Hay aksi.. Kullanıcı silinemedi.",
                        Item = res.Errors
                    };
                    return View("Error", err);
                }
                
            }
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
