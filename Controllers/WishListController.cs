using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Models.ViewModels;
using EFreshStore.Utility;
using Newtonsoft.Json;

namespace EFreshStore.Controllers
{
    public class WishListController : Controller
    {
        // GET: WishList
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<WishListViewModel> wishList = null;
            if (Session["UserId"] != null)
            {
                wishList = GetWishListByUserId();
            }
            return View(wishList);
        }

        private IEnumerable<WishListViewModel> GetWishListByUserId()
        {
            IEnumerable<WishListViewModel> wishList = null;
            long userId = Convert.ToInt64(Session["UserId"]);
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("WishList/GetByUser?userId=" + userId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    wishList = (new JavaScriptSerializer()).Deserialize<List<WishListViewModel>>(readTask.Result);
                }
                else
                {
                    wishList = new List<WishListViewModel>();
                }
            }

            return wishList;
        }

        public JsonResult AddOrRemove(long prodUnitId)
        {
            long userId = 0;
            if (Session["UserId"] == null)
            {
                return Json(new {status = "UnAuthorized"});
            }

            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }

            var wishList = new WishList();
            wishList.ProductUnitId = prodUnitId;
            wishList.IsDeleted = false;
            wishList.UserId = userId;
            using (var client = new HttpClientDemo())
            {
                var json = JsonConvert.SerializeObject(wishList);
                var postTask =
                    client.PostAsync("WishList/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                postTask.Wait();
                var result = postTask.Result;
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    return Json(new { status = "Created"});
                }

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { status = "Ok" });
                }
            }

            return Json(new
            {
                status = "Error"
            });
        }
        [HttpPost]
        public JsonResult Delete(long wishListId)
        {
            using (var client = new HttpClientDemo())
            {
                var deleteTask = client.PostAsync("WishList/Delete?id="+wishListId, null);
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { status = "Ok" });
                }

                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Json(new { status = "BadRequest" });
                }
            }

            return Json(new { status = "Error" });
        }

        public JsonResult DeleteAll()
        {
            IEnumerable<WishListViewModel> wishList = GetWishListByUserId();
            foreach (var product in wishList)
            {
                using (var client = new HttpClientDemo())
                {
                    var deleteTask = client.PostAsync("WishList/Delete?id=" + product.WishListId, null);
                    deleteTask.Wait();
                    var result = deleteTask.Result;
                    
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return Json(new { status = "BadRequest" });
                    }
                }
            }

            return Json(new { status = "Ok" });
        }

        public JsonResult GetWishList()
        {
            IEnumerable<WishListViewModel> wishList = GetWishListByUserId();

            return Json(wishList, JsonRequestBehavior.AllowGet);
        }
    }
}