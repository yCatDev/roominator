using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Roominator
{
    public class MobileHelper
    {
        private Entry entry = Entry.Unknown;

        private static bool IsMobileDevice(string userAgent)
        {
            if (userAgent.Contains("Mobile") || userAgent.Contains("Android"))
                return true;
            return false;
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
        APK,
        PC
    }
}
