using EFreshStore.Models.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using EFreshStore.Models;
using EFreshStore.Utility;
using Vereyon.Web;
using System.Net.Http;
using System.Threading.Tasks;
using EFreshStore.Models.ViewModels;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using System.Web;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.Enum;
using Newtonsoft.Json;

namespace EFreshStore.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IProductManager _productManager;
        //private readonly ICategoryManager _categoryManager;
        //private readonly IBrandManager _brandManager;
        //private readonly IProductUnitManager _productUnitManager;
        //private readonly ICorporateUserManager _corporateUserManager;
        //private readonly IUserManager _userManager;

        public HomeController()
        {
            //_productManager = new ProductManager();
            //_categoryManager = new CategoryManager();
            //_brandManager = new BrandManager();
            //_productUnitManager = new ProductUnitManager();
            //_corporateUserManager = new CorporateUserManager();
            //_userManager = new UserManager();
        }

        public ActionResult Login()
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.Url != null)
            {
                var url = Request.Url.ToString().ToLower();
                if (url.Contains("changepassword") || url.Contains("login") || url.Contains("resetpassword") ||
                    url.Contains("confirm"))
                {
                    Session["requesturl"] = null;
                }
                else
                {
                      Session["requesturl"] = Request.Url.ToString();
                }
                    
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, bool rememberMe = false)
        {
            string returnUrl = (string)Session["requesturl"];
            User user = ApiUtility.ValidateUser(email, password);
            LoginCredentials token = ApiUtility.ValidateUserToken(email, password);
            Session["userToken"] = token;

            if (user != null)
            {
                if (user.UserTypeId == (long) UserTypeEnum.MasterDepotUser ||
                    user.UserTypeId == (long)UserTypeEnum.DeliveryMan)
                {
                    Session["email"] = null;
                    Session["UserId"] = null;
                    Session["UserTypeId"] = null;
                    ViewBag.Message1 = "You're not eligible to login!";
                    return View();
                }

                if (Session["cartView"] != null)
                {
                    List<CartView> cartDatas = (List<CartView>)Session["cartView"];
                    foreach (var cartData in cartDatas)
                    {
                        Cart cart = new Cart
                        {
                            UserId = user.Id,
                            ProductUnitId = cartData.ProductUnitId,
                            Quantity = cartData.Quantity
                        };
                        using (var client = new HttpClientDemo())
                        {
                            var json = JsonConvert.SerializeObject(cart);
                            var postTask =
                                client.PostAsync("Cart/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                            postTask.Wait();
                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {

                            }
                        }
                    }
                }

                if (Session["cartView"] == null)
                {
                    List<CartView> cartDetails = ApiUtility.GetCartDetailsByUserId(user.Id);
                    Session["cartView"] = cartDetails;
                }
                CheckRememberMe(email, user, token, rememberMe);
                Session["requesturl"] = null;
                if (user.UserTypeId == (long)UserTypeEnum.Admin)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "~/";
                    }
                    return Redirect(returnUrl);
                }

                if (user.UserTypeId == (long)UserTypeEnum.Customer)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "~/";
                    }
                    return Redirect(returnUrl);
                }

                if (user.UserTypeId == (long)UserTypeEnum.Corporate)
                {
                    //Session["corporateUser"] = ApiUtility.GetCorporateUserByUserId(user.Id);
                    //CorporateUser corporateUsers = (CorporateUser)Session["corporateUser"];
                    //var discount = Convert.ToDecimal(corporateUsers.CorporateContract.DiscountPercentage);
                    //decimal discountPrice = discount / 100;
                    //Session["discount"] = discount;
                    //Response.Cookies["discountPercentage"].Value = discount.ToString();
                    //Response.Cookies["discountPercentage"].Expires = DateTime.Now.AddDays(15);
                    //if (corporateUsers.CorporateContract.Validity < DateTime.Now || corporateUsers.CorporateContract.Validity == null)
                    //{
                    //    discount = 0;
                    //    discountPrice = 0;
                    //    Session["discount"] = 0;
                    //    Response.Cookies["discountPercentage"].Value = 0.ToString();
                    //    Response.Cookies["discountPercentage"].Expires = DateTime.Now.AddDays(15);
                    //}
                    //List<CartView> cartDatas = (List<CartView>)Session["cartView"];
                    //if (cartDatas != null)
                    //{


                    //    if (corporateUsers.CorporateContract.Validity < DateTime.Now || corporateUsers.CorporateContract.Validity == null)
                    //    {
                    //        discount = 0;
                    //        discountPrice = 0;
                    //        Session["discount"] = 0;
                    //        Response.Cookies["discountPercentage"].Value = 0.ToString();
                    //        Response.Cookies["discountPercentage"].Expires = DateTime.Now.AddDays(15);
                    //    }
                    //    else
                    //    {
                    //        discount = Convert.ToDecimal(corporateUsers.CorporateContract.DiscountPercentage);
                    //        Response.Cookies["discountPercentage"].Value = corporateUsers.CorporateContract.DiscountPercentage.ToString();
                    //        Response.Cookies["discountPercentage"].Expires = DateTime.Now.AddDays(15);
                    //        discountPrice = discount / 100;
                    //        Session["discount"] = discount;
                    //    }


                    //    if (string.IsNullOrEmpty(returnUrl))
                    //    {
                    //        returnUrl = "~/";
                    //    }
                    //    return Redirect(returnUrl);
                    //}
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "~/";
                    }
                    return Redirect(returnUrl);
                }

                if (user.UserTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    //Session["MeghnaUserDiscount"] = ApiUtility.GetMeghnaUserDiscount();
                    //UserDiscount userDiscount = ApiUtility.GetMeghnaUserDiscount();
                    //var discount = Convert.ToDecimal(userDiscount.DiscountPercentage);
                    //decimal discountPrice = discount / 100;
                    //Session["discount"] = discount;
                    //Response.Cookies["discountPercentage"].Value = discount.ToString();
                    //Response.Cookies["discountPercentage"].Expires = DateTime.Now.AddDays(15);
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "~/";
                    }
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            Session["email"] = null;
            Session["UserId"] = null;
            Session["UserTypeId"] = null;
            ViewBag.Message1 = "Username or password doesn't match. Please try again.";
            return View();
        }

        //[Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //Roles.DeleteCookie();
            Session["email"] = null;
            Session["UserId"] = null;
            Session["UserTypeId"] = null;
            Session["couponCode"] = null;
            Session["finalCouponDiscount"] = null;
            Session["totalUpdatePrice"] = null;
            Session["cartView"] = null;
            Session.Clear();
            Session.Abandon(); // it will clear the session at the end of request
            Response.Cookies["username"].Expires = System.DateTime.Now.AddDays(-1);
            Response.Cookies["userid"].Expires = System.DateTime.Now.AddDays(-1);
            Response.Cookies["usertypeid"].Expires = System.DateTime.Now.AddDays(-1);
            Response.Cookies["discountPercentage"].Expires = System.DateTime.Now.AddDays(-1);
            //HttpCookie myCookie = new HttpCookie("user");
            //myCookie = Request.Cookies["user"];
            //myCookie.Expires = System.DateTime.Now.AddDays(-1);
            //myCookie.Value = null;
            Request.Cookies.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                User aUser = ApiUtility.GetByUserEmail(email);
                //User aUser = _userManager.GetByUserEmail(email);

                if (aUser != null)
                {
                    Session["ID"] = aUser.Username;
                    string subject = "[Meghna e-Commerce] Reset Password";
                    string body = "Dear user" + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "Click the link below to change your password" +
                        Environment.NewLine;
                    body += ConfigurationManager.AppSettings["HomeUrl"].ToString() + "account/reset-passowrd?UserName=" + aUser.Username + Environment.NewLine;

                    MailAddress mailAddress = new MailAddress(aUser.Username, aUser.Username);
                    if (!string.IsNullOrWhiteSpace(aUser.Username))
                    {
                        Email.SendEmail(subject, body, mailAddress);
                    }
                    ViewBag.Message = "A mail has been sent to this email addres to reset password.";
                    //FlashMessage.Confirmation("A mail has been sent to this email addres to reset password.");
                    return View();

                }
                else
                {
                    ViewBag.Message = "Sorry this email address dosen't exist in the record.";
                    //FlashMessage.Warning("Sorry this email address dosen't exist in the record");
                    return View();

                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Sorry! Email can't be sent to this address.";
                FlashMessage.Warning("Sorry! Email can't be sent to this address.");
            }

            return View();
        }
        [ActionName("ResetPassword")]
        public ActionResult PasswordReset(string UserName)
        {
            Session["ID"] = UserName;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string password)
        {
            try
            {
                string userName = Session["ID"].ToString();
                //var user = _userManager.GetByUserEmail(userName);
                var user = ApiUtility.GetByUserEmail(userName);
                //long userId = Convert.ToInt64(Session["ID"]);
                //var user = _userManager.GetById(userId);
                if (user != null)
                {
                    if (password == user.Password)
                    {
                        ViewBag.Message = "Sorry! New password can't be same as old password.";
                        // FlashMessage.Warning("Sorry! New password can't be same as old password.");
                        return View();
                    }
                    user.Password = password;
                    using (var client = new HttpClientDemo())
                    {
                        client.BaseAddress = new Uri(BaseUrl.url + "User/ChangePassword");
                        var json = JsonConvert.SerializeObject(user);
                        var postTask =
                            client.PostAsync("ChangePassword", new StringContent(json, Encoding.UTF8, "application/json"));
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "Reset password successfully.";
                            FlashMessage.Confirmation("Password reset successfully.");
                            return RedirectToAction("Login", "Home");
                            //return RedirectToAction("Login", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "Sorry! Password reset failed.";
                            FlashMessage.Warning("Sorry! Password change failed.");
                            return View();
                        }
                    }
                    //var isChange = _userManager.Update(user);
                    //if (isChange)
                    //{
                    //    FlashMessage.Confirmation("Password changed successfully.");
                    //    return RedirectToAction("Login", "Home");
                    //}
                    //else
                    //{
                    //    FlashMessage.Warning("Sorry! Password change failed.");
                    //    return View();
                    //}
                }


            }
            catch (Exception ex)
            {
                FlashMessage.Warning(ex.Message);
            }
            return View();
        }
        public JsonResult IsUserLoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return Json(new { status = "Ok" });
            }
            return Json(new { status = "UnAuthorized" });
        }
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] == null)
            {
                FlashMessage.Warning("Please login to change password.");
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    FlashMessage.Warning("Please login to change password.");
                    return RedirectToAction("Login", "Home");
                }
                long userId = Convert.ToInt64(Session["UserId"]);
                //string userName = Session["email"].ToString();
                User user = new User();
                user.Id = userId;
                user.Password = password;
                //var user = ApiUtility.GetByUserEmail(userName);
                //user.Password = password;
                using (var client = new HttpClientDemo())
                {
                    client.BaseAddress = new Uri(BaseUrl.url + "User/ChangePassword");
                    var json = JsonConvert.SerializeObject(user);
                    var postTask =
                        client.PostAsync("ChangePassword", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Password changed successfully.";
                        FlashMessage.Confirmation("Password changed successfully.Please login to continue");
                        //return View();
                        FormsAuthentication.SignOut();
                        //Roles.DeleteCookie();
                        Session["email"] = null;
                        Session["UserId"] = null;
                        Session["UserTypeId"] = null;
                        Session.Clear();
                        Session.Abandon(); // it will clear the session at the end of request
                        Response.Cookies["username"].Expires = System.DateTime.Now.AddDays(-1);
                        Response.Cookies["userid"].Expires = System.DateTime.Now.AddDays(-1);
                        Response.Cookies["usertypeid"].Expires = System.DateTime.Now.AddDays(-1);
                        Response.Cookies["discountPercentage"].Expires = System.DateTime.Now.AddDays(-1);
                        //HttpCookie myCookie = new HttpCookie("user");
                        //myCookie = Request.Cookies["user"];
                        //myCookie.Expires = System.DateTime.Now.AddDays(-1);
                        //myCookie.Value = null;
                        Request.Cookies.Clear();
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Sorry! Password change failed.";
                        FlashMessage.Warning("Sorry! Password change failed.");
                        return View();
                    }
                }

            }
            catch (Exception ex)
            {
                FlashMessage.Warning(ex.Message);
            }
            return View();
        }
        public async Task<ActionResult> Index(long? brandId, long? categoryId, string searchString)
        {
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Session["UserId"] != null)
            {
                if (Session["cartView"] == null)
                {
                    long userId = Convert.ToInt64(Session["UserId"]);
                    List<CartView> cartDetails = ApiUtility.GetCartDetailsByUserId(userId);
                    Session["cartView"] = cartDetails;
                }
            }

            //Session["couponCode"] = null;
            //Session["finalCouponDiscount"] = null;
            //Session["totalUpdatePrice"] = null;

            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
                var y = Session["discount"];
            }
            List<ProductUnit> products;
            Session["Category"] = Dropdown.Categories();
            Session["Brand"] = Dropdown.Brands();
            //List<Product> products = _productManager.GetAll();
            if (brandId != null)
            {

                Session["MegaMenuActive"] = "brand";
                Session["BrandActive"] = brandId;
                products = this.products().Where(c => c.Product.BrandId == brandId).ToList();
                Session["productList"] = products;
                //products = _productUnitManager.GetByBrand(brandId.Value).ToList();
            }
            else
            {
                Session["BrandActive"] = null;
                //products = _productUnitManager.GetAll().ToList();
                if (categoryId != null)
                {
                    Session["MegaMenuActive"] = "category";
                    products = this.products().Where(c => c.Product.CategoryId == categoryId).ToList();
                    Session["productList"] = products;
                    //products = _productUnitManager.GetByCategory(categoryId.Value).ToList();
                }
                else
                {
                    if (searchString != null)
                    {

                        products = this.products().Where(c => c.Product.Brand.Name.ToLower().Contains(searchString.ToLower())
                         || c.Product.Category.Name.ToLower().Contains(searchString.ToLower())
                         || c.Product.Name.ToLower().Contains(searchString.ToLower())
                         || c.Product.Description.ToLower().Contains(searchString.ToLower())).ToList();
                        // products = _productUnitManager.GetByCategory(categoryId.Value).ToList();
                        Session["productList"] = products;
                        Session["MegaMenuActive"] = "search";
                    }
                    else
                    {
                        Session["MegaMenuActive"] = "empty";
                        products = this.products();
                        Session["productList"] = products;
                    }
                }

            }
            //var userTypeId = Convert.ToInt64(Session["UserTypeId"]);

            //foreach (var product in products)
            //{
            //    var ratings = ApiUtility.GetRatingByProductUnit(product.Id);
            //    product.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Rating1) : 0;
            //    product.Ratings = ratings;
            //}
            return View(products);
            //return Json(products, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Search(long? brandId, long? categoryId, string q)
        {
            ViewBag.SearchTerm = q;
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
                var y = Session["discount"];
            }
            List<ProductUnit> products;
            Session["Category"] = Dropdown.Categories();
            Session["Brand"] = Dropdown.Brands();
            //List<Product> products = _productManager.GetAll();
            if (brandId != null)
            {
                Session["MegaMenuActive"] = "brand";
                products = this.products().Where(c => c.Product.BrandId == brandId).ToList();
                //products = _productUnitManager.GetByBrand(brandId.Value).ToList();
            }
            else
            {
                //products = _productUnitManager.GetAll().ToList();
                if (categoryId != null)
                {
                    Session["MegaMenuActive"] = "category";
                    products = this.products().Where(c => c.Product.CategoryId == categoryId).ToList();
                    //products = _productUnitManager.GetByCategory(categoryId.Value).ToList();
                }
                else
                {
                    if (q != null)
                    {
                        Session["MegaMenuActive"] = "search";
                        products = this.products().Where(c => c.Product.Brand.Name.ToLower().Contains(q.ToLower())
                         || c.Product.Category.Name.ToLower().Contains(q.ToLower())
                         || c.Product.Name.ToLower().Contains(q.ToLower())
                         || c.Product.Description.ToLower().Contains(q.ToLower())).ToList();
                        // products = _productUnitManager.GetByCategory(categoryId.Value).ToList();
                    }
                    else
                    {
                        Session["MegaMenuActive"] = "empty";
                        products = new List<ProductUnit>();
                    }
                }

            }

            //foreach (var product in products)
            //{
            //    var ratings = ApiUtility.GetRatingByProductUnit(product.Id);
            //    product.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Rating1) : 0;
            //}
            return View(products);

        }
        public ActionResult SearchProduct(string brandIds, string categoryIds, string q)
        {
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
            }

            List<ProductUnit> products = null;
            List<ProductUnit> productsByBrand=null;
            List<ProductUnit> productsByCategory = null;
            
            Session["Category"] = Dropdown.Categories();
            Session["Brand"] = Dropdown.Brands();
            long? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }
            string apiUrl = BaseUrl.url;
            WebClient client = new WebClient();
            client.Headers.Add("Accept: text/html, application/xhtml+xml, application/pdf, */*");
            client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            client.Headers.Add("Accept-Encoding: gzip, deflate, br");
            client.Headers.Add("Accept-Language: en-US,en;q=0.9");
            client.Headers.Add("Cache-Control: no-cache");
            client.Headers.Add("Upgrade-Insecure-Requests: 1");
            client.Headers["Content-type"] = "application/json";
            client.UseDefaultCredentials = true;
            client.Encoding = Encoding.UTF8;

            var brandIdList = new List<long>();
            var categoryIdList = new List<long>();

            string brandParameter = "";
            string categoryParameter = "";


            if (!string.IsNullOrEmpty(brandIds) && !string.IsNullOrEmpty(categoryIds))
            {
                for (int i = 0; i < brandIds.Split(',').Length; i++)
                {
                    brandIdList.Add(Convert.ToInt64(brandIds.Split(',')[i]));
                    if (i == brandIds.Split(',').Length - 1)
                    {
                        brandParameter += "brandIds=" + brandIdList[i];
                    }
                    else
                    {
                        brandParameter += "brandIds=" + brandIdList[i] + "&";
                    }
                }
                for (int i = 0; i < categoryIds.Split(',').Length; i++)
                {
                    categoryIdList.Add(Convert.ToInt64(categoryIds.Split(',')[i]));
                    if (i == categoryIds.Split(',').Length - 1)
                    {
                        categoryParameter += "categoryIds=" + categoryIdList[i];
                    }
                    else
                    {
                        categoryParameter += "categoryIds=" + categoryIdList[i] + "&";
                    }
                }
                string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByBrandIdsAndCategoryIds?userId=" + userId+"&"+brandParameter+"&"+categoryParameter));
                List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                products = productList.ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(brandIds))
                {

                    for (int i = 0; i < brandIds.Split(',').Length; i++)
                    {
                        brandIdList.Add(Convert.ToInt64(brandIds.Split(',')[i]));
                        if (i == brandIds.Split(',').Length - 1)
                        {
                            brandParameter += "brandIds=" + brandIdList[i];
                        }
                        else
                        {
                            brandParameter += "brandIds=" + brandIdList[i] + "&";
                        }
                    }
                    string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByBrandIds?userId=" + userId+"&"+brandParameter));
                    List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                    products = productList.ToList();
                    Session["MegaMenuActive"] = "brand";
                }

                else if (!string.IsNullOrEmpty(categoryIds))
                {

                    for (int i = 0; i < categoryIds.Split(',').Length; i++)
                    {
                        categoryIdList.Add(Convert.ToInt64(categoryIds.Split(',')[i]));
                        if (i == categoryIds.Split(',').Length - 1)
                        {
                            categoryParameter += "categoryIds=" + categoryIdList[i];
                        }
                        else
                        {
                            categoryParameter += "categoryIds=" + categoryIdList[i] + "&";
                        }
                    }
                    string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByCategoryIds?userId=" + userId+"&"+categoryParameter));
                    List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                    products = productList.ToList();
                    Session["MegaMenuActive"] = "category";
                }
                else
                {
                    Session["MegaMenuActive"] = "empty";
                    products = new List<ProductUnit>();
                }
            }

            if (!string.IsNullOrEmpty(q))
            {
                Session["MegaMenuActive"] = "search";
                products = products.Where(c => c.Product.Brand.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Category.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Description.ToLower().Contains(q.ToLower())).ToList();
            }
            //if (products==null)
            //{
            //    Session["MegaMenuActive"] = "empty";
            //    products = new List<ProductUnit>();
            //}
            //if (productsByBrand!=null && productsByCategory!=null)
            //{
            //    products = productsByBrand.Where(c => productsByCategory.Any(y => y.Id == c.Id)).ToList();
            //}            
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterProduct(string brands, string categories, string q)
        {
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
            }

            List<ProductUnit> products = null;
            List<ProductUnit> productsByBrand = null;
            List<ProductUnit> productsByCategory = null;

            Session["Category"] = Dropdown.Categories();
            Session["Brand"] = Dropdown.Brands();
            long? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }
            string apiUrl = BaseUrl.url;
            WebClient client = new WebClient();
            client.Headers.Add("Accept: text/html, application/xhtml+xml, application/pdf, */*");
            client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            client.Headers.Add("Accept-Encoding: gzip, deflate, br");
            client.Headers.Add("Accept-Language: en-US,en;q=0.9");
            client.Headers.Add("Cache-Control: no-cache");
            client.Headers.Add("Upgrade-Insecure-Requests: 1");
            client.Headers["Content-type"] = "application/json";
            client.UseDefaultCredentials = true;
            client.Encoding = Encoding.UTF8;

            var brandIdList = new List<long>();
            var categoryIdList = new List<long>();

            string brandParameter = "";
            string categoryParameter = "";


            if (!string.IsNullOrEmpty(brands) && !string.IsNullOrEmpty(categories))
            {
                for (int i = 0; i < brands.Split(',').Length; i++)
                {
                    brandIdList.Add(Convert.ToInt64(brands.Split(',')[i]));
                    if (i == brands.Split(',').Length - 1)
                    {
                        brandParameter += "brandIds=" + brandIdList[i];
                    }
                    else
                    {
                        brandParameter += "brandIds=" + brandIdList[i] + "&";
                    }
                }
                for (int i = 0; i < categories.Split(',').Length; i++)
                {
                    categoryIdList.Add(Convert.ToInt64(categories.Split(',')[i]));
                    if (i == categories.Split(',').Length - 1)
                    {
                        categoryParameter += "categoryIds=" + categoryIdList[i];
                    }
                    else
                    {
                        categoryParameter += "categoryIds=" + categoryIdList[i] + "&";
                    }
                }
                string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByBrandIdsAndCategoryIds?userId=" + userId + "&" + brandParameter + "&" + categoryParameter));
                List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                products = productList.ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(brands))
                {
                    var brandIds = brands.Split(',');
                    for (int i = 0; i < brandIds.Length; i++)
                    {
                        brandIdList.Add(Convert.ToInt64(brandIds[i]));
                        if (i == brandIds.Length - 1)
                        {
                            brandParameter += "brandIds=" + brandIdList[i];
                        }
                        else
                        {
                            brandParameter += "brandIds=" + brandIdList[i] + "&";
                        }
                    }
                    string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByBrandIds?userId=" + userId + "&" + brandParameter));
                    List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                    products = productList.ToList();
                    Session["MegaMenuActive"] = "brand";
                }

                else if (!string.IsNullOrEmpty(categories))
                {

                    for (int i = 0; i < categories.Split(',').Length; i++)
                    {
                        categoryIdList.Add(Convert.ToInt64(categories.Split(',')[i]));
                        if (i == categories.Split(',').Length - 1)
                        {
                            categoryParameter += "categoryIds=" + categoryIdList[i];
                        }
                        else
                        {
                            categoryParameter += "categoryIds=" + categoryIdList[i] + "&";
                        }
                    }
                    string json = client.DownloadString(new Uri(apiUrl + "Product/GetAllProductsByCategoryIds?userId=" + userId + "&" + categoryParameter));
                    List<ProductUnit> productList = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
                    products = productList.ToList();
                    Session["MegaMenuActive"] = "category";
                }
                else
                {
                    Session["MegaMenuActive"] = "empty";
                    products = new List<ProductUnit>();
                }
            }

            if (!string.IsNullOrEmpty(q))
            {
                Session["MegaMenuActive"] = "search";
                products = products.Where(c => c.Product.Brand.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Category.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Name.ToLower().Contains(q.ToLower())
                                                      || c.Product.Description.ToLower().Contains(q.ToLower())).ToList();
            }

            ViewBag.BrandIds = brandIdList;
            ViewBag.CategoryIds = categoryIdList;
            //if (products==null)
            //{
            //    Session["MegaMenuActive"] = "empty";
            //    products = new List<ProductUnit>();
            //}
            //if (productsByBrand!=null && productsByCategory!=null)
            //{
            //    products = productsByBrand.Where(c => productsByCategory.Any(y => y.Id == c.Id)).ToList();
            //}            
            return View("Search",products);
        }

        public async Task<ActionResult> SearchBrandWise(long brandId)
        {
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
                var y = Session["discount"];
            }
            List<ProductUnit> productList;
            if (brandId != null && brandId > 0)
            {

                productList = this.products().Where(c => c.Product.Brand.Id == brandId).ToList();
            }
            else
            {
                productList = this.products();
            }



            //foreach (var product in productList)
            //{
            //    var ratings = ApiUtility.GetRatingByProductUnit(product.Id);
            //    product.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Rating1) : 0;

            //}

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult autoFillSearchBox(string searchString)
        {
            //Note : you can bind same list from database  
            List<ProductUnit> products;

            List<string> lists = new List<string>();
            //Searching records from list using LINQ query  
            if (searchString != null)
            {
                products = this.products().Where(c => c.Product.Brand.Name.ToLower().Contains(searchString.ToLower())).ToList();
                //|| c.Product.Category.Name.ToLower().Contains(searchString.ToLower())
                //|| c.Product.Name.ToLower().Contains(searchString.ToLower())).ToList();
                // products = _productUnitManager.GetByCategory(categoryId.Value).ToList();
                List<Brand> brands = Dropdown.Brands().Where(b => b.Name.ToLower().Contains(searchString.ToLower())).ToList();
                List<Category> categories = Dropdown.Categories().Where(b => b.Name.ToLower().Contains(searchString.ToLower())).ToList();
                //lists = ;
                lists.AddRange(categories.Select(category => category.Name));
                lists.AddRange(brands.Select(brand => brand.Name));
                lists.AddRange(products.Select(product => product.Product.Name));
            }
            else
            {
                products = this.products();
                lists.AddRange(products.Select(product => product.Product.Name));
            }
            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoCompleteResult(string query)
        {
            List<ProductUnit> products = new List<ProductUnit>();
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Product/GetAllProducts?searchString=" + query);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    products = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(readTask.Result);
                }
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckoutPartial(string radioBtnVal)
        {
            ViewBag.DistrictId = Dropdown.Districts();
            Session["guest"] = null;
            if (radioBtnVal == "guest")
            {
                Session["guest"] = true;
                Session["District"] = Dropdown.Districts();
                return RedirectToAction("Checkout", "Order");
            }
            //return RedirectToAction("Create", "Customer");
            return View("~/Views/Customer/Create.cshtml");
        }
        public JsonResult QuickView(long prodUnitId)
        {
            var isLogin = false;
            if (Request.Cookies["username"] != null)
            {
                Session["email"] = Request.Cookies["username"].Value;
                var x = Session["email"];
                isLogin = true;
            }
            if (Request.Cookies["usertypeid"] != null)
                Session["UserTypeId"] = Request.Cookies["usertypeid"].Value;
            if (Request.Cookies["userid"] != null)
                Session["UserId"] = Request.Cookies["userid"].Value;
            if (Request.Cookies["discountPercentage"] != null)
            {
                Session["discount"] = Request.Cookies["discountPercentage"].Value;
                var y = Session["discount"];
            }

            long? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }

            ProductUnit productDetails = new ProductUnit();
            try
            {
                productDetails = ApiUtility.GetProductUnitByProductId(prodUnitId, userId);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(productDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ContactUs()
        {
            Session["MegaMenuActive"] = "contact";
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUs aContactUs)
        {
            Session["MegaMenuActive"] = "contact";
            string body = "Review from a customer: \nName: " + aContactUs.Name + "\nContact No: " + aContactUs.MobileNo + "\nThe message is given below:\n\n" + aContactUs.Message;
            MailAddress mail = new MailAddress("meghna.ecommerce@gmail.com");

            Email.SendEmail(aContactUs.Subject, body, mail);
            ViewBag.Message = "Email has been sent successfully.";
            return View();
        }
        [HttpGet]
        public ActionResult TrackOrder(string orderNo)
        {
            Session["MegaMenuActive"] = "Track Order";
            TrackOrderVm order = null;
            Session["OrderPass"] = "";
            if (orderNo != null)
            {
                using (var client = new HttpClientDemo())
                {
                    var responseTask = client.GetAsync("Order/GetOrderDetailsByOderNo?orderNo=" + orderNo);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<TrackOrderVm>();
                        readTask.Wait();
                        order = readTask.Result;
                    }
                    else
                    {
                        order = new TrackOrderVm();
                    }
                }
                if (order.CurrentOrderStateId != null)
                {
                    Session["OrderPass"] = Enum.GetName(typeof(OrderStatusEnum), order.CurrentOrderStateId);
                }
            }
            ViewBag.TextBoxValue = orderNo;
            ViewBag.Orders = order;
            return View();
        }
        public async Task<List<ProductUnit>> GetAllProductUnits()
        {
            IEnumerable<ProductUnit> products;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(BaseUrl.url + "Product/GetAllProducts");
                var responseTask = await client.GetAsync("GetAllProducts");
                //responseTask.Wait();

                var result = responseTask.Content;
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = result.ReadAsStringAsync();
                    readTask.Wait();
                    products = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(readTask.Result); ;
                }
                else
                {
                    products = Enumerable.Empty<ProductUnit>();
                }
            }

            return products.ToList();
        }

        public List<ProductUnit> products()
        {
            long? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }

            string apiUrl = BaseUrl.url;

            //string inputJson = (new JavaScriptSerializer()).Serialize(input);
            WebClient client = new WebClient();
            client.Headers.Add("Accept: text/html, application/xhtml+xml, application/pdf, */*");
            client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            client.Headers.Add("Accept-Encoding: gzip, deflate, br");
            client.Headers.Add("Accept-Language: en-US,en;q=0.9");
            client.Headers.Add("Cache-Control: no-cache");
            client.Headers.Add("Upgrade-Insecure-Requests: 1");
            client.Headers["Content-type"] = "application/json";
            client.UseDefaultCredentials = true;
            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(new Uri(apiUrl + "/Product/GetAllProductsByUserId?userId=" + userId));
            List<ProductUnit> pro = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(json);
            return pro.ToList();
        }


        public void CheckRememberMe(string email, User user, LoginCredentials token, bool rememberMe)
        {
            if (rememberMe)
            {
                Session["email"] = email;
                Session["UserId"] = user.Id;
                Session["UserTypeId"] = user.UserTypeId;
                Response.Cookies["username"].Value = email;
                Response.Cookies["usertypeid"].Value = user.UserTypeId.ToString();
                Response.Cookies["userid"].Value = user.Id.ToString();
                Response.Cookies["username"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["usertypeid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["AccessToken"].Value = token.AccessToken;
                Response.Cookies["TokenType"].Value = token.TokenType;
                Response.Cookies["RefreshToken"].Value = token.RefreshToken;
                Response.Cookies["Error"].Value = token.Error;
                Response.Cookies["ExpiresIn"].Value = token.ExpiresIn.ToString();

            }
            else
            {
                Session["email"] = email;
                Session["UserId"] = user.Id;
                Session["UserTypeId"] = user.UserTypeId;
            }
        }

        public ActionResult Faq()
        {
            Session["MegaMenuActive"] = "faq";
            IEnumerable<FAQ> faqs;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(BaseUrl.url + "FAQ/GetAllActive");
                var responseTask =  client.GetAsync("GetAllActive");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FAQ>>();
                    readTask.Wait();
                    faqs = readTask.Result;
                }
                else
                {
                    faqs = Enumerable.Empty<FAQ>();
                }
            }
            return View(faqs.ToList());
        }
    }
}