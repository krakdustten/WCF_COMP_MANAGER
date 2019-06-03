using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Script.Serialization;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.dataBase.models.subModels;
using WCF_COMP_MANAGER.code.venders.FarnellJSON;
using WCF_COMP_MANAGER.code.venders.MouserJSON;

namespace WCF_COMP_MANAGER.code.venders.seviceProviders
{
    public class FarnellServiceProvider : VenderServiceProvider
    {
        public static new String VenderName
        {
            get { return "Farnell"; }
        }

        public override Component getComponentFromNumber(string number)
        {
            Component comp = Component.getFromVenderNumber(VenderName, number);
            bool excists = false;

            if (comp != null)
            {
                if (comp.CheckedAt.AddDays(1).CompareTo(DateTime.Now) > 0)
                    return comp;
                excists = true;
            }
            if (!excists) comp = new Component();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                "https://api.element14.com/catalog/products?callInfo.responseDataFormat=json&term=id:" + number +
                "&storeInfo.id=be.farnell.com&callInfo.apiKey=j8effcajubg2ug2vyekzhs6f&resultsSettings.responseGroup=medium");
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FarnellProductSearchReturnJSON));
            Stream respStream = httpResponse.GetResponseStream();
            FarnellProductSearchReturnJSON mspr = (FarnellProductSearchReturnJSON)ser.ReadObject(respStream);

            if (mspr.premierFarnellPartNumberReturn.products.Count <= 0) return comp;
            FarnellProductJSON product = mspr.premierFarnellPartNumberReturn.products[0];

            comp.Name = product.displayName;
            comp.Manufacturer = product.vendorName;
            comp.ManufacturerNumber = product.translatedManufacturerPartNumber;
            comp.Vendername = VenderName;
            comp.VerderComponentNumber = number;
            comp.PriceString = "";
            foreach (FarnellPriceJSON price in product.prices)
            {
                CompPrice cp = new CompPrice();
                cp.Amount = price.from;
                cp.Price = price.cost;
                comp.Prices.Add(cp);
            }
            comp.Link = "https://be.farnell.com/search?st=" + number;
            comp.CheckedAt = DateTime.Now;

            if (excists)
                Component.update(comp);
            else
                Component.add(comp);

            return comp;
        }

        public override string getComponentNumberFromLink(string link)
        {
            String[] part = link.Split('?');
            if (part.Length < 1) return null;
            String[] subparts = part[0].Split('/');
            if (subparts.Length < 4) return null;
            if (!subparts[2].Contains("farnell")) return null;

            return subparts[subparts.Length - 1];
        }
    }
}