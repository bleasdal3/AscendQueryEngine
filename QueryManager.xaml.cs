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
        private DataTable schema;      
        private List<string> ViewList = new List<string>();
        private List<string> ColumnList = new List<string>();
        private List<string> SelectedColumns = new List<string>();
        private List<string> Conditions = new List<string>();
        private string View;
        

        public QueryManager()
        {
            InitializeComponent();

            if(PullDbViews())
            {
                ViewListBox.ItemsSource = ViewList;
            }
            else
            {
                throw new Exception(); //TODO proper error reporting here
            }

            BackToCol.IsEnabled = false;
            BackToView.IsEnabled = false;
            ColumnButton.IsEnabled = false;

            ComparatorBox.Items.Add("=");
            ComparatorBox.Items.Add("!=");
            ComparatorBox.Items.Add(">");
            ComparatorBox.Items.Add("<");
            ComparatorBox.Items.Add(">=");
            ComparatorBox.Items.Add("<=");

        }

        private bool PullDbViews()
        {
            try
            {
                string query = "SELECT TABLE_NAME FROM information_schema.`TABLES` WHERE TABLE_TYPE LIKE 'VIEW' AND TABLE_SCHEMA LIKE '"
                    + DbConnect.DbName + "'";

                var command = new MySqlCommand(query, DbConnect.Connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ViewList.Add(reader.GetString(0)); 
                }
                reader.Close();

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return false; //TODO - proper error handling
            }           
        }

        private void ViewSelect_Click(object sender, RoutedEventArgs args)
        {
            //populate DB cols for view
            View = (string)ViewListBox.SelectedItem; //check this casting works
            string query = "SELECT * FROM " + View;
            var command = new MySqlCommand(query, DbConnect.Connection);
            var reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            schema = reader.GetSchemaTable();

            foreach(DataRow col in schema.Rows)
            {
                ColumnList.Add(col.Field<String>("ColumnName"));
            }

            #region ResetToggles
            ColumnsListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            ColumnsListBox.ItemsSource = ColumnList;
            reader.Close();
            ViewButton.IsEnabled = false;
            BackToView.IsEnabled = true;
            ColumnButton.IsEnabled = true;
            #endregion
        }

        private void ColumnSelect_Click(object sender, RoutedEventArgs args)
        {
            //populate columns in conditions listbox
            foreach(string s in ColumnsListBox.SelectedItems)
            {
                SelectedColumns.Add(s);
            }

            #region ResetToggles
            ConditionsListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            ConditionsListBox.ItemsSource = SelectedColumns;
            ColumnButton.IsEnabled = false;
            BackToCol.IsEnabled = true;
            BackToView.IsEnabled = false;
            #endregion
        }

        private void BackToView_Click(object sender, RoutedEventArgs e)
        {
            #region ResetToggles
            ColumnsListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            ColumnList.Clear();
            ColumnsListBox.ItemsSource = ColumnList;
            ViewButton.IsEnabled = true;
            ColumnButton.IsEnabled = false;
            BackToCol.IsEnabled = false;
            BackToView.IsEnabled = false;
            #endregion
        }

        private void BackToCol_Click(object sender, RoutedEventArgs e)
        {
            #region ResetToggles
            ConditionsListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            SelectedColumns.Clear();
            ConditionsListBox.ItemsSource = SelectedColumns;
            ColumnButton.IsEnabled = true;
            BackToView.IsEnabled = true;
            BackToCol.IsEnabled = false;
            #endregion
        }

    }
}
