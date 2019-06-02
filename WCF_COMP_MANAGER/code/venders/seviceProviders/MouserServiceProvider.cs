using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.dataBase.models.subModels;
using WCF_COMP_MANAGER.code.venders.MouserJSON;
using WCF_COMP_MANAGER.code.venders.seviceProviders;

namespace WCF_COMP_MANAGER.code.venders
{
    public class MouserServiceProvider : VenderServiceProvider
    {
        public static String VenderName
        {
            get { return "Mouser"; }
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
                

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.mouser.com/api/V1/search/partnumber?apiKey=5e08dd87-13f3-444a-b64e-b91bc4cc2175");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    SearchByPartRequest = new{
                        mouserPartNumber = number
                    }
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MouserSearchPartReturnJSON));
            Stream respStream = httpResponse.GetResponseStream();
            MouserSearchPartReturnJSON mspr = (MouserSearchPartReturnJSON)ser.ReadObject(respStream);

            if (mspr.SearchResults.Parts.Count <= 0) return comp;
            MouserMouserPartReturnJSON part = mspr.SearchResults.Parts[0];

            if(!excists) comp = new Component();
            comp.Name = part.ManufacturerPartNumber;
            comp.Manufacturer = part.Manufacturer;
            comp.ManufacturerNumber = part.ManufacturerPartNumber;
            comp.Vendername = VenderName;
            comp.VerderComponentNumber = part.MouserPartNumber;
            comp.PriceString = "";
            foreach(MouserPricebreakReturnJSON price in part.PriceBreaks)
            {
                CompPrice cp = new CompPrice();
                cp.Amount = price.Quantity;
                cp.Price = double.Parse(price.Price.Remove(price.Price.Length - 1));
                comp.Prices.Add(cp);
            }
            comp.Link = part.ProductDetailUrl;
            comp.CheckedAt = DateTime.Now;

            if (excists)
                Component.update(comp);
            else
                Component.add(comp);

            return comp;
        }

        public override string getComponentNumberFromLink(string link)
        {
            String[] part = link.Split('/');
            if (part.Length < 4) return null;
            if (!part[2].Contains("mouser")) return null;

            HtmlAgilityPack.HtmlDocument dom = DOMUtils.getDOMFromLink(link);

            HtmlNode someNode = dom.GetElementbyId("spnMouserPartNumFormattedForProdInfo");

            if (someNode == null) return null;

            return someNode.InnerText.Trim(new char[] { '\r', '\t', '\n', ' '});
        }
    }
}