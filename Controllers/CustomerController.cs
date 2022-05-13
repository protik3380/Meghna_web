using System;
using System.Net.Http;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using EFreshStore.Utility;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Create()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index","Home");
            }
                return View();
        }

        [HttpPost]
        public ActionResult Create(Customer aCustomer)
        {
            aCustomer.CreatedOn = DateTime.Now;
            aCustomer.IsDeleted = false;
            
            try
            {
               if (ModelState.IsValid)
                {
                    using (var client = new HttpClientDemo())
                    {
                        var putTask = client.PostAsJsonAsync<Customer>("Customer/Add", aCustomer);
                        putTask.Wait();
                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                           // ViewBag.Message = "User added successfully.";
                            ViewBag.Message = "You have registered successfully.";
                           // FlashMessage.Confirmation("User added successfully.");
                            FlashMessage.Confirmation("You have registered successfully.");
                            return RedirectToAction("Login", "Home");
                        }
                    }
               }
               // ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                ViewBag.Message = "Sorry! This email is already exist. Please try another email";
                FlashMessage.Queue("Sorry! This email is already exist. Please try another email", "Warning: ", FlashMessageType.Warning, false);
                return View();
            }
            catch (Exception)
            {
                FlashMessage.Queue("Sorry! This customer can't be added.", "Warning: ", FlashMessageType.Warning, false);
                ViewBag.Message = "Sorry! This customer can't be added.";
                //FlashMessage.Warning("Sorry! This customer can't be added.");
                return RedirectToAction("Login", "Home");
            }
        }
    }
}