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
    public class DistrictController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(District aDistrict)
        {
            //ViewBag.district = Dropdown.Districts();
            try
            {
                if (aDistrict != null)
                {
                    aDistrict.CreatedOn = DateTime.Now;
                    aDistrict.IsDeleted = false;
                    using (var client = new HttpClientDemo())
                    {
                        //client.BaseAddress = new Uri(BaseUrl.url + "District/Create");
                        var postTask = client.PostAsJsonAsync<District>("District/Create", aDistrict);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            FlashMessage.Confirmation("District created successfully");
                            return RedirectToAction("Views", "District");
                        }
                        else
                        {
                            FlashMessage.Warning("District already exist.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(aDistrict);
        }

        [HttpGet]
        public ActionResult Views()
        {
            IEnumerable<District> districts = Dropdown.Districts();
            ViewBag.Districts = districts;
            return View(districts);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            District district = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "District/GetById");
                var responseTask = client.GetAsync("District/GetById/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<District>();
                    readTask.Wait();
                    district = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(district);
        }

        [HttpPost]
        public ActionResult Edit(District aDistrict)
        {
            if (aDistrict != null)
            {
                aDistrict.ModifiedOn = DateTime.Now;
                aDistrict.IsDeleted = false;
                aDistrict.CreatedOn = aDistrict.CreatedOn;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "District/Edit");
                    var putTask = client.PutAsJsonAsync<District>("District/Edit", aDistrict);

                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Distict updated successfully.");
                        return RedirectToAction("Views", "District");
                    }
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aDistrict);
        }
    }
}