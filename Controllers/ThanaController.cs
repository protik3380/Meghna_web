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
    public class ThanaController : Controller
    {
        public ActionResult Create()
        {
            ViewBag.district = Dropdown.Districts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Thana aThana)
        {
            ViewBag.district = Dropdown.Districts();
            try
            {
                if (aThana != null)
                {
                    aThana.CreatedOn = DateTime.Now;
                    aThana.IsDeleted = false;
                    using (var client = new HttpClientDemo())
                    {
                        //client.BaseAddress = new Uri(BaseUrl.url + "Thana/Create");
                        var postTask = client.PostAsJsonAsync<Thana>("Thana/Create", aThana);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            FlashMessage.Confirmation("Thana created successfully.");
                            return RedirectToAction("Views", "Thana");
                        }
                        else
                        {
                            FlashMessage.Warning("Thana already exist.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
          return View(aThana);
        }

        //Get all thanas
        [HttpGet]
        public ActionResult Views()
        {
            IEnumerable<Thana> thanas = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Thana/GetAll");
                var responseTask = client.GetAsync("Thana/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Thana>>();
                    readTask.Wait();
                    thanas = readTask.Result;
                }
                else
                {
                    thanas = new List<Thana>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            ViewBag.Thanas = thanas;
            return View(thanas);
        }

        public ActionResult Edit(long id)
        {
            Thana thana = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Thana/GetById");
                var responseTask = client.GetAsync("Thana/GetById/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Thana>();
                    readTask.Wait();
                    thana = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            IEnumerable<District> aDistrict = Dropdown.Districts();
            
            ViewBag.district = aDistrict;
            return View(thana);
        }
        [HttpPost]
        public ActionResult Edit(Thana aThana)
        {
            ViewBag.district = Dropdown.Districts();

            if (aThana != null)
            {
                aThana.ModifiedOn = DateTime.Now;
                aThana.IsDeleted = false;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Thana/Edit");
                    var putTask = client.PutAsJsonAsync<Thana>("Thana/Edit", aThana);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Thana updated successfully");
                        return RedirectToAction("Views", "Thana");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aThana);
        }
    }
}