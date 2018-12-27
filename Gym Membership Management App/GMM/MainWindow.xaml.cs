using databaseLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create the connection to the database.
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        private tblUser validatedUser = new tblUser();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string strUser = tbxUsername.Text;
            string strPassword = tbxPassword.Password;

            if (blnValidateUserInput(strUser, strPassword))
            {
                GetUserData(strUser, strPassword);

                if (validatedUser.userID > 0)
                {
                    // Create a new instance of the dashboard screen and pass the validatedUser object to it.
                    this.Hide();
                    DashBoard dashBoard = new DashBoard();
                    dashBoard.Owner = this;
                    dashBoard.user = validatedUser;
                    dashBoard.ShowDialog();
                    
                    
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Username and/or Password cannot be blank", "Login Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Boolean blnValidateUserInput(string strUserID, string strPassword)
        {
            // Validate that the user has input both a username and password
            if (strUserID == "" || strPassword == "")
            {
                return false;
            }
            return true;
        }

        private void GetUserData(string strUserID, string strPassword)
        {
            // Try to connect to the database, catching and throwing an error if unavailable.
            // Then validate the username and password values input
            // by the user against the user table in the database.

            try
            {
                
                foreach (var user in db.tblUsers.Where(d => d.userName == strUserID && d.userPassword == strPassword))
                {
                    if (user.userName == strUserID && user.userPassword == strPassword)
                    {
                        validatedUser = user;
                    }
                }

            }
           catch(EntityException)
            {
                MessageBox.Show("Unable to connect to the database, please try again later.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Exit out of the application, if unable to connect to the database
                this.Close();
                Environment.Exit(0);
 
                
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Exit out of the application
            this.Close();
            Environment.Exit(0);
        }
    }
}
