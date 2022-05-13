using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.ViewModels;
using EFreshStore.Utility;
using Newtonsoft.Json.Linq;

namespace EFreshStore.Controllers
{
    public class CouponController : Controller
    {
        public JsonResult CheckValidity(string couponCode)
        {

            if (couponCode != null && Session["userTypeId"] != null && Session["GrandTotal"] != null)
            {
                var userTypeId = Convert.ToInt64(Session["userTypeId"]);
                var userId = Convert.ToInt64(Session["UserId"]);
                var grandTotal = Convert.ToDouble(Session["GrandTotal"]);
                CouponDiscountVM couponDiscountFromApi = null;
                CouponDiscountVM couponDiscount = new CouponDiscountVM
                {
                    CouponCode = couponCode,
                    UserTypeId = userTypeId,
                    GrandTotal = grandTotal,
                    UserId = userId
                };

                var result = ApiUtility.CheckCouponValidityAndCalculateDiscount(couponDiscount);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = result.Content.ReadAsAsync<CouponDiscountVM>();
                    readTask.Wait();
                    couponDiscountFromApi = readTask.Result;
                    Session["couponCode"] = couponCode;
                    Session["finalCouponDiscount"] = couponDiscountFromApi.FinalCouponDiscount;
                    Session["totalUpdatePrice"] = couponDiscountFromApi.TotalUpdatedPrice;
                    return Json(new { couponDiscountFromApi, status = "Ok" });
                }
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return Json(new { status = "NotFound" });
                }

                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    dynamic response = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    return Json(new { status = "BadRequest", message = response.message.ToString() });
                }

            }

            return Json(new { status = "Error" });
        }

        public ActionResult SetAndUpdateSession(string couponCode, string finalCouponDiscount, string totalUpdatePrice)
        {
            Session["couponCode"] = couponCode;
            Session["finalCouponDiscount"] = finalCouponDiscount;
            Session["totalUpdatePrice"] = totalUpdatePrice;
            return this.Json(new { success = true });
        }

        public ActionResult ClearSession()
        {
            Session["couponCode"] = null;
            Session["finalCouponDiscount"] = null;
            Session["totalUpdatePrice"] = null;
            return this.Json(new { success = true });
        }

    }
}