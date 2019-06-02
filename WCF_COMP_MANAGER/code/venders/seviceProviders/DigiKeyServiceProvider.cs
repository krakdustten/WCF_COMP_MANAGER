using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.dataBase.models.subModels;

namespace WCF_COMP_MANAGER.code.venders.seviceProviders
{
    public class DigiKeyServiceProvider : VenderServiceProvider
    {
        public static new String VenderName
        {
            get { return "DigiKey"; }
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


            HtmlDocument dom = DOMUtils.getDOMFromLink("https://www.digikey.be/products/nl?keywords=" + number);

            addInfoToComp(comp, dom);
            comp.Vendername = VenderName;
            comp.Name = number;
            comp.Link = "https://www.digikey.be/products/nl?keywords=" + number;
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
            if (!part[2].Contains("digikey")) return null;

            HtmlAgilityPack.HtmlDocument dom = DOMUtils.getDOMFromLink(link);

            HtmlNode someNode = dom.GetElementbyId("reportPartNumber");
            if (someNode == null)
                return null;
            return someNode.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
        }

        private void addInfoToComp(Component comp, HtmlDocument dom)
        {
            HtmlNode pn = dom.GetElementbyId("reportPartNumber");
            if (pn == null) return;
            comp.VerderComponentNumber = pn.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });

            HtmlNode top = dom.GetElementbyId("product-overview");
            if (top == null) return;
            HtmlNode s1 = DOMUtils.getNextChildNodeType(top, "tbody", 0);
            if (s1 == null) s1 = top;

            foreach (HtmlNode hn in s1.ChildNodes)
            {
                if (hn.Name.Equals("tr"))
                {
                    HtmlNode td = DOMUtils.getNextChildNodeType(hn, "td", 0);
                    if (td == null) continue;
                    HtmlNode h1 = DOMUtils.getNextChildNodeType(td, "h1", 0);
                    if (h1 != null)
                    {
                        comp.ManufacturerNumber = h1.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
                        continue;
                    }
                    HtmlNode h2 = DOMUtils.getNextChildNodeType(td, "h2", 0);
                    if (h2 != null)
                    {
                        HtmlNode span1 = DOMUtils.getNextChildNodeType(h2, "span", 0);
                        if (span1 == null) continue;
                        HtmlNode span2 = DOMUtils.getNextChildNodeType(span1, "span", 0);
                        if (span2 == null) continue;
                        comp.Manufacturer = span2.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
                        continue;
                    }
                }
            }

            comp.PriceString = "";
            HtmlNode pricedoc = dom.GetElementbyId("priceProcurement");
            if (pricedoc == null) return;
            HtmlNode p0 = DOMUtils.getNextChildNodeWithClass(pricedoc, "div", "catalog-pricing");
            if (p0 == null) return;
            HtmlNode p1 = DOMUtils.getNextChildNodeWithClass(p0, "table", "product-dollars");
            if (p1 == null) return;
            HtmlNode p2 = DOMUtils.getNextChildNodeType(p1, "tbody", 0);
            if (p2 == null) p2 = p1;
            foreach (HtmlNode hn in p2.ChildNodes)
            {
                if (hn.Name.Equals("tr"))
                {
                    HtmlNode amt = DOMUtils.getNextChildNodeType(hn, "td", 0);
                    if (amt == null) continue;
                    HtmlNode upr = DOMUtils.getNextChildNodeType(hn, "td", 1);
                    if (upr == null) continue;
                    HtmlNode upr2 = DOMUtils.getNextChildNodeType(hn, "span", 0);
                    if (upr2 != null) upr = upr2;

                    try
                    {
                        String amo = amt.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' }).Replace(".", "");
                        String[] pri = upr.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' }).Split(' ');

                        int am = int.Parse(amo);
                        double pr = double.Parse(pri[pri.Length - 1]);

                        CompPrice cp = new CompPrice();
                        cp.Amount = am;
                        cp.Price = pr;
                        comp.Prices.Add(cp);
                    }
                    catch (Exception e) { }
                }
            }
        }
    }
}