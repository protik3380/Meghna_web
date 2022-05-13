using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace EFreshStore.Utility
{
    public static class Extension
    {
        public static string JSONSerialize(this NameValueCollection _nvc)
        {
            return JsonConvert.SerializeObject(_nvc.AllKeys.ToDictionary(k => k, k => _nvc.GetValues(k)));
        }
        public static void JSONDeserialize(this NameValueCollection _nvc, string _serializedString)
        {
            var deserializedobject = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(_serializedString);

            foreach (var kvp in deserializedobject)
            {
                foreach (var str in kvp.Value)
                {
                    _nvc.Add(kvp.Key, str);
                }
            }
        }
    }
}