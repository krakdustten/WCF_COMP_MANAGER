using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace WCF_COMP_MANAGER.code.venders.returnServices
{
    /// <summary>
    /// Summary description for ReturnService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReturnService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void StoreDigikeyCode()
        {
            String error = HttpContext.Current.Request.QueryString["error"];
            String code = HttpContext.Current.Request.QueryString["code"];
            if (error != null) {
                SimpleDatabaseLogger.log("Digikey_return", error);
                return;
            }
            AuthenticationKeyHandler.setAuthKey("Digikey_AccessToken", code);
            return;
        }

    }
}
