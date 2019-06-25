using System;
using System.Collections.Generic;
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
    /// Interaction logic for DbLogin.xaml
    /// </summary>
    public partial class DbLogin : Window
    {
        public DbLogin()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //attempt authentication
            //DbConnect dbconnect = new DbConnect();
            int result;
            bool success = int.TryParse(portNumber.Text, out result);

            if (success)
            {
                DbConnect.Port = result;
            }
            else
            {
                DbConnect.Port = 0; //failure state
            }


            DbConnect.Hostname = textBoxHostname.Text;
            DbConnect.Username = textBoxUsername.Text;
            DbConnect.Password = passwordBox.Password;

            if (DbConnect.Connect())
            {
                //launch query manager window
                QueryManager queryManager = new QueryManager(); //pass connection as arg
                queryManager.Show();
                this.Close();
            }
            else
            {
                errorMessage.Text = "Error connecting to database. Contact IT support.";
            }
        }
    }
}
