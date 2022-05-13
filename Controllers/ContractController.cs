using EFreshStore.Models.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using EFreshStore.Models;
using EFreshStore.Models.Enum;
using EFreshStore.Models.ViewModels;
using EFreshStore.Utility;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class ContractController : Controller
    {
        [HttpGet]
        public ActionResult ViewUser()
        {
            var email = Session["email"];
            var userId = Session["UserId"];
            if (Session["UserId"] == null)
            {
                return RedirectToAction("index", "Home");
            }
            List<CorporateUser> corporateUsers;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Contract/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CorporateUser>>();
                    readTask.Wait();
                    corporateUsers = (List<CorporateUser>)readTask.Result;
                }
                else
                {
                    corporateUsers = new List<CorporateUser>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                ViewBag.contract = corporateUsers;
                return View();
            }
        }

        public ActionResult ChangePassword()
        {
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
                User user = new User();
                user.Id = userId;
                user.Password = password;
                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync<User>("User/ChangePassword", user);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Password changed successfully.");
                        return RedirectToAction("ViewUser", "Contract");
                    }
                    else
                    {
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

        public ActionResult EditProfile()
        {
            if (Session["UserId"] == null)
            {

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(UpdateProfileVm profileVm)
        {
            string returnUrl = (string)Session["requesturl"];
            if (Session["UserId"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (Session["email"] != null)
            {
                var userTypeId = Convert.ToInt64(Session["UserTypeId"]);
                if (userTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    if (profileVm != null)
                    {
                        using (var client = new HttpClientDemo())
                        {
                            var putTask = client.PostAsJsonAsync<UpdateProfileVm>("MeghnaUser/Edit", profileVm);
                            putTask.Wait();
                            var result = putTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                FlashMessage.Confirmation("Profile has been updated successfully");
                                return RedirectToAction("ViewUser", "Contract");
                            }
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                        }
                    }
                    return View();
                }
                else if (userTypeId == (long)UserTypeEnum.Corporate)
                {
                    if (profileVm != null)
                    {
                        using (var client = new HttpClientDemo())
                        {
                            var putTask = client.PostAsJsonAsync<UpdateProfileVm>(BaseUrl.url + "Contract/EditUser", profileVm);
                            putTask.Wait();
                            var result = putTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                FlashMessage.Confirmation("Profile has been updated successfully");
                                return RedirectToAction("ViewUser", "Contract");
                            }
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                        }
                    }
                    return RedirectToAction("ViewUser", "Contract");
                }
                else if (userTypeId == (long)UserTypeEnum.Customer)
                {
                    if (profileVm != null)
                    {
                        using (var client = new HttpClientDemo())
                        {
                            var putTask = client.PostAsJsonAsync<UpdateProfileVm>("Customer/Edit", profileVm);
                            putTask.Wait();
                            var result = putTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                FlashMessage.Confirmation("Profile has been updated successfully");
                                return RedirectToAction("ViewUser", "Contract");
                            }
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                        }
                    }
                    return RedirectToAction("ViewUser", "Contract");
                }
                else
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "~/";
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }

        }
    }
}