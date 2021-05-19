using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Roominator
{
    public class MobileHelper
    {
        private static string[] mobileDevices = new string[] {"iphone","ppc",
                                                      "windows ce","blackberry",
                                                      "opera mini","mobile","palm",
                                                      "portable","opera mobi" };

        public static bool IsMobileDevice(string userAgent)
        {
            userAgent = userAgent.ToLower();
            return mobileDevices.Any(x => userAgent.Contains(x));
        }

        public string CheckDevice()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://roominator-nure.herokuapp.com/");
            return req.UserAgent;
            if (IsMobileDevice("https://roominator-nure.herokuapp.com/"))
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
