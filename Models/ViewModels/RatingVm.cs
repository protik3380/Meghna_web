using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.ViewModels
{
    [Serializable]
    public class RatingVm
    {
        public long RatingId { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public short Rating1 { get; set; }
        public string Review { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public DateTime RatingTime { get; set; }
    }
}