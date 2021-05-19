using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Roominator
{
    public class MobileHelper
    {
        private static string[] mobileDevices = new string[] {"iphone","ppc",
                                                      "windows ce","blackberry",
                                                      "opera mini","mobile","palm",
                                                      "portable","opera mobi" };

        private static bool IsMobileDevice(string userAgent)
        {
            userAgent = userAgent.ToLower();
            return mobileDevices.Any(x => userAgent.Contains(x));
        }

        public string CheckDevice(string userAgent)
        {
            if (IsMobileDevice(userAgent))
            {
                return "mobile";
            }
            else
            {
                return "pc";
            }
        }
    }
}
