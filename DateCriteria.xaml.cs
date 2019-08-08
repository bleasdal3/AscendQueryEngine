using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for DateCriteria.xaml
    /// </summary>
    public partial class DateCriteria : Window
    {

        public string StartDate = "";
        public string EndDate = "";
        public DateCriteria(string columnName)
        {
            InitializeComponent();
            DateCriteriaData.ColumnName = columnName;
        }

        private void SubmitDatesButton_Click(object sender, RoutedEventArgs e)
        {

                if (StartPicker.SelectedDate == null) //null or blank?
                {
                    StartDate = "1900-01-01";
                }
                else
                {
                    DateTime date = (DateTime)StartPicker.SelectedDate; //this casting is bonkers. DateTime? doesnt have the associated toString overload I need
                    DateCriteriaData.StartDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);                
                }

                if (EndPicker.SelectedDate == null)
                {
                    EndDate = "2100-01-01";
                }
                else
                {
                    DateTime date = (DateTime)EndPicker.SelectedDate; 
                    DateCriteriaData.EndDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

            this.Close();
        }
    }
}
