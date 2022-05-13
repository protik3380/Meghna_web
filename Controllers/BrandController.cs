using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using EFreshStore.Utility;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class BrandController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand aBrand)
        {
            ViewBag.brand = Dropdown.Brands();
            try
            {
                if (aBrand != null)
                {
                    aBrand.CreatedOn = DateTime.Now;
                    aBrand.IsActive = true;
                    aBrand.IsDeleted = false;

                    using (var client = new HttpClientDemo())
                    {
                        //client.BaseAddress = new Uri(BaseUrl.url + "Brand/Create");
                        var postTask = client.PostAsJsonAsync<Brand>("Create", aBrand);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            FlashMessage.Confirmation("Brand created sucessfully");
                            return RedirectToAction("Index", "Brand");
                        }
                        else
                        {
                            FlashMessage.Warning("Brand already exist");
                        }
                    }

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                
            }

            return View(aBrand);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Brand> brands = null;
            using (var client = new HttpClientDemo())
            {
                client.BaseAddress = new Uri(BaseUrl.url + "Brand/GetAll");
                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Brand>>();
                    readTask.Wait();
                    brands = readTask.Result;
                }
                else
                {
                    brands = Enumerable.Empty<Brand>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.Brands = brands;
            return View(brands);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            Brand brand = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Brand/GetById");
                var responseTask = client.GetAsync("GetById/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Brand>();
                    readTask.Wait();
                    brand = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand aBrand)
        {
            if (aBrand != null)
            {
                aBrand.ModifiedOn = DateTime.Now;
                aBrand.IsActive = true;
                aBrand.IsDeleted = false;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Brand/Edit");
                    var putTask = client.PutAsJsonAsync<Brand>("Brand/Edit", aBrand);

                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Brand updated sucessfully.");
                        return RedirectToAction("Index", "Brand");
                    }
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aBrand);
        }
    }
}