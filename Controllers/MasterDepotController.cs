using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using Vereyon.Web;
using System.Net.Http;
using EFreshStore.Utility;

namespace EFreshStore.Controllers
{
    public class MasterDepotController : Controller
    {
        // GET: Master Depot
        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////Create Master Depot
        //[HttpPost]
        //public ActionResult Create(MasterDepot aMasterDepot)
        //{
        //    if (aMasterDepot != null)
        //    {
        //        using (var client = new HttpClientDemo())
        //        {
        //            //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/Add");
        //            var postTask = client.PostAsJsonAsync<MasterDepot>("MasterDepot/Add", aMasterDepot);
        //            postTask.Wait();
        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                FlashMessage.Confirmation("Master depot created.");
        //                return RedirectToAction("Index", "MasterDepot");
        //            }
        //            else
        //            {
        //                FlashMessage.Warning("Master depot saved failed.");
        //                return View(aMasterDepot);
        //            }
        //        }
        //    }
        //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //    return View();
        //}

        ////Get all master depot
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    IEnumerable<MasterDepot> masterDepots = Dropdown.MasterDepo();
            
        //    ViewBag.Products = masterDepots;
        //    return View(masterDepots);
        //}


        //[HttpGet]
        //public ActionResult Edit(long id)
        //{
        //    MasterDepot masterDepot = null;
        //    using (var client = new HttpClientDemo())
        //    {
        //        //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/GetById");
        //        var responseTask = client.GetAsync("MasterDepot/GetById/" + id);
        //        responseTask.Wait();
        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<MasterDepot>();
        //            readTask.Wait();
        //            masterDepot = readTask.Result;
        //        }
        //        else
        //        {
        //            masterDepot = new MasterDepot();
        //            return RedirectToAction("Create", "MasterDepot");
        //        }
        //    }
        //    return View(masterDepot);
        //}

        //[HttpPost]
        //public ActionResult Edit(MasterDepot aMasterDepot)
        //{

        //    if (aMasterDepot != null)
        //    {
        //        using (var client = new HttpClientDemo())
        //        {
        //            //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/Edit");
        //            var postTask = client.PutAsJsonAsync("MasterDepot/Edit", aMasterDepot);
        //            postTask.Wait();
        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                FlashMessage.Confirmation("Master depot updated.");
        //                return RedirectToAction("Index", "MasterDepot");
        //            }
        //            else
        //            {
        //                FlashMessage.Warning("Master depot update failed.");
        //                return View(aMasterDepot);
        //            }
        //        }
        //    }
        //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //    return View();
        //}

        ////link master depot with thana
        //public ActionResult LinkMasterDepotWithThana()
        //{
        //    ViewBag.MasterDepotId = new SelectList(Dropdown.MasterDepo(), "Id", "Name");
        //    ViewBag.DistrictId = Dropdown.Districts();
        //    //ViewBag.ThanaId = new SelectList(Dropdown.MasterDepo(), "Id", "Name");
        //    return View();
        //}

        public JsonResult GetThanaByDistrictId(long? districtId)
        {
            List<Thana> thanaList;
            if (districtId == null)
            {
                thanaList = new List<Thana>();
            }
            else
            {
                thanaList = Dropdown.GetAllThanaById((long)districtId);
            }

            return Json(thanaList, JsonRequestBehavior.AllowGet);
        }

        //POST link master depot with thana
        //[HttpPost]
        //public ActionResult LinkMasterDepotWithThana(ThanaWiseMasterDepot thanaWiseMasterDepot)
        //{
        //    ViewBag.MasterDepotId = new SelectList(Dropdown.MasterDepo(), "Id", "Name");
        //    ViewBag.DistrictId = Dropdown.Districts();
        //    if (thanaWiseMasterDepot != null)
        //    {
        //        using (var client = new HttpClientDemo())
        //        {
        //            //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/LinkMasterDepotWithThana");
        //            var postTask = client.PostAsJsonAsync("MasterDepot/LinkMasterDepotWithThana", thanaWiseMasterDepot);
        //            postTask.Wait();
        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                FlashMessage.Confirmation("Thana and master depot has been linked");
        //                return RedirectToAction("LinkMasterDepotWithThana", "MasterDepot");
        //            }
        //            else
        //            {
        //                FlashMessage.Warning("Master depot linked failed.");
        //                return View();
        //            }
        //        }
        //    }
        //    return View();
        //}

        //public ActionResult Views()
        //{
        //    IEnumerable<ThanaWiseMasterDepot> thanaWiseMasterDepots = null;
        //    using (var client = new HttpClientDemo())
        //    {
        //        //client.BaseAddress = new Uri(BaseUrl.url + "MasterDepot/GetAllThanaWithMasterDepot");
        //        var responseTask = client.GetAsync("MasterDepot/GetAllThanaWithMasterDepot");
        //        responseTask.Wait();
        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<IList<ThanaWiseMasterDepot>>();
        //            readTask.Wait();
        //            thanaWiseMasterDepots = readTask.Result;
        //        }
        //        else
        //        {
        //            thanaWiseMasterDepots = new List<ThanaWiseMasterDepot>();
        //            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //        }
        //    }

        //    ViewBag.Products = thanaWiseMasterDepots;
        //    return View(thanaWiseMasterDepots);
        //}
    }
}