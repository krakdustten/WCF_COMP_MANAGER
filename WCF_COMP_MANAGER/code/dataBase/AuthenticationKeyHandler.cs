using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WCF_COMP_MANAGER.code.venders
{
    public class AuthenticationKeyHandler
    {
        public static void setAuthKey(String key, String value)
        {
            String oldValue = getAuthKey(key);
            if(oldValue == null)
            {
                MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO authentication (name, value) " +
                                                    "VALUES (@name, @value)", con);
                cmd.Parameters.AddWithValue("@name", key);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE authentication " +
                                                    "SET value = @value, " +
                                                    "settime = @settime " +
                                                    "WHERE name = @name", con);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@settime", DateTime.Now);
                cmd.Parameters.AddWithValue("@name", key);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static String getAuthKey(String key)
        {
            MySqlConnection con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT value FROM authentication WHERE name = @name", con);
            cmd.Parameters.AddWithValue("@name", key);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                String value = dataReader.GetString(0);
                con.Close();
                return value;
            }
            con.Close();
            return null;
        }
    }
}