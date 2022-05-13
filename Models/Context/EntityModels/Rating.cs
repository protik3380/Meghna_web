using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public class Rating
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public short Rating1 { get; set; }
        public string Review { get; set; }
        public System.DateTime RatingTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedOn { get; set; }

        public virtual ProductUnit ProductUnit { get; set; }
        public virtual User User { get; set; }
    }
}