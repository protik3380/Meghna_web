using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.ViewModels
{
    public class WishListViewModel
    {
        public long WishListId { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public long ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal DistributorPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImage { get; set; }
    }
}