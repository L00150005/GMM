using databaseLibrary;
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

namespace GMM
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {
        // Create a new User object
        public tblUser user = new tblUser();
        public DashBoard()
        {
            InitializeComponent();

        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminUI adminUI = new AdminUI();
            frmMain.Navigate(adminUI);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ReportUI reportUI = new ReportUI();
            frmMain.Navigate(reportUI);
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            ClientUI clientUI = new ClientUI();
            frmMain.Navigate(clientUI);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txbSearch.Text == "")
            {
                MessageBox.Show("Please enter a Client number", "Client Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // If a user inputs a client number, create a new instance of the Search screen
                // and pass the client number to this new object.
                SearchUI searchUI = new SearchUI();
                try
                {
                    searchUI.searchClient = int.Parse(txbSearch.Text);
                    frmMain.Navigate(searchUI);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Client Search", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        private void btnPlan_Click(object sender, RoutedEventArgs e)
        {
            PlanUI planUI = new PlanUI();
            frmMain.Navigate(planUI);
        }

        private void setMenuAccess(tblUser user)
        {
            // Ensure that only Admin users can see the Admin, Client and Plan buttons
            // in the stkAdminButtons stack panel.
            if (user.userSecurityLevel == 1)
            {
                stkAdminButtons.Visibility = Visibility.Visible;
            }
            else
            {
                stkAdminButtons.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setMenuAccess(user);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Exit out of the application
            this.Close();
            Environment.Exit(0);
        }
    }
}
