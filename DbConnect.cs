using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace AscendQueryEngine
{
   static class DbConnect
    {
        public static int Port { private get; set; }
        public static string Hostname { private get; set; }
        public static string Password { private get; set; }
        public static string Username { private get; set; }
        public static string DbName { get; set; }

        public static MySqlConnection Connection { get; set; }

        public static string SerializeTable(DataTable table)
        {
            string JSONString = "";
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

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

        public static void Close()
        {
            Connection.Close();
        }

        public static string ToCSV(this IDataReader dataReader, bool includeHeaderAsFirstRow = true,
            string separator = ",") 
        {
            /* With thanks and credit to https://stackoverflow.com/users/8010507/nigje */

            DataTable dataTable = new DataTable();
            StringBuilder csvRows = new StringBuilder();
            string row = "";
            int columns;
            try
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        row += (dataTable.Columns[index]);
                        if (index < columns - 1)
                            row += (separator);
                    }
                    row += (Environment.NewLine);
                }
                csvRows.Append(row);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    row = "";
                    //Row
                    //for (int index = 0; index < columns - 1; index++)
                        for (int index = 0; index < columns; index++)
                        {
                        string value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                            {
                                value = value.Replace("\r", "");
                            }
                            while (value.Contains("\n"))
                            {
                                value = value.Replace("\n", "");
                            }
                        }
                        row += value;
                        if (index < columns - 1)
                            row += separator;
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(separator, " ");
                    row += Environment.NewLine;
                    csvRows.Append(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return csvRows.ToString();
        }

    }
}
