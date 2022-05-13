using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using EFreshStore.Utility;

namespace EFreshStore.Controllers
{
    public class DistributorController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Distributor> distributors = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Distributor/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Distributor>>();
                    readTask.Wait();
                    distributors = readTask.Result;
                }
                else
                {
                    distributors = new List<Distributor>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.Products = distributors;
            return View(distributors);
        }
        public ActionResult Create()
        {
            IEnumerable<MasterDepot> masterDepots = Dropdown.MasterDepo();
            IEnumerable<Thana> thanas = Dropdown.Thanas();
            //using (var client = new HttpClientDemo())
            //{
            //    //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/GetAll");
            //    var responseTask = client.GetAsync("MasterDepot/GetAll");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<MasterDepot>>();
            //        readTask.Wait();
            //        masterDepots = readTask.Result;
            //    }
            //    else
            //    {
            //        masterDepots = new List<MasterDepot>();
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            //using (var client = new HttpClientDemo())
            //{
            //    client.BaseAddress = new Uri(BaseUrl.url + "Thana/GetAll");
            //    var responseTask = client.GetAsync("GetAll");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<Thana>>();
            //        readTask.Wait();
            //        thanas = readTask.Result;
            //    }
            //    else
            //    {
            //        thanas = Enumerable.Empty<Thana>();
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}

            ViewBag.masterDepot = masterDepots;
            ViewBag.thana = thanas;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Distributor aDistributor)
        {
            ViewBag.masterDepot = Dropdown.MasterDepo();
            ViewBag.thana = Dropdown.Thanas();
            if (aDistributor != null)
            {
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Distributor/Create");
                    var postTask = client.PostAsJsonAsync<Distributor>("Distributor/Create", aDistributor);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Distributor");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aDistributor);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            Distributor distributor = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Distributor/GetById");
                var responseTask = client.GetAsync("Distributor/GetById?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Distributor>();
                    readTask.Wait();
                    distributor = readTask.Result;
                }
                else
                {
                    distributor = new Distributor();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.masterDepot = Dropdown.MasterDepo();
            ViewBag.thana = Dropdown.Thanas();
            return View(distributor);
        }

        [HttpPost]
        public ActionResult Edit(Distributor aDistributor)
        {
            ViewBag.masterDepot = Dropdown.MasterDepo();
            ViewBag.thana = Dropdown.Thanas();

            if (aDistributor != null)
            {
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Distributor/Edit");
                    var putTask = client.PutAsJsonAsync<Distributor>("Distributor/Edit", aDistributor);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Distributor");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aDistributor);
        }
    }
}