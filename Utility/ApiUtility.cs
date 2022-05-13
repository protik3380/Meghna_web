using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EFreshStore.Models.Context;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using EFreshStore.Models;
using EFreshStore.Models.Context.EntityModels;
using EFreshStore.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vereyon.Web;

namespace EFreshStore.Utility
{
    public static class ApiUtility
    {
        public static User ValidateUser(string email, string password)
        {
            //ValidateUserTest(email,password);
            User user = null;
            using (var client = new HttpClientDemo())
            {
                client.BaseAddress = new Uri(BaseUrl.url + "User/Login");
                var responseTask = client.GetAsync("Login?email=" + email + "&password=" + password);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    user = (new JavaScriptSerializer()).Deserialize<User>(readTask.Result);
                }
                else
                {
                    user = null;
                }
            }

            return user;
        }

        //test
        public static LoginCredentials ValidateUserToken(string email, string password)
        {
            using (var client = new HttpClientDemo())
            {
                var userCredentials = new Dictionary<string, string>
                    {
                        {"grant_type", "password"},
                        {"username", email},
                        {"password", password}
                    };
                var tokenResponse = client.PostAsync(BaseUrl.url + "/token", new FormUrlEncodedContent(userCredentials)).Result;
                //var readTask =
                //    tokenResponse.Content.ReadAsAsync<LoginCredentials>(new[] { new JsonMediaTypeFormatter() }).Result;
                dynamic readTask = JObject.Parse(tokenResponse.Content.ReadAsStringAsync().Result);
                LoginCredentials token = new LoginCredentials
                {
                    AccessToken = readTask.access_token == null ? null : readTask.access_token.ToString(),
                    TokenType = readTask.token_type == null ? null : readTask.token_type.ToString(),
                    Error = readTask.error == null ? null : readTask.error.ToString(),
                    RefreshToken = readTask.refresh_token == null ? null : readTask.refresh_token.ToString(),
                    ExpiresIn = readTask.expires_in == null ? 0 : Convert.ToInt32(readTask.expires_in.ToString()),
                };

                //var token = new JavaScriptSerializer().Deserialize<LoginCredentials>(readTask.ToString());
                if (string.IsNullOrEmpty(token.Error))
                {
                    //FlashMessage.Confirmation("Token issued is:  ", token.AccessToken);
                    return token;
                }
                else
                {
                    //FlashMessage.Warning("Error :   ", token.Error);
                    return token;
                }
                //Console.Read();
            }
        }

    public static User GetByUserEmail(string email)
        {
            User user = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "User/GetByUserEmail");
                var responseTask = client.GetAsync("User/GetByUserEmail?email=" + email);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    user = (new JavaScriptSerializer()).Deserialize<User>(readTask.Result);
                }
                else
                {
                    user = null;
                }
            }

            return user;
        }


        public static Customer GetCustomerByUserId(long id)
        {
            Customer customer = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Customer/GetByUserId");
                var responseTask = client.GetAsync("Customer/GetByUserId?id=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    customer = (new JavaScriptSerializer()).Deserialize<Customer>(readTask.Result);
                }
                else
                {
                    customer = new Customer();
                }
            }

            return customer;
        }
        public static CorporateUser GetCorporateUserByUserId(long id)
        {
            CorporateUser corporateUser = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Contract/GetCorporateUserById");
                var responseTask = client.GetAsync("Contract/GetCorporateUserById/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    corporateUser = (new JavaScriptSerializer()).Deserialize<CorporateUser>(readTask.Result);
                }
                else
                {
                    corporateUser = new CorporateUser();
                }
            }

            return corporateUser;
        }

        public static MeghnaUser GetMeghnaUserByUserId(long id)
        {
            MeghnaUser meghBaUser = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "MeghnaUser/GetByUserId");
                var responseTask = client.GetAsync("MeghnaUser/GetByUserId?id=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    meghBaUser = (new JavaScriptSerializer()).Deserialize<MeghnaUser>(readTask.Result);
                }
                else
                {
                    meghBaUser = new MeghnaUser();
                }
            }

            return meghBaUser;
        }

        public static ProductDiscount GetProductDiscountByProductUnitID(long id)
        {
            ProductDiscount productDiscount = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Customer/GetByUserId");
                var responseTask = client.GetAsync("Product/GetDiscountByProductUnit?id=" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    productDiscount = (new JavaScriptSerializer()).Deserialize<ProductDiscount>(readTask.Result);
                }
                else
                {
                    productDiscount = new ProductDiscount();
                }
            }

            return productDiscount;
        }


        public static UserDiscount GetMeghnaUserDiscount()
        {
            UserDiscount userDiscount = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("MeghnaUser/GetMeghnaUserDiscount");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    userDiscount = new JavaScriptSerializer().Deserialize<UserDiscount>(readTask.Result);
                }
                else
                {
                    userDiscount = new UserDiscount();
                }
            }

            return userDiscount;
        }



        public static List<CartView> GetCartDetailsByUserId(long userId)
        {
            List<CartView> cartViews = new List<CartView>();
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Cart/GetByUser?userId=" + userId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    cartViews = new JavaScriptSerializer().Deserialize<List<CartView>>(readTask.Result);
                    cartViews = cartViews.OrderBy(p => p.ProductName).ToList();
                    //foreach (var product in cartViews)
                    //{
                    //    product.Price = product.Price * product.Quantity;
                    //    product.UnitPrice = product.UnitPrice * product.Quantity;
                    //    product.DistributorPrice = product.DistributorPrice * product.Quantity;
                    //}
                }
                else
                {
                    cartViews = new List<CartView>();
                }
            }

            return cartViews;
        }

        public static List<CartView> GetCalculatedCartProductPrice(List<CartView> cart)
        {
            List<CartView> cartViews = new List<CartView>();
            using (var client = new HttpClientDemo())
            {
                var json = JsonConvert.SerializeObject(cart);
                var responseTask =
                    client.PostAsync("Cart/GetCalculatedCartProductPrice", new StringContent(json, Encoding.UTF8, "application/json"));
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    cartViews = new JavaScriptSerializer().Deserialize<List<CartView>>(readTask.Result);
                    cartViews = cartViews.OrderBy(p => p.ProductName).ToList();
                }
                else
                {
                    cartViews = new List<CartView>();
                }
            }

            return cartViews;
        }

        public static List<Rating> GetRatingByProductUnit(long productUnitId)
        {
            List<RatingVm> ratingVms = new List<RatingVm>();
            List<Rating> ratings = new List<Rating>();

            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Rating/GetRatingByProductUnit?id=" + productUnitId);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    ratingVms = (new JavaScriptSerializer()).Deserialize<List<RatingVm>>(readTask.Result);
                    ratingVms = ratingVms.OrderByDescending(r => r.RatingTime).ToList();
                    foreach (var ratingVm in ratingVms)
                    {
                        Rating rating = new Rating
                        {
                            Rating1 = ratingVm.Rating1
                        };
                        ratings.Add(rating);
                    }
                }
                else
                {
                    ratingVms = new List<RatingVm>();
                }
            }
            return ratings;
        }

        public static ProductUnit GetProductUnitByProductId(long productId, long? userId)
        {
            ProductUnit product = null;
            using (var client = new HttpClientDemo())
            {
                //client.BaseAddress = new Uri(BaseUrl.url + "Product/GetProductDetails");
                var responseTask = client.GetAsync("Product/GetProductDetailsWithUserId?id=" + productId + "&userId=" + userId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    product = (new JavaScriptSerializer()).Deserialize<ProductUnit>(readTask.Result);
                }
                return product;
            }
        }

        public static HttpResponseMessage CheckCouponValidityAndCalculateDiscount(CouponDiscountVM couponDiscount)
        {
            using (var client = new HttpClientDemo())
            {
                var postTask = client.PostAsJsonAsync("Coupon/CheckValidity", couponDiscount);
                postTask.Wait();
                return postTask.Result;
            }
        }

public static DeliveryCharge GetValidDeliveryCharge()
        {
            DeliveryCharge deliveryCharge = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("DeliveryCharge/GetValidDeliveryCharge");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    deliveryCharge = (new JavaScriptSerializer()).Deserialize<DeliveryCharge>(readTask.Result);
                }
            }

            return deliveryCharge;
        }

        public static LPGComboDiscount LpgDiscount()
        {
            LPGComboDiscount lpgDiscount = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("LPGComboDiscount/GetLPGComboDiscount");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    lpgDiscount = (new JavaScriptSerializer()).Deserialize<LPGComboDiscount>(readTask.Result);
                }
                else
                {
                    lpgDiscount = new LPGComboDiscount();
                }
            }

            return lpgDiscount;
        }

        public static Order GetOrderByOrderNo(string orderNo)
        {
            Order order = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Order/GetOrderByOrderNo?orderNo=" + orderNo);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    order = (new JavaScriptSerializer()).Deserialize<Order>(readTask.Result);
                }
            }

            return order;
        }

        public static Configuration ActiveConfiguration()
        {
            Configuration configuration = null;
            using (var client = new HttpClientDemo())
            {
                var responseTask = client.GetAsync("Configuration/GetActiveConfiguration");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    configuration = (new JavaScriptSerializer()).Deserialize<Configuration>(readTask.Result);
                }
                else
                {
                    configuration = new Configuration();
                }
            }

            return configuration;
        }

        public static HttpResponseMessage DeleteOrderForSkippedTransaction(string orderNo)
        {
            using (var client = new HttpClientDemo())
            {
                var deleteTask = client.PostAsync("Order/DeleteOrderForSkippedTransaction?orderNo=" + orderNo, null);
                deleteTask.Wait();
                return deleteTask.Result;
            }
        }

    }
}