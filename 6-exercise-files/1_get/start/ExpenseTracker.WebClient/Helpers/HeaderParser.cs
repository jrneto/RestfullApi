using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace ExpenseTracker.WebClient.Helpers
{
    public static class HeaderParser
    {
        public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders responseHeaders)
        {
            if (responseHeaders.Contains("X-Pagination"))
            {
                var xPag = responseHeaders.First(ph => ph.Key == "X-Pagination").Value;

                return JsonConvert.DeserializeObject<PagingInfo>(xPag.First());
            }

            return null;
        }
    }
}