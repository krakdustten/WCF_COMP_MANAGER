using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCF_COMP_MANAGER.code.dataBase.models;

namespace WCF_COMP_MANAGER.code.venders.seviceProviders
{
    public abstract class VenderServiceProvider
    {
        public static String VenderName { get { return ""; } }

        public abstract String getComponentNumberFromLink(String link);
        public abstract Component getComponentFromNumber(String number);


        private static Dictionary<string, VenderServiceProvider> venders = new Dictionary<string, VenderServiceProvider>
        {
            {MouserServiceProvider.VenderName.ToLower(), new MouserServiceProvider() },
            {RSOnlineServiceProvider.VenderName.ToLower(), new RSOnlineServiceProvider() },
            {DigiKeyServiceProvider.VenderName.ToLower(), new DigiKeyServiceProvider() },
            {FarnellServiceProvider.VenderName.ToLower(), new FarnellServiceProvider() }
        };

        public static VenderServiceProvider getVenderFromName(String name)
        {
            if (venders.ContainsKey(name.ToLower()))
            {
                return venders[name.ToLower()];
            }
            return null;
        }

        public static List<String> getAllVenderNames()
        {
            List<String> ret = new List<string>();
            foreach (KeyValuePair<string, VenderServiceProvider> entry in venders)
            {
                ret.Add(entry.Key);
            }
            return ret;
        }
        public static Component getComponentFromLink(String link)
        {
            foreach (KeyValuePair<string, VenderServiceProvider> entry in venders)
            {
                String component = entry.Value.getComponentNumberFromLink(link);
                if (component != null)
                {
                    Component c = entry.Value.getComponentFromNumber(component);
                    if (c != null) return c;
                }
            }
            return null;
        }
    }
}
