using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WCF_COMP_MANAGER.code.dataBase.models;
using WCF_COMP_MANAGER.code.dataBase.models.subModels;

namespace WCF_COMP_MANAGER.code.venders.seviceProviders
{
    public class RSOnlineServiceProvider : VenderServiceProvider
    {
        public static String VenderName
        {
            get { return "RSOnline"; }
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

            HtmlDocument dom = DOMUtils.getDOMFromLink("https://benl.rs-online.com/web/c/?sra=oss&r=t&searchTerm=" + number);

            HtmlNode prices = dom.GetElementbyId("break-prices-list");
            HtmlNode genInfo = dom.GetElementbyId("pagecell");

            if (prices == null || genInfo == null) return null;

            comp.PriceString = "";
            foreach (HtmlNode p in prices.ChildNodes)
            {
                String amount = null;
                String price = null;
                if(p.ChildNodes.Count > 2)
                {
                    foreach(HtmlNode cn in p.ChildNodes)
                    {
                        String classValue = null;
                        foreach(HtmlAttribute a in cn.Attributes)
                            if (a.Name.Equals("class"))
                                classValue = a.Value;
              
                        if(classValue != null)
                        {
                            if (classValue.StartsWith("breakRange"))
                                amount = cn.InnerHtml;
                            else if (classValue.StartsWith("unitPrice"))
                                price = cn.InnerHtml;
                        }
                    }
                }
                if(amount != null && price != null)
                {
                    try
                    {
                        String[] amo = amount.Trim(new char[] { '\r', '\t', '\n', ' ' }).Split(' ');
                        String[] pri = price.Trim(new char[] { '\r', '\t', '\n', ' ' }).Split(' ');

                        int am = int.Parse(amo[0]);
                        double pr = double.Parse(pri[0]);

                        CompPrice cp = new CompPrice();
                        cp.Amount = am;
                        cp.Price = pr;
                        comp.Prices.Add(cp);
                    }
                    catch(Exception e) {}
                }
            }

            addInfoToComp(comp, genInfo);
            comp.Link = "https://benl.rs-online.com/web/c/?sra=oss&r=t&searchTerm=" + number;
            comp.Name = number;
            comp.Vendername = VenderName;
            comp.CheckedAt = DateTime.Now;

            if (excists)
                Component.update(comp);
            else
                Component.add(comp);

            return comp;
        }

        public override string getComponentNumberFromLink(string link)
        {
            HtmlDocument dom = DOMUtils.getDOMFromLink(link);
            HtmlNode genInfo = dom.GetElementbyId("pagecell");
            if (genInfo == null) return null;
            HtmlNode s1 = DOMUtils.getNextChildNodeWithClass(genInfo, "div", "advLineLevelContainer container");
            if (s1 == null) return null;
            HtmlNode s2 = DOMUtils.getNextChildNodeWithClass(s1, "div", "col-xs-12 prodDescDivLL");
            if (s2 == null) return null;
            HtmlNode s3 = DOMUtils.getNextChildNodeWithClass(s2, "div", "col-xs-10");
            if (s3 == null) return null;
            HtmlNode s4 = DOMUtils.getNextChildNodeWithClass(s3, "div", "col-xs-12 keyDetailsDivLL");
            if (s4 == null) return null;
            HtmlNode top = DOMUtils.getNextChildNodeWithClass(s4, "ul", "keyDetailsLL");
            if (top == null) return null;
            HtmlNode provNum1 = DOMUtils.getNextChildNodeType(top, "li", 0);
            if (provNum1 == null) return null;
            HtmlNode provNum2 = DOMUtils.getNextChildNodeWithClass(provNum1, "span", "keyValue");
            if (provNum2 == null) return null;

            return provNum2.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
        }

        private void addInfoToComp(Component comp, HtmlNode genInfo)
        {
            HtmlNode s1 = DOMUtils.getNextChildNodeWithClass(genInfo, "div", "advLineLevelContainer container");
            if (s1 == null) return;
            HtmlNode s2 = DOMUtils.getNextChildNodeWithClass(s1, "div", "col-xs-12 prodDescDivLL");
            if (s2 == null) return;
            HtmlNode s3 = DOMUtils.getNextChildNodeWithClass(s2, "div", "col-xs-10");
            if (s3 == null) return;
            HtmlNode s4 = DOMUtils.getNextChildNodeWithClass(s3, "div", "col-xs-12 keyDetailsDivLL");
            if (s4 == null) return;
            HtmlNode top = DOMUtils.getNextChildNodeWithClass(s4, "ul", "keyDetailsLL");
            if (top == null) return;
            HtmlNode provNum1 = DOMUtils.getNextChildNodeType(top, "li", 0);
            if (provNum1 == null) return;
            HtmlNode provNum2 = DOMUtils.getNextChildNodeWithClass(provNum1, "span", "keyValue");
            if (provNum2 == null) return;
            HtmlNode fabrNum1 = DOMUtils.getNextChildNodeType(top, "li", 1);
            if (fabrNum1 == null) return;
            HtmlNode fabrNum2 = DOMUtils.getNextChildNodeWithClass(fabrNum1, "span", "keyValue");
            if (fabrNum2 == null) return;
            HtmlNode fabr1 = DOMUtils.getNextChildNodeType(top, "li", 2);
            if (fabr1 == null) return;
            HtmlNode fabr2 = DOMUtils.getNextChildNodeWithClass(fabr1, "span", "keyValue");
            if (fabr2 == null) return;
            HtmlNode fabr3 = DOMUtils.getNextChildNodeType(fabr2, "a", 0);
            if (fabr3 == null) return;
            HtmlNode fabr4 = DOMUtils.getNextChildNodeType(fabr3, "span", 0);
            if (fabr4 == null) return;

            comp.VerderComponentNumber = provNum2.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
            comp.ManufacturerNumber = fabrNum2.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
            comp.Manufacturer = fabr4.InnerText.Trim(new char[] { '\r', '\t', '\n', ' ' });
        }
    }
}