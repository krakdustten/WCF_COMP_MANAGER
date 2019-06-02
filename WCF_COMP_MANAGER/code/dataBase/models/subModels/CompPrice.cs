using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WCF_COMP_MANAGER.code.dataBase.models.subModels
{
    public class CompPrice
    {
        private int amount;
        private double price;

        public CompPrice()
        {

        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        [XmlIgnore]
        public String comb
        {
            get { return amount + ": " + price; }
            set
            {
                String[] d = value.Split(':');
                amount = int.Parse(d[0]);
                price = double.Parse(d[1]);
            }
        }
    }
}