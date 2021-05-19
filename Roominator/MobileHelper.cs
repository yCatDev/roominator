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
        private Entry entry = Entry.Unknown;

        private static string[] mobileDevices = new string[] {"iphone","ppc",
                                                      "windows ce","blackberry",
                                                      "opera mini","mobile","palm",
                                                      "portable","opera mobi" };

        private static bool IsMobileDevice(string userAgent)
        {
            userAgent = userAgent.ToLower();
            return mobileDevices.Any(x => userAgent.Contains(x));
        }

        public Entry CheckDevice(string userAgent)
        {
            if (entry == Entry.Unknown)
            {
                if (IsMobileDevice(userAgent))
                    entry = Entry.Mobile;
                else
                    entry = Entry.PC;
            }
            return entry;
        }

        public void SetEntry(Entry entry)
        {
            this.entry = entry;
        }

        public Entry GetEntry()
        {
            return entry;
        }

        public string GetEntryToString()
        {
            return entry.ToString();
        }
    }

    public enum Entry
    {
        Unknown, 
        Mobile,
        PC,
        APK
    }
}
