using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Efika.Crm.Web.Common
{
    public static class Utility
    {

        public static void SettingPage( System.Web.HttpResponse  response)
        {
            response.Expires = 0;
            response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            response.AddHeader("pragma", "no-cache");
            response.AddHeader("cache-control", "private");
            response.CacheControl = "no-cache";
        }
    }
}