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
    class DbConnect
    {
        private int Port;
        private string Hostname;
        private string Password;
        private string Username;

        private string dbName;
        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        private MySqlConnection connection;
      
        public DbConnect(string port, string hostname, string username, string password) //constructor
        {
            int result;
            bool success = int.TryParse(port, out result);

            if(success)
            {
                Port = result;
            }
            else
            {
                Port = 0; //failure state
            }

            
            Hostname = hostname;
            Username = username;
            Password = password;
        }

        public bool Connect()
        {
            string DbName = "propcoent-ascendestates"; //TOCHECK
            
            try
            {
                string connectionString = string.Format("Server={0}; database={1}; UID={2}; password={3};",
                    Hostname, DbName, Username, Password); 


                MessageBoxResult result = MessageBox.Show(connectionString);

                connection = new MySqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch(Exception e)
            {
                MessageBoxResult result = MessageBox.Show(e.ToString());
                return false; 
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
