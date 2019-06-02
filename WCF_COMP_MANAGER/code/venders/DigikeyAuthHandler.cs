using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WCF_COMP_MANAGER.code.venders
{
    public class DigikeyAuthHandler
    {
        //https://sso.digikey.com/as/authorization.oauth2?response_type=code&client_id=2dfb7b19-d3cf-48d4-9010-c9ed87701506&redirect_uri=https://localhost:44305/code/venders/ReturnService.asmx/StoreDigikeyCode
        //https://sso.digikey.com/as/authorization.oauth2?response_type=code&client_id=2dfb7b19-d3cf-48d4-9010-c9ed87701506&redirect_uri=https://localhost:44305/code/venders/ReturnService.asmx/StoreDigikeyCode

        private static String secret = "uR3yM3oH1bC7iX4bR3xD0bO7bE8vU2wN2uG7xO0yJ3eK7vJ4cG";
        private static String clientId = "2dfb7b19-d3cf-48d4-9010-c9ed87701506";
        private static String redirectURL = "https://localhost:44305/code/venders/ReturnService.asmx/StoreDigikeyCode";

        public static String getAccesToken(Boolean renew)
        {
            String token = AuthenticationKeyHandler.getAuthKey("Digikey_AccessToken");
            if(token == null || renew)
                askForToken();
            return token;
        }

        private static void askForToken()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://sso.digikey.com/as/authorization.oauth2?response_type=code&client_id=" + clientId + "&redirect_uri=" + redirectURL);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";

            httpWebRequest.GetResponse();
        }
    }
}