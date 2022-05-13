using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    public class ContactUs
    {
        public string Subject { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}