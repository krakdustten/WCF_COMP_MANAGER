using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Serialization;
using WCF_COMP_MANAGER.code.dataBase.models.subModels;

namespace WCF_COMP_MANAGER.code.dataBase.models
{
    public class Component
    {
        private long id;
        private String name;
        private String manufacturer;
        private String manufacturerNumber;
        private String vendername;
        private String verderComponentNumber;
        private List<CompPrice> prices;
        private String link;
        private DateTime checkedAt;

        public Component()
        {
            id = -1;
            prices = new List<CompPrice>();
        }

        public Component(long id)
        {
            this.id = id;
            prices = new List<CompPrice>();
        }

        public long Id
        {
            get { return id; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; }
        }

        public String ManufacturerNumber
        {
            get { return manufacturerNumber; }
            set { manufacturerNumber = value; }
        }

        public String Vendername
        {
            get { return vendername; }
            set { vendername = value; }
        }

        public String VerderComponentNumber
        {
            get { return verderComponentNumber; }
            set { verderComponentNumber = value; }
        }

        public List<CompPrice> Prices
        {
            get { return prices; }
        }

        [XmlIgnore]
        public String PriceString
        {
            get
            {
                String s = "";
                foreach(CompPrice cp in prices)
                    s += cp.comb + "; ";

                return s.Length > 0 ? s.Remove(s.Length - 2) : "";
            }
            set
            {
                prices = new List<CompPrice>();
                String[] s = value.Split(';');
                foreach(String ss in s)
                {
                    try
                    {
                        CompPrice c = new CompPrice();
                        c.comb = ss;
                        prices.Add(c);
                    }catch(Exception e)
                    {

                    }
                }
            }
        }

        public String Link
        {
            get { return link; }
            set { link = value; }
        }

        public DateTime CheckedAt
        {
            get { return checkedAt; }
            set { checkedAt = value; }
        }

        public static Component getFromVenderNumber(String vender, String verderComponentNumber)
        {
            MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT id, name, manufacturer, manufacturerNumber, vendername, verderComponentNumber, prices, link, checkedAt " +
                                                "FROM components WHERE vendername = @vendername AND verderComponentNumber = @verderComponentNumber", con);
            cmd.Parameters.AddWithValue("@vendername", vender);
            cmd.Parameters.AddWithValue("@verderComponentNumber", verderComponentNumber);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Component c = new Component(dr.GetInt64(0));
                c.Name = dr.GetString(1);
                c.Manufacturer = dr.GetString(2);
                c.ManufacturerNumber = dr.GetString(3);
                c.Vendername = dr.GetString(4);
                c.verderComponentNumber = dr.GetString(5);
                c.PriceString = dr.GetString(6);
                c.Link = dr.GetString(7);
                c.CheckedAt = dr.GetDateTime(8);
                con.Close();
                return c;
            }
            con.Close();
            return null;
        }

        public static void add(Component comp)
        {
            MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO components (name, manufacturer, manufacturerNumber, vendername, verderComponentNumber, prices, link, checkedAt) " +
                                                "VALUES (@name, @manufacturer, @manufacturerNumber, @vendername, @verderComponentNumber, @prices, @link, @checkedAt)", con);
            cmd.Parameters.AddWithValue("@name", comp.Name);
            cmd.Parameters.AddWithValue("@manufacturer", comp.Manufacturer);
            cmd.Parameters.AddWithValue("@manufacturerNumber", comp.ManufacturerNumber);
            cmd.Parameters.AddWithValue("@vendername", comp.Vendername);
            cmd.Parameters.AddWithValue("@verderComponentNumber", comp.VerderComponentNumber);
            cmd.Parameters.AddWithValue("@prices", comp.PriceString);
            cmd.Parameters.AddWithValue("@link", comp.Link);
            cmd.Parameters.AddWithValue("@checkedAt", comp.CheckedAt);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void update(Component comp)
        {
            MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE components " +
                                                "SET name = @name, " +
                                                    "manufacturer = @manufacturer, " +
                                                    "manufacturerNumber = @manufacturerNumber, " +
                                                    "vendername = @vendername, " +
                                                    "verderComponentNumber = @verderComponentNumber, " +
                                                    "prices = @prices, " +
                                                    "link = @link, " +
                                                    "checkedAt = @checkedAt" +
                                                "WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", comp.Id);
            cmd.Parameters.AddWithValue("@name", comp.Name);
            cmd.Parameters.AddWithValue("@manufacturer", comp.Manufacturer);
            cmd.Parameters.AddWithValue("@manufacturerNumber", comp.ManufacturerNumber);
            cmd.Parameters.AddWithValue("@vendername", comp.Vendername);
            cmd.Parameters.AddWithValue("@verderComponentNumber", comp.VerderComponentNumber);
            cmd.Parameters.AddWithValue("@prices", comp.PriceString);
            cmd.Parameters.AddWithValue("@link", comp.Link);
            cmd.Parameters.AddWithValue("@checkedAt", comp.CheckedAt);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}