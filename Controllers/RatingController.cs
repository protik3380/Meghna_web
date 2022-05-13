using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EFreshStore.Models.Context;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.ViewModels;
using EFreshStore.Utility;
using Newtonsoft.Json;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class RatingController : Controller
    {
        [HttpPost]
        public ActionResult Create(Rating rating)
        {
           var product =  GetProductUnitByProductId(rating.ProductUnitId);
            long userId = 0;
            if (Session["UserId"] == null)
            {
                return Redirect("../Product/Details?ProductId="+product.ProductId);
            }

            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
                rating.UserId = userId;
            }
            try
            {
                if (rating != null)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var json = JsonConvert.SerializeObject(rating);
                        var postTask =
                            client.PostAsync("Rating/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                        
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            FlashMessage.Confirmation("Your review has been submitted");
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            }
            
            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult CreateReview(Rating rating)
        {
            
            long userId = 0;
            if (Session["UserId"] == null)
            {
                return Json(new {status = "UnAuthorized"});
            }

            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
                rating.UserId = userId;
            }
            try
            {
                if (rating.Rating1 != 0)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var json = JsonConvert.SerializeObject(rating);
                        var postTask =
                            client.PostAsync("Rating/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                       
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var ratings = GetRatingByProductUnit(rating.ProductUnitId);
                            var averageRating = ratings.Average(r => r.Rating1);
                            var ratingCount = ratings.Count;
                            var reviewDetails = ratings.Find(r => r.UserId == userId);
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                return Json(new { reviewDetails = reviewDetails, ratingCount= ratingCount, averageRating= averageRating, status = "Ok" });
                            }
                            return Json(new {reviewDetails = reviewDetails, ratingCount = ratingCount, averageRating = averageRating, status = "Created"});
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { status = "Failed" });

            }

            return Json(new {status = "Failed"});

        }

        private List<RatingVm> GetRatingByProductUnit(long productUnitId)
        {
            List<RatingVm> ratings = new List<RatingVm>();
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Rating/GetRatingByProductUnit?id=" + productUnitId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    ratings = (new JavaScriptSerializer()).Deserialize<List<RatingVm>>(readTask.Result); ;
                    ratings = ratings.OrderByDescending(r => r.RatingTime).ToList();
                }
                else
                {
                    ratings = new List<RatingVm>();
                }
            }

            return ratings;
        }

        public ProductUnit GetProductUnitByProductId(long productId)
        {
            ProductUnit product = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetProductDetails");
                var responseTask = client.GetAsync("Product/GetProductDetails?id=" + productId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    product = (new JavaScriptSerializer()).Deserialize<ProductUnit>(readTask.Result);
                }
                else
                {
                    product = new ProductUnit();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                return product;
            }
        }
    }

}