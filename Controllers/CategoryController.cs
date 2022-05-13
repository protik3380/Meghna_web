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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Create()
        {
            return View();
        }
        //Create category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category aCategory)
        {
            //ViewBag.category = Dropdown.Categories();
            try
            {
                if (aCategory != null)
                {
                    aCategory.CreatedOn = DateTime.Now;
                    aCategory.IsActive = true;
                    aCategory.IsDeleted = false;
                    using (var client = new HttpClientDemo())
                    {
                        //client.BaseAddress = new Uri(BaseUrl.url + "category/Create");
                        var postTask = client.PostAsJsonAsync<Category>("category/Create", aCategory);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            FlashMessage.Confirmation("Category created successfully.");
                            return RedirectToAction("Index", "Category");
                        }
                        else
                        {
                            FlashMessage.Warning("Category already exist.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(aCategory);
        }

        //Get all categories
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Category> categories = Dropdown.Categories();
            ViewBag.Categories = categories;
            return View(categories);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            Category category = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Category/GetById");
                var responseTask = client.GetAsync("category/GetById/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Category>();
                    readTask.Wait();
                    category = readTask.Result;
                }
                else
                {
                    category = new Category();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category aCategory)
        {
            if (aCategory != null)
            {
                aCategory.ModifiedOn = DateTime.Now;
                aCategory.IsActive = true;
                aCategory.IsDeleted = false;
                using (var client = new HttpClientDemo())
                {
                    //client.BaseAddress = new Uri(BaseUrl.url + "Category/Edit");
                    var putTask = client.PutAsJsonAsync<Category>("category/Edit", aCategory);

                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        FlashMessage.Confirmation("Category updated successfully.");
                        return RedirectToAction("Index", "Category");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(aCategory);
        }
    }
}