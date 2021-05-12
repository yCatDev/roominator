using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Roominator {
    public class APIManager
    {
        private const string SCOPE = "https://www.googleapis.com/auth/userinfo.email";
        private  string redirect_uri = "";
        private const string CLIENT_ID = "636057004195-qufc1kso44du6lpbp191vdmdvogco237.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "QNn_2SkcZkqgmCItvB-0Lb7N";
        string code = "";
        HttpClient client = new HttpClient();
        [Inject]
        public NavigationManager NavManager { get; set; }
        public object HttpServerUtility { get; private set; }

        public string getRedirectURL(string uri) {
            redirect_uri = uri + "menu";
            string request = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                $"redirect_uri={redirect_uri}&" +
                $"client_id={CLIENT_ID}&" +
                $"scope={SCOPE}&" +
                $"response_type=code";

            return request;
        }

        public void getCode(string uri) {
            uri = HttpUtility.UrlDecode(uri);
            string code = "";
            Regex regex = new("(code=)(.+?)&");
            MatchCollection matches = regex.Matches(uri);
            foreach (Match match in matches) {
                code = match.Groups[2].Value;
            }
            this.code =  code;
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> sendRequestToExchangeAccessTokenAsync() {
            var values = new Dictionary<string, string>
                {
                    { "code", $"{code}"},
                    { "client_id", $"{CLIENT_ID}" },
                    { "client_secret", $"{CLIENT_SECRET}" },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", $"{redirect_uri}" }
                };
            foreach (var pair in values) {
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }
            var answer = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(values));
            string json = answer.Content.ReadAsStringAsync().Result;
            object obj = JsonConvert.DeserializeObject(json);
            string res = JsonConvert.SerializeObject(obj);
            Console.WriteLine(res);
            return answer;
        }
    }
}

