using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EFreshStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "contact-us",
              url: "contact-us",
              defaults: new
              {
                  controller = "Home",
                  action = "ContactUs"
              }
          );
            routes.MapRoute(
              name: "faq",
              url: "faq",
              defaults: new
              {
                  controller = "Home",
                  action = "FAQ"
              }
          );
        routes.MapRoute(
            name: "track-order",
            url: "track-order/{orderNo}",
            defaults: new
            {
                controller = "Home",
                action = "TrackOrder",
                orderNo = UrlParameter.Optional
            }
          );
            routes.MapRoute(
               name: "filter",
               url: "search-by",
               defaults: new
               {
                   controller = "Home",
                   action = "FilterProduct"
               }
           );
            routes.MapRoute(
                name: "searchProductByQuery",
                url: "search/{q}",
                defaults: new
                {
                    controller = "Home",
                    action = "Search",
                    q = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "searchProductByCategory",
                url: "search/category/{categoryId}",
                defaults: new
                {
                    controller = "Home",
                    action = "Search",
                    categoryId = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "searchProductByBrand",
                url: "search/brand/{brandId}",
                defaults: new
                {
                    controller = "Home",
                    action = "Search"
                }
            );
            routes.MapRoute(
                name: "home",
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                   
                }
            );
            routes.MapRoute(
                name: "productByBrand",
                url: "home/brand/{brandId}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    brandId = UrlParameter.Optional
                }
            );
            routes.MapRoute(
               name: "product",
               url: "product/details/{ProductId}",
               defaults: new
               {
                   controller = "Product",
                   action = "Details"
               }
           );
            routes.MapRoute(
              name: "sign-in",
              url: "account/sign-in",
              defaults: new
              {
                  controller = "Customer",
                  action = "Create"
              }
          );
            routes.MapRoute(
               name: "login",
               url: "account/login",
               defaults: new
               {
                   controller = "Home",
                   action = "Login"
               }
           );
            routes.MapRoute(
              name: "profile",
              url: "account/profile",
              defaults: new
              {
                  controller = "Contract",
                  action = "ViewUser"
              }
          );
           routes.MapRoute(
              name: "edit-profile",
              url: "account/edit-profile",
              defaults: new
              {
                  controller = "Contract",
                  action = "EditProfile"
              }
          );
            routes.MapRoute(
              name: "change-passowrd",
              url: "account/change-passowrd",
              defaults: new
              {
                  controller = "Home",
                  action = "ChangePassword",
              }
          );
            routes.MapRoute(
              name: "reset-passowrd",
              url: "account/reset-passowrd",
              defaults: new
              {
                  controller = "Home",
                  action = "ResetPassword"

              }
          );
            routes.MapRoute(
              name: "forgot-passowrd",
              url: "account/forgot-passowrd",
              defaults: new
              {
                  controller = "Home",
                  action = "ForgotPassword"
              }
          );
           routes.MapRoute(
              name: "wish-list",
              url: "wish-list",
              defaults: new
              {
                  controller = "WishList",
                  action = "Index"
              }
          );
            routes.MapRoute(
              name: "checkout",
              url: "order/checkout",
              defaults: new
              {
                  controller = "Order",
                  action = "Checkout"
              }
          );
            routes.MapRoute(
              name: "cart",
              url: "order/cart",
              defaults: new
              {
                  controller = "Order",
                  action = "Cart"
              }
          );
            routes.MapRoute(
              name: "empty-cart",
              url: "order/empty-cart",
              defaults: new
              {
                  controller = "Order",
                  action = "EmptyCart"
              }
          );
        routes.MapRoute(
              name: "confirm-order",
              url: "order/confirm-order",
              defaults: new
              {
                  controller = "Order",
                  action = "Confirm"
              }
          );
        routes.MapRoute(
            name: "transaction-failed",
            url: "order/transaction-failed",
            defaults: new
            {
                controller = "Order",
                action = "Failed"
            }
        );
        routes.MapRoute(
            name: "transaction-init-failed",
            url: "order/transaction-initialization-failed",
            defaults: new
            {
                controller = "Order",
                action = "TransactionInitailizationFailed"
            }
        );
            routes.MapRoute(
            name: "transaction-cancelled",
            url: "order/transaction-cancelled",
            defaults: new
            {
                controller = "Order",
                action = "Cancelled"
            }
        );
            routes.MapRoute(
              name: "my-orders",
              url: "order/my-orders",
              defaults: new
              {
                  controller = "Order",
                  action = "MyOrders"
              }
          );
            routes.MapRoute(
              name: "order-details",
              url: "order/details/{id}",
              defaults: new
              {
                  controller = "Order",
                  action = "Details"
              }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
