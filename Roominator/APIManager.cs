using Microsoft.AspNetCore.Components;
using Google.Apis;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace Roominator {
    class APIManager
    {
        private const string SCOPE = "https://www.googleapis.com/auth/userinfo.email";
        private const string REDIRECT_URI = "https://roominator-nure.herokuapp.com/menu";
        private const string CLIENT_ID = "636057004195-qufc1kso44du6lpbp191vdmdvogco237.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "QNn_2SkcZkqgmCItvB-0Lb7N";
        [Inject]
        public NavigationManager NavManager { get; set; }
        public string createRequest(string uri) {
            string request = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                $"redirect_uri={uri}menu&" +
                $"client_id={CLIENT_ID}&" +
                $"scope={SCOPE}&" +
                $"response_type=code";
            return request;
        }

        public string getCode(string uri) {
            string code = "";
            Regex regex = new Regex("(code.+?)&");
            MatchCollection matches = regex.Matches(uri);
            foreach (Match match in matches) {
                code = match.Value;
            }
            return code;
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> sendRequestToExchangeAccessTokenAsync(string code) {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
                {
                    { "client_id", $"{CLIENT_ID}&" },
                    { "client_secret", $"{CLIENT_SECRET}&" },
                    { "grant_type", $"authorization_code&" },
                    { "redirect_uri", $"{REDIRECT_URI}&" }
                };
            var answer = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(values));
            Console.WriteLine(answer);
            return answer;
        }
    }
}

