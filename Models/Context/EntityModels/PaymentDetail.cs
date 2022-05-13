using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    public class PaymentDetail
    {
        public long Id { get; set; }
        public string tran_id { get; set; }
        public DateTime? tran_date { get; set; }
        public string status { get; set; }
        public string val_id { get; set; }
        public decimal? amount { get; set; }
        public decimal? store_amount { get; set; }
        public string currency { get; set; }
        public string bank_tran_id { get; set; }
        public string card_type { get; set; }
        public string card_no { get; set; }
        public string card_issuer { get; set; }
        public string card_brand { get; set; }
        public string card_issuer_country { get; set; }
        public string card_issuer_country_code { get; set; }
        public string currency_type { get; set; }
        public decimal? currency_amount { get; set; }
        public decimal? currency_rate { get; set; }
        public decimal? base_fair { get; set; }
        public string value_a { get; set; }
        public string value_b { get; set; }
        public string value_c { get; set; }
        public string risk_title { get; set; }
        public int? risk_level { get; set; }
        public string APIConnect { get; set; }
        public string validated_on { get; set; }
        public string gw_version { get; set; }
    }
}