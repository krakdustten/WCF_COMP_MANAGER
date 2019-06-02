using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.venders;
using WCF_COMP_MANAGER.code.venders.seviceProviders;

namespace WCF_COMP_MANAGER
{
    /// <summary>
    /// Summary description for TestService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TestService : System.Web.Services.WebService
    {

        [WebMethod]
        public Component HelloWorld(String link)
        {
            return VenderServiceProvider.getComponentFromLink(link);
        }

        [WebMethod]
        public Component HelloWorld2(String name)
        {
            return VenderServiceProvider.getVenderFromName("RSOnline").getComponentFromNumber(name);
        }
    }
}
