using EFreshStore.Models.Context;
using EFreshStore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class ProductLineController : Controller
    {
        // GET: ProductLine
        public ActionResult Index()
        {
            IEnumerable<ProductLine> productLines = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/GetAll");
                var responseTask = client.GetAsync("ProductLine/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ProductLine>>();
                    readTask.Wait();
                    productLines = readTask.Result;
                }
                else
                {
                    productLines = new List<ProductLine>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.Products = productLines;
            return View(productLines);
        }

        //GET product line create action method
        public ActionResult Create()
        {
            return View();
        }

        //POST product line create action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductLine productLine)
        {
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/Add");
                var postTask =
                    client.PostAsJsonAsync<ProductLine>("ProductLine/Add", productLine);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    FlashMessage.Confirmation("product line has been saved.");
                    return RedirectToAction("Index", "ProductLine");
                }
                else
                {
                    FlashMessage.Warning(result.Content.ReadAsStringAsync().Result);
                    return View();
                }
            }

        }

        //GET product line edit action method
        public ActionResult Edit(long? id)
        {
            ProductLine productLine = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/GetById");
                var responseTask = client.GetAsync("ProductLine/GetById/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProductLine>();
                    readTask.Wait();
                    productLine = readTask.Result;
                }
                else
                {
                    productLine = null;
                    return RedirectToAction("Index", "ProductLine");
                }
            }

            if (productLine == null)
            {
                FlashMessage.Warning("Product line not found");
                return View();
            }

            return View(productLine);
        }

        //POST product line edit action method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(ProductLine productLine)
        {
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/Edit");
                var postTask = client.PutAsJsonAsync<ProductLine>("ProductLine/Edit", productLine);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    FlashMessage.Confirmation("product line has been updated.");
                    return RedirectToAction("Index", "ProductLine");
                }
                else
                {
                    FlashMessage.Warning(result.Content.ReadAsStringAsync().Result);
                    return View();
                }
            }

        }

        //GET link product action method
        public ActionResult Detail()
        {
            IEnumerable<ProductLineDetail> productLineDetails = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/Detail");
                var responseTask = client.GetAsync("ProductLine/Detail");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ProductLineDetail>>();
                    readTask.Wait();
                    productLineDetails = readTask.Result;
                }
                else
                {
                    productLineDetails = new List<ProductLineDetail>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.Products = productLineDetails;
            return View(productLineDetails);
        }
        public ActionResult AddDetail()
        {
            ViewBag.product = Dropdown.Products();
            ViewBag.productLine = Dropdown.ProductLines();
            return View();
        }

        //POST link product action method
        [HttpPost]
        public ActionResult AddDetail(ProductLineDetail productLineDetail)
        {
            ViewBag.product = Dropdown.Products();
            ViewBag.productLine = Dropdown.ProductLines();
            if (productLineDetail != null)
            {
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/AddDetail");
                    var postTask =
                        client.PostAsJsonAsync<ProductLineDetail>("ProductLine/AddDetail", productLineDetail);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("product line has been saved.");
                        return RedirectToAction("AddDetail", "ProductLine");
                    }
                    else
                    {
                        FlashMessage.Warning(result.Content.ReadAsStringAsync().Result);
                        return View();
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult EditDetail(long id)
        {
            ViewBag.product = Dropdown.Products();
            ViewBag.productLine = Dropdown.ProductLines();
            ProductLineDetail productLineDetail = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/GetByDetailId");
                var responseTask = client.GetAsync("ProductLine/GetByDetailId/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProductLineDetail>();
                    readTask.Wait();
                    productLineDetail = readTask.Result;
                }
                else
                {
                    productLineDetail = new ProductLineDetail();
                    return RedirectToAction("Detail", "ProductLine");
                }
            }

            if (productLineDetail == null)
            {
                FlashMessage.Warning("Product line not found");
                return View();
            }

            return View(productLineDetail);
        }

        [HttpPost]
        public ActionResult EditDetail(ProductLineDetail productLineDetail)
        {
            ViewBag.product = Dropdown.Products();
            ViewBag.productLine = Dropdown.ProductLines();
            if (productLineDetail != null)
            {
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/EditDetail");
                    var postTask =
                        client.PutAsJsonAsync<ProductLineDetail>("ProductLine/EditDetail", productLineDetail);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("product line has been updated.");
                        return RedirectToAction("Detail", "ProductLine");
                    }
                    else
                    {
                        FlashMessage.Warning(result.Content.ReadAsStringAsync().Result);
                        return View();
                    }
                }
            }

            return View();
        }
    }
}