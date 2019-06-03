using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Web;

namespace WCF_COMP_MANAGER.code
{
    public class DOMUtils
    {
        public static HtmlDocument getDOMFromLink(String link, String accept = "*/*")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(link);
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseText);
                return htmlDoc;
            }
        }

        public static HtmlNode getNextChildNodeWithClass(HtmlNode parent, String type, String htmlClass)
        {
            foreach (HtmlNode hn in parent.ChildNodes)
            {
                if (hn.Name.Equals(type))
                {
                    String classValue = null;
                    foreach (HtmlAttribute a in hn.Attributes)
                        if (a.Name.Equals("class"))
                            classValue = a.Value;
                    if (classValue != null)
                    {
                        if (classValue.Contains(htmlClass))
                            return hn;
                    }
                }
            }
            return null;
        }

        public static HtmlNode getNextChildNodeType(HtmlNode parent, String type, int num)
        {
            int tel = 0;
            foreach (HtmlNode hn in parent.ChildNodes)
            {
                if (hn.Name.Equals(type))
                {
                    if(tel == num)return hn;
                    tel++;
                }
            }
            return null;
        }
    }
}