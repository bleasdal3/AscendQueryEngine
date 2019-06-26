using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AscendQueryEngine
{
   static class DbConnect
    {
        public static int Port { private get; set; }
        public static string Hostname { private get; set; }
        public static string Password { private get; set; }
        public static string Username { private get; set; }

        public static MySqlConnection Connection { get; set; }

        public const string DbName = "propcoent-ascendestates";


        public static bool Connect()
        {           
            try
            {
                string connectionString = string.Format("Server={0}; database={1}; UID={2}; password={3};",
                    Hostname, DbName, Username, Password);

                Connection = new MySqlConnection(connectionString);
                Connection.Open();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return false; 
            }
        }

        static void Close()
        {
            Connection.Close();
        }
    }
}
