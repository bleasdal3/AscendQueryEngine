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
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

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
        private string FullQuery = "";
        private string path = "";
        string WhereClause = "";
        bool InitialCondition = true;


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
            ExecuteQuery.IsEnabled = false;
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
                DbConnect.Close();

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
            DbConnect.Connect();
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

            DbConnect.Close();

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
            FullQuery += "SELECT ";

            foreach(string s in ColumnsListBox.SelectedItems)
            {
                
                SelectedColumns.Add(s);
                FullQuery += s + ", ";
            }

            //trim last comma
            FullQuery = FullQuery.Substring(0, (FullQuery.Length - 2));

            //add view
            FullQuery += " FROM " + View;
            FullQueryBox.Text = FullQuery;
             

            #region ResetToggles
            ConditionsListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            ConditionsListBox.ItemsSource = SelectedColumns;
            ColumnButton.IsEnabled = false;
            BackToCol.IsEnabled = true;
            BackToView.IsEnabled = false;
            ExecuteQuery.IsEnabled = true;
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
            FullQuery = "";
            FullQueryBox.Text = FullQuery;            
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
            FullQuery = "";
            FullQueryBox.Text = FullQuery;
            WhereClause = "";
            InitialCondition = true;
            ExecuteQuery.IsEnabled = false;
            #endregion
        }

        private void ApplyConditionButton_Click(object sender, RoutedEventArgs e)
        {
            string Comparator = ComparatorBox.Text;
            string Criteria = InputBox.Text;
            string Condition = (string)ConditionsListBox.SelectedItem;

            
            if(Comparator == "" || Criteria == "" || Condition == null)
            {
                MessageBox.Show("Please enter a comparator and criteria, and also a column.");
            }
            else
            {
                if(InitialCondition == false)
                {
                    WhereClause += " AND ";
                }
                else
                {
                    WhereClause += " WHERE ";
                }

                WhereClause += Condition + " " + Comparator + " " + Criteria;
                FullQueryBox.Text = (FullQuery += WhereClause);
                InitialCondition = false;

                #region ResetToggles
                ComparatorBox.Text = "";
                ConditionsListBox.UnselectAll();
                InputBox.Text = "";
                WhereClause = "";
                #endregion
            }
        }

        private void OutputPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                PathBox.Text = path;
            }
        }

        private void ExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            FullQuery += ";"; //close query
            //FullQuery += " LIMIT 30;"; //debug limit
            ExecuteQuery.IsEnabled = false; //prevent multiple clicks by impatient user if the file is large.
            
            try
            {
                if (path == "")
                {
                    MessageBox.Show("Please select output file.");
                }
                else
                {
                    DbConnect.Connect();

                    var command = new MySqlCommand(FullQuery, DbConnect.Connection);
                    //DataTable table = new DataTable();
                    //table.Load(command.ExecuteReader());
                    //string _csv = DataTableReader.T
                    //string JSONString = DbConnect.SerializeTable(table);

                    MySqlDataReader reader = command.ExecuteReader();
                    string _csv = reader.ToCSV(true);

                    File.WriteAllText(path, _csv);

                    reader.Close();
                    DbConnect.Close();

                    MessageBox.Show("Complete!");
                    ExecuteQuery.IsEnabled = true; //turn it back on again
                    path = "";
                    PathBox.Text = ""; //require manual input to overwrite.
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ExecuteQuery.IsEnabled = true;
            }

        }

    }
}
