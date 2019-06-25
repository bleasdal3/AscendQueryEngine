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
            DbConnect dbconnect = new DbConnect(textBoxHostname.Text, textBoxHostname.Text, textBoxUsername.Text, passwordBox.Password);

            if(dbconnect.Connect())
            {
                //launch query manager window


                //I guess connection is successful here?
                errorMessage.Text = "I think we're connected!";
            }
            else
            {
                errorMessage.Text = "Error connecting to database. Contact IT support.";
            }
        }
    }
}
