using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using EFreshStore.Models.Context;
using EFreshStore.Models.Context.EntityModels;
using Newtonsoft.Json;

namespace EFreshStore.Utility
{
    public static class Dropdown
    {
        public static List<Product> Products()
        {
            IEnumerable<Product> products = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetAll");
                var responseTask = client.GetAsync("Product/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    products = (new JavaScriptSerializer()).Deserialize<List<Product>>(readTask.Result);
                }
                else
                {
                    products = Enumerable.Empty<Product>();
                }
            }
            return products.ToList();
        }

        public static List<CorporateContract> Contracts()
        {
            IEnumerable<CorporateContract> contracts = null;
            using (var client = new HttpClientDemo())
            {
                client.BaseAddress = new Uri(BaseUrl.url + "Contract/GetAll");
                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    contracts = (new JavaScriptSerializer()).Deserialize<List<CorporateContract>>(readTask.Result);
                }
                else
                {
                    contracts = Enumerable.Empty<CorporateContract>();
                }
            }
            return contracts.ToList();
        }

        public static List<Category> Categories()
        {
            IEnumerable<Category> categories = null;
            using (var client = new HttpClientDemo())
            {               
                var responseTask = client.GetAsync("Category/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    categories = (new JavaScriptSerializer()).Deserialize<List<Category>>(readTask.Result);

                }
                else
                {
                    categories = new List<Category>();
                }
            }
            return categories.ToList();

            
        }

        public static List<Brand> Brands()
        {
            IEnumerable<Brand> brands = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Brand/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    brands = (new JavaScriptSerializer()).Deserialize<List<Brand>>(readTask.Result);
                }
                else
                {
                    brands = new List<Brand>();
                }
            }
            return brands.ToList();
        }

        public static List<District> Districts()
        {
            IEnumerable<District> districts = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("District/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    districts = (new JavaScriptSerializer()).Deserialize<List<District>>(readTask.Result);
                }
                else
                {
                    districts = Enumerable.Empty<District>();
                }
            }
            return districts.ToList();
        }

        public static List<CorporateDepartment> CorporateDepartments()
        {
            IEnumerable<CorporateDepartment> corporateDepartmentList = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("CorporateDepartment/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    corporateDepartmentList = (new JavaScriptSerializer()).Deserialize<List<CorporateDepartment>>(readTask.Result);
                }
                else
                {
                    corporateDepartmentList = Enumerable.Empty<CorporateDepartment>();
                }
            }
            return corporateDepartmentList.ToList();
        }

        public static List<CorporateDesignation> CorporateDesignations()
        {
            IEnumerable<CorporateDesignation> corporateDesignationList = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("CorporateDesignation/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    corporateDesignationList = (new JavaScriptSerializer()).Deserialize<List<CorporateDesignation>>(readTask.Result);
                }
                else
                {
                    corporateDesignationList = Enumerable.Empty<CorporateDesignation>();
                }
            }
            return corporateDesignationList.ToList();
        }

        public static List<MeghnaDepartment> MeghnaDepartments()
        {
            IEnumerable<MeghnaDepartment> meghnaDepartmentList = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("MeghnaDepartment/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    meghnaDepartmentList = (new JavaScriptSerializer()).Deserialize<List<MeghnaDepartment>>(readTask.Result);
                }
                else
                {
                    meghnaDepartmentList = Enumerable.Empty<MeghnaDepartment>();
                }
            }
            return meghnaDepartmentList.ToList();
        }

        public static List<MeghnaDesignation> MeghnaDesignations()
        {
            IEnumerable<MeghnaDesignation> meghnaDesignationList = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("MeghnaDesignation/GetAllActive");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    meghnaDesignationList = (new JavaScriptSerializer()).Deserialize<List<MeghnaDesignation>>(readTask.Result); ;
                }
                else
                {
                    meghnaDesignationList = Enumerable.Empty<MeghnaDesignation>();
                }
            }
            return meghnaDesignationList.ToList();
        }
        public static List<Thana> GetAllThanaById(long id)
        {
            IEnumerable<Thana> thanas = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Thana/GetByDistrictId/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    thanas = (new JavaScriptSerializer()).Deserialize<List<Thana>>(readTask.Result); ;
                }
                else
                {
                    thanas = new List<Thana>();
                }
            }
            return thanas.ToList();
        }

        public static List<MasterDepot> MasterDepo()
        {
            IEnumerable<MasterDepot> masterDepots = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("MasterDepot/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    masterDepots = (new JavaScriptSerializer()).Deserialize<List<MasterDepot>>(readTask.Result); ;
                }
                else
                {
                    masterDepots = new List<MasterDepot>();
                }
            }
            return masterDepots.ToList();
        }

        public static List<Thana> Thanas()
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
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    thanas = (new JavaScriptSerializer()).Deserialize<List<Thana>>(readTask.Result);
                }
                else
                {
                    thanas = new List<Thana>();
                }
            }
            return thanas.ToList();
        }

        public static List<ProductLine> ProductLines()
        {
            IEnumerable<ProductLine> productLines = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "ProductLine/GetAll");
                var responseTask = client.GetAsync("ProductLine/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ProductLine>>();
                    readTask.Wait();
                    productLines = readTask.Result;
                }
                else
                {
                    productLines = new List<ProductLine>();
                }
            }
            return productLines.ToList();
        }
    }
}