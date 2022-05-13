using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using EFreshStore.Models.Context;
using EFreshStore.Utility;
using Vereyon.Web;

namespace EFreshStore.Controllers
{
    public class MeghnaUserController : Controller
    {
        //Add Bulk Meghna User
        public ActionResult AddMeghnaUser()
        {
            return View();
        }

        [ActionName("AddMeghnaUser")]
        [HttpPost]
        public ActionResult AddBulkMeghnaUser()
        {
            List<MeghnaUser> savedUsers = new List<MeghnaUser>();
            List<MeghnaUser> duplicateEmails = new List<MeghnaUser>();
            if (Request.Files["ecxelFile"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["ecxelFile"].FileName).ToLower();
                string query = null;
                string connString = "";
                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                string filePath = string.Format("{0}/{1}", Server.MapPath("~/Content"), Request.Files["ecxelFile"].FileName);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        Request.Files["ecxelFile"].SaveAs(filePath);

                        if (extension == ".csv")
                        {
                            DataTable dt = UtilityClass.ConvertCSVtoDataTable(filePath);
                            ViewBag.Data = dt;
                            var isExistColumn = UtilityClass.IsAllColumnExist(dt, UtilityClass.MeghnaUserProperty());
                            if (!isExistColumn)
                            {
                                FlashMessage.Warning("Some columns are missing");
                                return View();
                            }
                            List<MeghnaUser> meghnaUsers = (from DataRow row in dt.Rows
                                                            select new MeghnaUser
                                                            {
                                                                EmployeeId = row["EmployeeId"].ToString(),
                                                                Name = row["Name"].ToString(),
                                                                MobileNo = row["MobileNo"].ToString(),
                                                                Email = row["Email"].ToString(),
                                                                Designation = row["Designation"].ToString(),
                                                                DeliveryAddress1 = row["DeliveryAddress1"].ToString(),
                                                                DeliveryAddress2 = row["DeliveryAddress2"].ToString(),
                                                                IsActive = true,
                                                                IsDeleted = false,
                                                                CreatedOn = DateTime.Now
                                                            }).ToList();
                            System.IO.File.Delete(filePath);
                            int count = 0;
                            foreach (var user in meghnaUsers)
                            {
                                using (var client = new HttpClientDemo())
                                {
                                    //client.BaseAddress = new Uri(BaseUrl.url + "MeghnaUser/AddMeghnaUser");
                                    var postTask = client.PostAsJsonAsync<MeghnaUser>("MeghnaUser/AddMeghnaUser", user);
                                    postTask.Wait();
                                    var result = postTask.Result;
                                    if (result.IsSuccessStatusCode)
                                    {
                                        savedUsers.Add(user);
                                        count += 1;
                                    }
                                    duplicateEmails.Add(user);
                                }
                            }
                            ViewBag.Count = count;
                            ViewBag.SavedUser = savedUsers;
                            ViewBag.DuplicateEmails = duplicateEmails;
                            return View();
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath +
                                         ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            DataTable dt = UtilityClass.ConvertXLSXtoDataTable(filePath, connString);
                            ViewBag.Data = dt;
                            List<string> meghnaUserColumnList = UtilityClass.MeghnaUserProperty();
                            string columnNames = null;
                            foreach (var column in meghnaUserColumnList)
                            {
                                columnNames += column + ", ";
                            }
                            var isExistColumn = UtilityClass.IsAllColumnExist(dt, UtilityClass.MeghnaUserProperty());
                            if (!isExistColumn)
                            {
                                FlashMessage.Warning("Some columns are missing.");
                                FlashMessage.Warning("These columns are mandetory: "+ columnNames);
                                return View();
                            }
                            List<MeghnaUser> meghnaUsers = (from DataRow row in dt.Rows
                                                            select new MeghnaUser
                                                            {
                                                                EmployeeId = row["EmployeeId"].ToString(),
                                                                Name = row["Name"].ToString(),
                                                                MobileNo = row["MobileNo"].ToString(),
                                                                Email = row["Email"].ToString(),
                                                                Designation = row["Designation"].ToString(),
                                                                DeliveryAddress1 = row["DeliveryAddress1"].ToString(),
                                                                DeliveryAddress2 = row["DeliveryAddress2"].ToString(),
                                                                IsActive = true,
                                                                IsDeleted = false,
                                                                CreatedOn = DateTime.Now
                                                            }).ToList();
                            System.IO.File.Delete(filePath);
                            int count = 0;
                            foreach (var user in meghnaUsers)
                            {
                                using (var client = new HttpClientDemo())
                                {
                                    //client.BaseAddress = new Uri(BaseUrl.url + "MeghnaUser/AddMeghnaUser");
                                    var postTask = client.PostAsJsonAsync<MeghnaUser>("MeghnaUser/AddMeghnaUser", user);
                                    postTask.Wait();
                                    var result = postTask.Result;
                                    if (result.IsSuccessStatusCode)
                                    {
                                        savedUsers.Add(user);
                                        count += 1;
                                    }
                                    duplicateEmails.Add(user);
                                }
                            }
                            ViewBag.Count = count;
                            ViewBag.SavedUser = savedUsers;
                            ViewBag.DuplicateEmails = duplicateEmails;
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = @"Provider='Microsoft.ACE.OLEDB.12.0';Data Source='" + filePath +
                                         "';Extended Properties='Excel 12.0;IMEX=1'";
                            DataTable dt = UtilityClass.ConvertXLSXtoDataTable(filePath, connString);
                            var isExistColumn = UtilityClass.IsAllColumnExist(dt, UtilityClass.MeghnaUserProperty());
                            if (!isExistColumn)
                            {
                                FlashMessage.Warning("Some columns are missing");
                                return View();
                            }
                            List<MeghnaUser> meghnaUsers = (from DataRow row in dt.Rows
                                                            select new MeghnaUser
                                                            {
                                                                EmployeeId = row["EmployeeId"].ToString(),
                                                                Name = row["Name"].ToString(),
                                                                MobileNo = row["MobileNo"].ToString(),
                                                                Email = row["Email"].ToString(),
                                                                Designation = row["Designation"].ToString(),
                                                                DeliveryAddress1 = row["DeliveryAddress1"].ToString(),
                                                                DeliveryAddress2 = row["DeliveryAddress2"].ToString(),
                                                                IsActive = true,
                                                                IsDeleted = false,
                                                                CreatedOn = DateTime.Now
                                                            }).ToList();
                            System.IO.File.Delete(filePath);
                            int count = 0;
                            foreach (var user in meghnaUsers)
                            {
                                using (var client = new HttpClientDemo())
                                {
                                    //client.BaseAddress = new Uri(BaseUrl.url + "MeghnaUser/AddMeghnaUser");
                                    var postTask = client.PostAsJsonAsync<MeghnaUser>("MeghnaUser/AddMeghnaUser", user);
                                    postTask.Wait();
                                    var result = postTask.Result;
                                    if (result.IsSuccessStatusCode)
                                    {
                                        savedUsers.Add(user);
                                        count += 1;
                                    }
                                    duplicateEmails.Add(user);
                                }
                            }
                            ViewBag.Count = count;
                            ViewBag.SavedUser = savedUsers;
                            ViewBag.DuplicateEmails = duplicateEmails;
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Please upload files in .xls, .xlsx or .csv format.";
                }
            }
            return View();
        }
    }
}