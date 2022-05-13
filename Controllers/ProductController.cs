using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStore.Models.Context;
using System.Web.Mvc;
using EFreshStore.Models;
using EFreshStore.Models.ViewModels;
using System.IO;
using System.Net;
using Vereyon.Web;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.Enum;
using EFreshStore.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EFreshStore.Controllers
{
    public class ProductController : Controller
    {
        private List<CartView> _cartViews;
        public ProductController()
        {
            _cartViews = new List<CartView>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(long productId)
        {

            long? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt64(Session["UserId"]);
            }

            try
            {
                if(productId <= 0)
                {
                    return RedirectToAction("Index","Home");
                }

                ProductUnit productDetails = ApiUtility.GetProductUnitByProductId(productId, userId);
                if (productDetails == null)
                {
                    return View();
                }
                return View(productDetails);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        
        public JsonResult GetCartData(OrderDetail details, string image, string discountPrice)
        {

            var LpgComboDiscount = 0.0;
            if (details.ProductUnitId == null)
            {
                if (Session["cartView"] != null)
                {
                    _cartViews = (List<CartView>)Session["cartView"];
                }
                
                if (Session["UserId"] != null)
                {
                    var userId = Convert.ToInt64(Session["UserId"]);
                    _cartViews = ApiUtility.GetCartDetailsByUserId(userId);
                }
                Session["cartView"] = _cartViews;
                var LpgDiscountedProducts = _cartViews.Where(c => c.TotalLPGDiscount != null);
                if (LpgDiscountedProducts.Count() != 0)
                {
                    LpgComboDiscount = (double)LpgDiscountedProducts.Max(x => x.TotalLPGDiscount);
                }
                if (Session["totalUpdatePrice"] != null)
                {
                    var totalUpdatedPrice = Convert.ToDouble(Session["totalUpdatePrice"]);
                    return Json(new { cartViews = _cartViews, hasDiscount = true, totalUpdatedPrice, lpgComboDiscount = LpgComboDiscount }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { cartViews = _cartViews, hasDiscount = false, lpgComboDiscount = LpgComboDiscount }, JsonRequestBehavior.AllowGet);
            }


            Session["couponCode"] = null;
            Session["finalCouponDiscount"] = null;
            Session["totalUpdatePrice"] = null;
            decimal discountP = Convert.ToDecimal(discountPrice);
            if (discountP == 0)
            {
                details.DiscountPrice = details.Price;
            }
            else
            {
                details.DiscountPrice = Convert.ToDecimal(discountPrice);

            }
            int count = 0;
            if (Session["cartView"] != null)
            {
                _cartViews = (List<CartView>)Session["cartView"];
                var productToAdd = _cartViews.Find(p => p.ProductUnitId == details.ProductUnitId);
                if (productToAdd != null)
                {
                    productToAdd.Quantity += (int)details.Quantity;
                    productToAdd.UnitPrice += (double)details.Price;
                    productToAdd.Price += (double)details.DiscountPrice;
                    productToAdd.DistributorPrice += (double)details.DistributorPrice;
                    productToAdd.Brand = details.BrandName;
                    productToAdd.Category = details.CategoryName;
                    productToAdd.ProductUnit = details.Unit;
                }
                if (productToAdd == null)
                {
                    _cartViews.Add(new CartView
                    {
                        ProductUnitId = (long)details.ProductUnitId,
                        ProductTypeId = (long)details.ProductTypeId,
                        ProductName = details.ProductUnit.Product.Name,
                        Quantity = (int)details.Quantity,
                        UnitPrice = (double)details.Price,
                        Price = (double)details.DiscountPrice,
                        DistributorPrice = details.DistributorPrice,
                        Category = details.CategoryName,
                        Brand = details.BrandName,
                        ProductUnit = details.Unit,
                        ProductImage = image
                    });
                }

                count = _cartViews.Count;
            }
            else
            {
                if (details != null)
                {
                    _cartViews.Add(new CartView
                    {
                        ProductUnitId = (long)details.ProductUnitId,
                        ProductTypeId = (long)details.ProductTypeId,
                        ProductName = details.ProductUnit.Product.Name,
                        Quantity = (int)details.Quantity,
                        UnitPrice = (double)details.Price,
                        Price = (double)details.DiscountPrice,
                        DistributorPrice = details.DistributorPrice,
                        Category = details.CategoryName,
                        Brand = details.BrandName,
                        ProductUnit = details.Unit,
                        ProductImage = image
                    });
                    count = _cartViews.Count;
                }
            }

            if (Session["UserId"] != null)
            {
                var userId = Convert.ToInt64(Session["UserId"]);
                Cart cart = new Cart
                {
                    UserId = userId,
                    ProductUnitId = (long) details.ProductUnitId,
                    Quantity = (int) details.Quantity
                };
                List<CartView> cartDetails = ApiUtility.GetCartDetailsByUserId(userId);
                var productToAdd = cartDetails.Find(p => p.ProductUnitId == details.ProductUnitId);
                if (productToAdd != null)
                {
                    cart.Quantity += productToAdd.Quantity;
                }

                using (var client = new HttpClientDemo())
                {
                   var json = JsonConvert.SerializeObject(cart);
                    var postTask =
                        client.PostAsync("Cart/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        _cartViews = new JavaScriptSerializer().Deserialize<List<CartView>>(readTask.Result);
                        _cartViews = _cartViews.OrderBy(p => p.ProductName).ToList();
                    }
                    else
                    {
                        _cartViews = new List<CartView>();
                    }
                }
            }
            else
            {
                _cartViews = ApiUtility.GetCalculatedCartProductPrice(_cartViews);
            }

            Session["cartView"] = _cartViews;
            Session["cartCount"] = count;


           
           var cartProductsWithoutLPG = _cartViews.Where(c=>c.TotalLPGDiscount != null);
           if (cartProductsWithoutLPG.Count()!= 0)
           {
               LpgComboDiscount = (double) cartProductsWithoutLPG.Max(x=>x.TotalLPGDiscount);
            }
            return Json(new { cartViews = _cartViews, hasDiscount = false,lpgComboDiscount=LpgComboDiscount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAllToCartFromWishList()
        {

            Session["couponCode"] = null;
            Session["finalCouponDiscount"] = null;
            Session["totalUpdatePrice"] = null;

            IEnumerable<WishListViewModel> wishList = GetWishListByUserId();
            OrderDetail details = new OrderDetail();
            foreach (var product in wishList)
            {
                decimal discountP = Convert.ToDecimal(product.Price);
                if (discountP == 0)
                {
                    details.DiscountPrice = product.UnitPrice;
                }
                else
                {
                    details.DiscountPrice = Convert.ToDecimal(product.Price);

                }
                int count = 0;
                if (Session["cartView"] != null)
                {
                    _cartViews = (List<CartView>)Session["cartView"];
                    var productToAdd = _cartViews.Find(p => p.ProductUnitId == product.ProductUnitId);
                    if (productToAdd != null)
                    {
                        productToAdd.Quantity += 1;
                        productToAdd.UnitPrice += (double) product.UnitPrice;
                        productToAdd.Price += (double)details.DiscountPrice;
                        productToAdd.DistributorPrice += (double)product.DistributorPrice;
                        productToAdd.Brand = product.Brand;
                        productToAdd.Category = product.Category;
                        productToAdd.ProductUnit = product.ProductUnit;
                    }
                    if (productToAdd == null)
                    {
                        _cartViews.Add(new CartView
                        {
                            ProductUnitId = (long)product.ProductUnitId,
                            ProductTypeId = (long)details.ProductTypeId,
                            ProductName = product.ProductName,
                            Quantity = 1,
                            UnitPrice = (double)product.UnitPrice,
                            Price = (double)details.DiscountPrice,
                            DistributorPrice = (double)product.DistributorPrice,
                            Category = product.Category,
                            Brand = product.Brand,
                            ProductUnit = product.ProductUnit,
                            ProductImage = product.ProductImage
                        });
                    }

                    count = _cartViews.Count;
                }
                else
                {
                    if (details != null)
                    {
                        _cartViews.Add(new CartView
                        {
                            ProductUnitId = (long)product.ProductUnitId,
                            ProductTypeId = (long)product.ProductTypeId,
                            ProductName = product.ProductName,
                            Quantity = 1,
                            UnitPrice = (double)product.UnitPrice,
                            Price = (double)details.DiscountPrice,
                            DistributorPrice = (double)product.DistributorPrice,
                            Category = product.Category,
                            Brand = product.Brand,
                            ProductUnit = product.ProductUnit,
                            ProductImage = product.ProductImage
                        });
                        count = _cartViews.Count;
                    }
                }

                if (Session["UserId"] != null)
                {
                    var userId = Convert.ToInt64(Session["UserId"]);
                    Cart cart = new Cart
                    {
                        UserId = userId,
                        ProductUnitId = (long)product.ProductUnitId,
                        Quantity = 1
                    };
                    List<CartView> cartDetails = ApiUtility.GetCartDetailsByUserId(userId);
                    var productToAdd = cartDetails.Find(p => p.ProductUnitId == product.ProductUnitId);
                    if (productToAdd != null)
                    {
                        cart.Quantity += productToAdd.Quantity;
                    }

                    using (var client = new HttpClientDemo())
                    {
                        var json = JsonConvert.SerializeObject(cart);
                        var postTask =
                            client.PostAsync("Cart/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                        postTask.Wait();
                        var result = postTask.Result;
                        if (!result.IsSuccessStatusCode)
                        {
                            return Json(new { status = "Failed" });
                        }
                    }

                    if (Session["cartView"] == null)
                    {
                        _cartViews = cartDetails;
                    }
                }

                Session["cartView"] = _cartViews;
                Session["cartCount"] = count;

                //using (var client = new HttpClientDemo())
                //{
                //    var deleteTask = client.PostAsync("WishList/Delete?id=" + product.WishListId, null);
                //    deleteTask.Wait();
                //    var result = deleteTask.Result;

                //    if (result.StatusCode == HttpStatusCode.BadRequest)
                //    {
                //        return Json(new { status = "Failed" });
                //    }
                //}
            }

            return Json(new {status = "Ok"});

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
       

        [HttpPost]
        public JsonResult UpdateQuantity(int quantity, int unitId, double price)
        {
            
            if (Session["cartView"] != null)
            {
                _cartViews = (List<CartView>)Session["cartView"];
                foreach (var order in _cartViews)
                {
                    if (order.ProductUnitId == unitId)
                    {
                        //long userTypeId = 0;
                        //var unitPrice = order.UnitPrice / order.Quantity;
                        //var unitPriceWithDiscount = order.Price / order.Quantity;
                        //var unitPriceWithDistributorPrice = order.DistributorPrice / order.Quantity;
                        //order.Quantity = quantity;
                        //order.UnitPrice = unitPrice * quantity;
                        //if (userTypeId == (long)UserTypeEnum.MeghnaUser)
                        //{
                        //    order.Price = unitPriceWithDistributorPrice * quantity;
                        //}
                        //else
                        //{
                        //    order.Price = unitPriceWithDiscount * quantity;
                        //}

                        //order.DistributorPrice = unitPriceWithDistributorPrice  * quantity;
                        order.Quantity = quantity;

                    }

                    if (Session["UserId"] != null)
                    {
                        var userId = Convert.ToInt64(Session["UserId"]);
                        Cart cart = new Cart
                        {
                            UserId = userId,
                            ProductUnitId = unitId,
                            Quantity = quantity
                        };

                        using (var client = new HttpClientDemo())
                        {
                            var json = JsonConvert.SerializeObject(cart);
                            var postTask =
                                client.PostAsync("Cart/Create", new StringContent(json, Encoding.UTF8, "application/json"));
                            postTask.Wait();
                            var result = postTask.Result;
                        }
                    }
                }
                if (Session["UserId"] != null)
                {
                    long userId = (long)Session["UserId"];
                    _cartViews = ApiUtility.GetCartDetailsByUserId(userId);
                }
                else
                {
                    _cartViews = ApiUtility.GetCalculatedCartProductPrice(_cartViews);
                }
                
                var lpgComboDiscount = 0.0;
                var totalLpgDiscountedProducts = _cartViews.Where(c => c.TotalLPGDiscount != null);
                if (totalLpgDiscountedProducts.Count() != 0)
                {
                    lpgComboDiscount = (double) totalLpgDiscountedProducts.Max(x => x.TotalLPGDiscount);
                }
                Session["cartView"] = _cartViews;
                List<CartView> cartData = (List<CartView>)Session["cartView"];
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
                            return Json(new { cartData, subTotal, status = "BadRequest", message = response.message.ToString() });
                        }
                        return Json(new { cartData, hasDiscount = true, subTotal, status = "Error" });
                    }

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        couponDiscountFromApi = (new JavaScriptSerializer()).Deserialize<CouponDiscountVM>(readTask.Result);
                        Session["couponCode"] = couponCode;
                        Session["finalCouponDiscount"] = couponDiscountFromApi.FinalCouponDiscount;
                        Session["totalUpdatePrice"] = couponDiscountFromApi.TotalUpdatedPrice;
                        return Json(new { cartData, hasDiscount = true, subTotal, discount = couponDiscountFromApi, status="Ok"});
                    }
                }

                Session["couponCode"] = null;
                Session["finalCouponDiscount"] = null;
                Session["totalUpdatePrice"] = null;

                return Json(new { cartData,hasDiscount = false, subTotal, status = "Ok" });
            }
            return Json(new { hasDiscount = false, subTotal=0, status = "Error" });
        }

        public ActionResult Add()
        {
            IEnumerable<Category> categories = Dropdown.Categories();
            IEnumerable<Brand> brands = Dropdown.Brands();
            //using (var client = new HttpClientDemo())
            //{

            //    //client.BaseAddress = new Uri(BaseUrl.url + "Category/GetAll");
            //    var responseTask = client.GetAsync("Category/GetAll");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<Category>>();
            //        readTask.Wait();
            //        categories = readTask.Result;
            //    }
            //    else
            //    {
            //        categories = new List<Category>();
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            //using (var client = new HttpClientDemo())
            //{
            //    //client.BaseAddress = new Uri(BaseUrl.url + "Brand/GetAll");
            //    var responseTask = client.GetAsync("Brand/GetAll");
            //    responseTask.Wait();

            //   var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<Brand>>();
            //        readTask.Wait();
            //        brands = readTask.Result;
            //    }
            //    else
            //    {
            //        brands = new List<Brand>();
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            
            ViewBag.category = categories;
            ViewBag.brand = brands;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product aProduct)
        {
            ViewBag.category = Dropdown.Categories();
            ViewBag.brand = Dropdown.Brands();            
            if(aProduct!=null)
            {
                aProduct.CreatedOn = DateTime.Now;
                aProduct.IsActive = true;
                aProduct.IsDeleted = false;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url+ "Product/Add");
                    var json = JsonConvert.SerializeObject(aProduct);
                    var postTask =
                        client.PostAsync("Product/Add", new StringContent(json, Encoding.UTF8, "application/json"));
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Product added succesfully.");
                        return RedirectToAction("Views","Product");
                    }
                }             

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aProduct);
        }

        public ActionResult AddDetails()
        {
            ViewBag.product = Dropdown.Products();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetails(ProductUnitVm productUnitvm)
        {
            if (productUnitvm != null)
            {
                productUnitvm.IsDeleted = false;
                productUnitvm.IsActive = true;
                foreach (var pic in productUnitvm.ProductImages)
                {
                    if (pic.ImageLocation != null)
                    {
                        MemoryStream target = new MemoryStream();
                        pic.ImageLocation.InputStream.CopyTo(target);
                        var data = target.ToArray();
                        productUnitvm.ImageBytes.Add(data);
                    }
                }
                
                productUnitvm.ProductImages = null;
                using (var client = new HttpClientDemo())
                {
                    var json = JsonConvert.SerializeObject(productUnitvm);
                    var postTask =
                        client.PostAsync("Product/AddDetails", new StringContent(json, Encoding.UTF8, "application/json"));
                    
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Product details has been added.");
                        return RedirectToAction("ViewProductUnit", "Product");
                    }
                }
                FlashMessage.Warning("Product details cannot be added.");
                
            }
            return View();
        }

        public ActionResult EditDetails(long id)
        {
            ViewBag.product = Dropdown.Products();
            ProductUnit productDetails = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetProductDetails");
                var responseTask = client.GetAsync("Product/GetProductDetails?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    productDetails = new JavaScriptSerializer().Deserialize<ProductUnit>(readTask.Result);
                }
                else
                {
                    productDetails = new ProductUnit();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(productDetails);
        }

        //Edit Product Details action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(ProductUnitVm productUnitvm)
        {
            if (productUnitvm != null)
            {
                productUnitvm.IsDeleted = false;
                productUnitvm.IsActive = true;
                foreach (var pic in productUnitvm.ProductImages)
                {
                    if (pic.ImageLocation != null)
                    {
                        MemoryStream target = new MemoryStream();
                        pic.ImageLocation.InputStream.CopyTo(target);
                        var data = target.ToArray();
                        productUnitvm.ImageBytes.Add(data);
                    }
                }

                productUnitvm.ProductImages = null;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Product/EditDetails");
                    var putTask = client.PutAsJsonAsync("Product/EditDetails", productUnitvm);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ViewProductUnit", "Product"); 
                    }
                }
                FlashMessage.Confirmation("Product details has been Updated.");
                return RedirectToAction("ViewProductUnit", "Product");
            }
            return RedirectToAction("AddDetails", "Product");
        }
        
        public ActionResult Views()
        {
            IEnumerable<Product> products = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetAll");
                var responseTask = client.GetAsync("Product/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    products = (new JavaScriptSerializer()).Deserialize<List<Product
                    >>(readTask.Result);
                }
                else
                {
                    products = new List<Product>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
               
            ViewBag.Products = products;
            return View(products);
        }

        public ActionResult Edit(long id)
        {
            Product product =null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetById");
                var responseTask = client.GetAsync("Product/GetById?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    product = (new JavaScriptSerializer()).Deserialize<Product>(readTask.Result);
                }
                else
                {
                    product = new Product();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
          
            ViewBag.category = Dropdown.Categories();
            ViewBag.brand = Dropdown.Brands();
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product aProduct)
        {
            ViewBag.category = Dropdown.Categories();
            ViewBag.brand = Dropdown.Brands();

            if (aProduct != null)
            {
                aProduct.ModifiedOn = DateTime.Now;
                aProduct.IsActive = true;
                aProduct.IsDeleted = false;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Product/Edit");
                    var putTask = client.PutAsJsonAsync<Product>("Product/Edit", aProduct);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Product updated sucessfully.");
                        return RedirectToAction("Views", "Product");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aProduct);
        }


        //
        public ActionResult ViewProductUnit()
        {
            List<ProductUnit> productUnits = GetAllProductUnits();
            ViewBag.productUnits = productUnits;
            return View(productUnits);
        }
        

        public List<ProductUnit> GetAllProductUnits()
        {
            IEnumerable<ProductUnit> products;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetAllProducts");
                var responseTask = client.GetAsync("Product/GetAllProducts");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    products = (new JavaScriptSerializer()).Deserialize<List<ProductUnit>>(readTask.Result);
                }
                else
                {
                    products = new List<ProductUnit>();
                }
            }

            return products.ToList();
        }
    }
}