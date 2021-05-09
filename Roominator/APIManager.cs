using Microsoft.AspNetCore.Components;
using Google.Apis;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace Roominator {
    class APIManager
    {
        private const string scope = "https://www.googleapis.com/auth/userinfo.email";
        private const string redirectURI = "https://roominator-nure.herokuapp.com/menu";
        private const string redURI = "https%3A%2F%2Froominator-nure.herokuapp.com%2Fmenu";
        private const string clientId = "636057004195-qufc1kso44du6lpbp191vdmdvogco237.apps.googleusercontent.com";
        public string createRequest() {
            string request = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                $"redirect_uri={redirectURI}&" +
                $"client_id={clientId}&" +
                $"scope={scope}&" +
                $"response_type=code";
            return request;
        }

    }
}

