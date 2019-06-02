using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WCF_COMP_MANAGER.code
{
    public class SimpleDatabaseLogger
    {
        public static void log(String origin, String s)
        {
            MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO log (origin, value) " +
                                                "VALUES (@origin, @value)", con);
            cmd.Parameters.AddWithValue("@origin", origin);
            cmd.Parameters.AddWithValue("@value", s);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}