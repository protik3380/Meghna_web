using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Utility;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class SubscriberController : Controller
    {
        // GET: Subscriber
        public JsonResult Subscribe(Subscriber subscriber)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            
            if (subscriber.Email != null && Regex.IsMatch(subscriber.Email, pattern))
            {
                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync<Subscriber>("Subscriber/SubscribeUser", subscriber);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new {status = "Ok"});
                    }

                    if (result.StatusCode == HttpStatusCode.Created)
                    {
                        return Json(new { status = "Created" });
                    }

                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return Json(new { status = "Error" });
                    }
                }
            }

            return Json(new { status = "Error" });
        }
    }
}