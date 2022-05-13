using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.ViewModels
{
    [Serializable]
    public class CartVm
    {
        public long CartId { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double DistributorPrice { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ProductImage { get; set; }
    }
}