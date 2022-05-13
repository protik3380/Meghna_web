using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Models.Context;

namespace EFreshStore.Models.ViewModels
{
    public class ProductDetailsVm
    {
        public ProductUnit ProductUnit { get; set; }
        public ICollection<RatingVm> Ratings { get; set; }
        public double Rating { get; set; }

        
    }
}