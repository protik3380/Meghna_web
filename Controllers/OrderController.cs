using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.Enum;
using EFreshStore.Models.ViewModels;
using EFreshStore.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EFreshStore.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["OrderNo"] != null)
            {
                ApiUtility.DeleteOrderForSkippedTransaction(Session["OrderNo"].ToString());
            }

            ViewBag.S = "Hello";
            ViewBag.DistrictId = Dropdown.Districts();
            Session["District"] = Dropdown.Districts();

            if (Session["UserId"] != null)
            {
                long userId = Convert.ToInt64(Session["UserId"]);
                List<CartView> cartDetails = ApiUtility.GetCartDetailsByUserId(userId);
                Session["cartView"] = cartDetails;
            }
            if (Session["cartView"] == null)
            {
                return RedirectToAction("EmptyCart");
            }

            List<CartView> cartValue = (List<CartView>)Session["cartView"];
            if (cartValue.Count != 0)
            {
                Session["DeliveryCharge"] = ApiUtility.GetValidDeliveryCharge();
                Session["DeliveryChargeAmount"] = null;
                if (Session["DeliveryCharge"] != null)
                {
                    var lpg = cartValue.FirstOrDefault(x => x.ProductTypeId == (long)ProductTypeEnum.LPG);
                    if (lpg != null)
                    {
                        Session["DeliveryChargeAmount"] = ((DeliveryCharge)Session["DeliveryCharge"]).LPGDeliveryCharge;
                    }
                    else
                    {
                        Session["DeliveryChargeAmount"] = ((DeliveryCharge)Session["DeliveryCharge"]).FMCGDeliveryCharge;
                    }
                }
                ViewBag.DistrictId = Dropdown.Districts();
                if (Session["UserId"] == null)
                {
                    return View();
                }
                long userId = Convert.ToInt64(Session["UserId"]);
                long userTypeId = Convert.ToInt64(Session["userTypeId"]);
                if (userTypeId == (long)UserTypeEnum.Corporate)
                {
                    CorporateUser user = ApiUtility.GetCorporateUserByUserId(userId);
                    ViewBag.User = user;
                    return View();
                }
                if (userTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    MeghnaUser user = ApiUtility.GetMeghnaUserByUserId(userId);
                    ViewBag.User = user;
                    return View();
                }
                Customer customer = ApiUtility.GetCustomerByUserId(userId);
                ViewBag.User = customer;
                return View();
            }
            return RedirectToAction("EmptyCart");
        }


        [HttpPost]
        public ActionResult Checkout(Order anOrder, string address1)
        {
            Session["OrderNo"] = null;
            ViewBag.DistrictId = Dropdown.Districts();
            anOrder.UpdateAddress = address1;
            bool isSaved = false;
            List<long> productIds = new List<long>();
            if (Session["cartView"] != null)
            {
                anOrder.UserId = Convert.ToInt64(Session["UserId"]);
                List<CartView> cartData = (List<CartView>)Session["cartView"];
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var order in cartData)
                {
                    long id = order.ProductUnitId;
                    productIds.Add(id);
                    orderDetails.Add(new OrderDetail
                    {
                        ProductUnitId = order.ProductUnitId,
                        Quantity = order.Quantity,
                        Price = (decimal?)order.Price,
                        PacketQuantity = order.Quantity
                    });
                }

                anOrder.OrderDetails = orderDetails;
                if (Session["UserId"] != null)
                {
                    var userId = Convert.ToInt64(Session["UserId"]);
                    anOrder.UserId = userId;
                    if (Session["couponCode"] != null) anOrder.CouponCode = Session["couponCode"].ToString();
                    if (Session["finalCouponDiscount"] != null)
                        anOrder.CouponDiscount = Convert.ToDecimal(Session["finalCouponDiscount"]);
                }
                else
                {
                    anOrder.UserId = null;
                }
                anOrder.OrderDate = DateTime.Now;
                anOrder.DeliveryCharge = Convert.ToDecimal(Session["DeliveryChargeAmount"]);
                anOrder.OrderNo = Session["OrderNo"]?.ToString();

                if (anOrder.PaymentMethod == "") return View();

                using (var client = new HttpClientDemo())
                {
                    var json = JsonConvert.SerializeObject(anOrder);
                    var postTask =
                        client.PostAsync("Order/Checkout", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var data = (new JavaScriptSerializer()).Deserialize<Order>(readTask.Result); ;
                        Session["placedOrder"] = data;
                        if (anOrder.PaymentMethod == "ONLINE")
                        {
                            try
                            {
                                Session["OrderNo"] = data.OrderNo;
                                var response = InitiateSSLCommerzTransaction(data);
                                return Redirect(response);
                            }
                            catch (Exception e)
                            {
                                var deleteTask = client.PostAsync("Order/Delete?orderNo=" + data.OrderNo, null);
                                deleteTask.Wait();
                                return RedirectToAction("TransactionInitailizationFailed");
                            }
                        }

                        if (anOrder.PaymentMethod == "COD")
                        {
                            return RedirectToAction("Confirm");
                        }


                    }
                }
            }
            return View();
        }

        private string InitiateSSLCommerzTransaction(Order order)
        {
            var grandTotal = (decimal)Session["GrandTotal"];
            var finalTotal = order.CouponDiscount != null ? Convert.ToDecimal(grandTotal - order.CouponDiscount) : grandTotal;
            finalTotal = Convert.ToDecimal(finalTotal + order.DeliveryCharge);

            NameValueCollection PostData = new NameValueCollection();
            PostData.Add("currency", "BDT");
            PostData.Add("total_amount", finalTotal.ToString());
            PostData.Add("tran_id", order.OrderNo);
            PostData.Add("success_url", BaseUrl.homeUrl + "order/confirm-order");
            PostData.Add("fail_url", BaseUrl.homeUrl + "order/transaction-failed"); // "Fail.aspx" page needs to be created
            PostData.Add("cancel_url", BaseUrl.homeUrl + "order/transaction-cancelled"); // "Cancel.aspx" page needs to be created
            PostData.Add("version", "4.00");
            PostData.Add("cus_name", order.CustomerName);
            PostData.Add("cus_email", order.Email);
            PostData.Add("cus_add1", order.DeliveryAddress);
            //PostData.Add("cus_add2", "Address Line Tw");
            PostData.Add("cus_city", "City Name");
            PostData.Add("cus_state", "State Name");
            PostData.Add("cus_postcode", "Post Code");
            PostData.Add("cus_country", "Bangladesh");
            PostData.Add("cus_phone", order.MobileNo);
            //PostData.Add("cus_fax", "0171111111");
            PostData.Add("ship_name", "testnerdcace7");
            //PostData.Add("ship_name", "testnerdcyfm3");
            PostData.Add("ship_add1", "Address Line One");
            PostData.Add("ship_add2", "Address Line Two");
            PostData.Add("ship_city", "City Name");
            PostData.Add("ship_state", "State Name");
            PostData.Add("ship_postcode", "Post Code");
            PostData.Add("ship_country", "Bangladesh");
            PostData.Add("value_a", "ref001");
            PostData.Add("value_b", "ref002");
            PostData.Add("value_c", "ref003");
            PostData.Add("value_d", "ref004");

            SSLCommerz sslcz = new SSLCommerz("nerdc5e8eede5660f8", "nerdc5e8eede5660f8@ssl", true);
            //SSLCommerz sslcz = new SSLCommerz("nerdc5e82f0d18a1fc", "nerdc5e82f0d18a1fc@ssl", true);
            String response = sslcz.InitiateTransaction(PostData);
            //Response.Redirect(response);
            return response;

        }

        public ActionResult Failed()
        {
            var request = System.Web.HttpContext.Current.Request;
            if (!string.IsNullOrEmpty(request.Form["status"]) && request.Form["status"] == "FAILED")
            {
                using (var client = new HttpClientDemo())
                {
                    var deleteTask = client.PostAsync("Order/Delete?orderNo=" + request.Form["tran_id"], null);
                    deleteTask.Wait();
                    var result = deleteTask.Result;

                }

                return RedirectToAction("Failed");
            }

            return View();
        }

        public ActionResult TransactionInitailizationFailed()
        {
            return View();
        }

        public ActionResult Cancelled()
        {
            var request = System.Web.HttpContext.Current.Request;
            if (!string.IsNullOrEmpty(request.Form["status"]) && request.Form["status"] == "CANCELLED")
            {
                using (var client = new HttpClientDemo())
                {
                    var deleteTask = client.PostAsync("Order/Delete?orderNo=" + request.Form["tran_id"], null);
                    deleteTask.Wait();
                    var result = deleteTask.Result;
                }

                return RedirectToAction("Cancelled");
            }

            return View();
        }

        private void ClearCart()
        {
            List<CartView> cartData = null;
            if (Session["UserId"] != null)
            {
                if (Session["cartView"] != null)
                {
                    cartData = (List<CartView>)Session["cartView"];
                }
                var userId = Convert.ToInt64(Session["UserId"]);
                if (Session["cartView"] == null)
                {
                    cartData = ApiUtility.GetCartDetailsByUserId(userId);
                }
                var result = new HttpResponseMessage();
                foreach (var product in cartData)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var deleteTask = client.PostAsync("Cart/Delete?userId=" + userId + "&productUnitId=" + product.ProductUnitId, null);
                        deleteTask.Wait();
                        result = deleteTask.Result;
                    }
                }
            }
        }

        public JsonResult CheckMasterDepotLink(long id)
        {
            ViewBag.DistrictId = Dropdown.Districts();
            using (var client = new HttpClientDemo())
            {
                var postTask = client.GetAsync("MasterDepot/GetMasterDepotByThanaId/" + id);
                postTask.Wait();
                var result = postTask.Result;
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = "Sorry! There is no service available for this region";
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return Json("bad request", JsonRequestBehavior.AllowGet);
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
        public void UpdateUserAddress(long userId, string address)
        {
            long userTypeId = (long)Session["userTypeId"];
            if (userTypeId == (long)UserTypeEnum.Corporate)
            {
                CorporateUser user = ApiUtility.GetCorporateUserByUserId(userId);
                ViewBag.User = user;
                user.DeliveryAddress1 = address;
                using (var client = new HttpClientDemo())
                {
                    var json = JsonConvert.SerializeObject(user);
                    var postTask =
                        client.PostAsync("Contract/Edit", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                }
            }
            if (userTypeId == (long)UserTypeEnum.Customer)
            {
                Customer customer = ApiUtility.GetCustomerByUserId(userId);
                customer.DeliveryAddress1 = address;
                using (var client = new HttpClientDemo())
                {
                    var json = JsonConvert.SerializeObject(customer);
                    var postTask =
                        client.PostAsync("customer/Edit", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                }
            }
        }
        // GET Cart Action Method
        public ActionResult Cart()
        {
            if (Session["UserId"] != null)
            {
                Session["couponCode"] = null;
                Session["finalCouponDiscount"] = null;
                Session["totalUpdatePrice"] = null;
                var userId = Convert.ToInt64(Session["UserId"]);
                List<CartView> cartViews = ApiUtility.GetCartDetailsByUserId(userId);
                if (cartViews.Count != 0)
                {
                    Session["cartView"] = cartViews;
                    return View();
                }
            }
            if (Session["cartView"] != null)
            {
                List<CartView> cartValue = (List<CartView>)Session["cartView"];
                if (cartValue.Count != 0)
                {
                    return View();
                }
                return RedirectToAction("EmptyCart");
            }
            return RedirectToAction("EmptyCart");

        }

        public ActionResult EmptyCart()
        {
            return View();
        }

        public ActionResult Confirm()
        {
            if (Session["placedOrder"] != null)
            {
                RestoreSessionData();
                ClearCart();
            }
            Order order = null;
            var request = System.Web.HttpContext.Current.Request;
            if (!string.IsNullOrEmpty(request.Form["status"]) && request.Form["status"] == "VALID")
            {
                var transactionId = request.Form["tran_id"];
                order = ApiUtility.GetOrderByOrderNo(transactionId);
                if (order == null)
                    return View(order);

                var total = order.OrderDetails.Sum(x => x.Price);
                var finalTotal = order.CouponDiscount != null ? Convert.ToDecimal(total - order.CouponDiscount) : total;

                ////AMOUNT and Currency FROM DB FOR THIS TRANSACTION
                var amount = finalTotal.ToString();
                var currency = "BDT";


                SSLCommerz sslcz = new SSLCommerz("nerdc5e8eede5660f8", "nerdc5e8eede5660f8@ssl", true);
                //SSLCommerz sslcz = new SSLCommerz("nerdc5e82f0d18a1fc", "nerdc5e82f0d18a1fc@ssl", true);
                var paymentDetail = sslcz.OrderValidate(transactionId, amount, currency, request);

                if (paymentDetail != null)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var postTask = client.PostAsJsonAsync("PaymentDetail/Create", paymentDetail);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Confirm");
                        }
                    }
                }

            }
            return View(order);
        }

        private void RestoreSessionData()
        {
            var email = Session["email"];
            var userId = Session["UserId"];
            var userTypeId = Session["UserTypeId"];
            var placedOrder = Session["placedOrder"];
            var grandTotal = Session["GrandTotal"];
            Session["email"] = email;
            Session["UserId"] = userId;
            Session["UserTypeId"] = userTypeId;
            Session["placedOrder"] = placedOrder;
            Session["GrandTotal"] = grandTotal;
            Session["couponCode"] = null;
            Session["finalCouponDiscount"] = null;
            Session["totalUpdatePrice"] = null;
            Session["cartView"] = null;
        }
        [HttpPost]
        public ActionResult Cart(List<CartView> cartData)
        {
            return View();
        }
        //delete from cart
        public JsonResult DeleteFromCart(long id)
        {
            List<CartView> cartData = (List<CartView>)Session["cartView"];
            if (cartData == null)
            {
                if (Session["UserId"] != null)
                {
                    var userId = Convert.ToInt64(Session["UserId"]);
                    cartData = ApiUtility.GetCartDetailsByUserId(userId);
                    if (cartData.Count != 0)
                    {
                        Session["cartView"] = cartData;
                    }
                }
            }

            var product = cartData.Find(c => c.ProductUnitId == id);
            cartData.Remove(product);
            if (Session["UserId"] != null)
            {
                var userId = Convert.ToInt64(Session["UserId"]);
                var result = new HttpResponseMessage();
                using (var client = new HttpClientDemo())
                {
                    var deleteTask = client.PostAsync("Cart/Delete?userId=" + userId + "&productUnitId=" + id, null);
                    deleteTask.Wait();
                    result = deleteTask.Result;

                }
            }

            if (Session["UserId"] != null)
            {
                long userId = (long)Session["UserId"];
                cartData = ApiUtility.GetCartDetailsByUserId(userId);
            }
            else
            {
                cartData = ApiUtility.GetCalculatedCartProductPrice(cartData);
            }
            Session["cartView"] = cartData;
            var lpgComboDiscount = 0.0;
            var totalLpgDiscountedProducts = cartData.Where(c => c.TotalLPGDiscount != null);
            if (totalLpgDiscountedProducts.Count() != 0)
            {
                lpgComboDiscount = (double)totalLpgDiscountedProducts.Max(x => x.TotalLPGDiscount);
            }
            var subTotal = cartData.Sum(c => c.Price);
            Session["GrandTotal"] = subTotal;
            if (Session["couponCode"] != null && Session["userTypeId"] != null)
            {

                var userTypeId = Convert.ToInt64(Session["userTypeId"]);
                var userId = Convert.ToInt64(Session["UserId"]);
                var couponCode = Session["couponCode"].ToString();
                CouponDiscountVM couponDiscountFromApi = null;
                CouponDiscountVM couponDiscount = new CouponDiscountVM
                {
                    CouponCode = couponCode,
                    UserTypeId = userTypeId,
                    GrandTotal = subTotal,
                    UserId = userId
                };

                var result = ApiUtility.CheckCouponValidityAndCalculateDiscount(couponDiscount);
                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        dynamic response = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                        return Json(new { cartData, subTotal, lpgComboDiscount, status = "BadRequest", message = response.message.ToString() });
                    }
                    return Json(new { cartData, lpgComboDiscount, hasDiscount = false, subTotal, status = "Error" });
                }
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = result.Content.ReadAsAsync<CouponDiscountVM>();
                    readTask.Wait();
                    couponDiscountFromApi = readTask.Result;
                    Session["couponCode"] = couponCode;
                    Session["finalCouponDiscount"] = couponDiscountFromApi.FinalCouponDiscount;
                    Session["totalUpdatePrice"] = couponDiscountFromApi.TotalUpdatedPrice;

                    return Json(new { cartData, status = "Ok", hasDiscount = true, subTotal, discount = couponDiscountFromApi }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { cartData, status = "Ok", subTotal, lpgComboDiscount, hasDiscount = false }, JsonRequestBehavior.AllowGet);
        }
        //delete from cart
        public JsonResult ClearAllCart()
        {
            List<CartView> cartData = (List<CartView>)Session["cartView"];

            if (Session["UserId"] != null)
            {
                var userId = Convert.ToInt64(Session["UserId"]);
                var result = new HttpResponseMessage();
                foreach (var product in cartData)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var deleteTask = client.PostAsync("Cart/Delete?userId=" + userId + "&productUnitId=" + product.ProductUnitId, null);
                        deleteTask.Wait();
                        result = deleteTask.Result;
                    }

                }
            }
            cartData = new List<CartView>();
            Session["cartView"] = cartData;
            return Json(cartData);
        }

        public JsonResult GetSubTotal()
        {
            List<CartView> cartData = (List<CartView>)Session["cartView"];
            var subTotal = cartData.Sum(c => c.Price);

            return Json(subTotal);
        }
        //Get All Order method
        public ActionResult Report()
        {
            List<Order> orders = new List<Order>();
            return View(orders);
        }

        public ActionResult MyOrders()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            long userId = Convert.ToInt64(Session["UserId"]);
            List<Order> orders = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Order/GetOrderByUserId?id=" + userId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    orders = (new JavaScriptSerializer()).Deserialize<List<Order>>(readTask.Result).ToList();
                    orders = orders.OrderByDescending(c => c.OrderDate).ToList();
                }
                else
                {
                    orders = new List<Order>();
                }
            }
            return View(orders);
        }

        public ActionResult Details(long id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            long userId = Convert.ToInt64(Session["UserId"]);
            Order order = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Order/GetOrderDetailsForUser?id=" + id + "&userId=" + userId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    order = (new JavaScriptSerializer()).Deserialize<Order>(readTask.Result); ;
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Status = "Unauthorized";
                    return View(new Order());
                }
                else
                {
                    order = new Order();
                }

            }
            //  order.DeliveryManName = 
            IEnumerable<OrderDetail> orderDetails = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Order/GetOrderDetailsByOderId?id=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    orderDetails = (new JavaScriptSerializer()).Deserialize<List<OrderDetail>>(readTask.Result);
                }
                else
                {
                    orderDetails = new List<OrderDetail>(); ;
                }
            }
            ViewBag.OrderDetails = orderDetails;
            Session["OrderState"] = Enum.GetName(typeof(OrderStatusEnum), order.OrderStateId);
            return View(order);
        }
    }
}