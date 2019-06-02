using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.venders.seviceProviders;

namespace WCF_COMP_MANAGER
{
    /// <summary>
    /// Summary description for ComponentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ComponentService : System.Web.Services.WebService
    {

        [WebMethod]
        public Component GetComponentFromLink(String link)
        {
            return VenderServiceProvider.getComponentFromLink(link);
        }

        [WebMethod]
        public Component GetComponentFromVender(String vendername, String vendernumber)
        {
            VenderServiceProvider vender = VenderServiceProvider.getVenderFromName(vendername);
            if (vender == null) return null;
            return vender.getComponentFromNumber(vendernumber);
        }
    }
}
