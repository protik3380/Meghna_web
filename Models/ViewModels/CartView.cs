using System;

namespace EFreshStore.Models.ViewModels
{
    [Serializable]
    public class CartView
    {
        public long CartId { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public long ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string CartonSize { get; set; }
        public string ProductUnit { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Price { get; set; }
        public double DistributorPrice { get; set; }
        public string ProductImage { get; set; }
        public decimal? TotalLPGDiscount { get; set; }
    }
}