using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AscendQueryEngine
{
    /// <summary>
    /// Interaction logic for QueryManager.xaml
    /// </summary>
    public partial class QueryManager : Window
    {
        DataTable schema;
        private List<string> ViewList = new List<string>();

        public QueryManager()
        {
            InitializeComponent();

            if(PullDatabaseViews())
            {

            }
            else
            {
                throw new Exception(); //proper error reporting here
            }
            
        }

        private bool PullDatabaseViews()
        {
            try
            {
                string query = "SELECT TABLE_NAME FROM information_schema.`TABLES` WHERE TABLE_TYPE LIKE 'VIEW' AND TABLE_SCHEMA LIKE '"
                    + DbConnect.DbName + "'";

                var command = new MySqlCommand(query, DbConnect.connection);
                var reader = command.ExecuteReader();

                int index = 1;

                while (reader.Read())
                {
                    ViewList.Add(reader.GetString(index)); //dipshit. this is iterating along columns returned. You've ONLY returned table names. 
                    index++;
                }

                MessageBoxResult result = MessageBox.Show(ViewList.ToString());
                return true;
            }
            catch(Exception e)
            {
                MessageBoxResult result = MessageBox.Show(e.ToString());
                return false; //proper error handling
            }


           
        }
    }
}
