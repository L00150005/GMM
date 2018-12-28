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
    /// Interaction logic for SearchUI.xaml
    /// </summary>
    public partial class SearchUI : Page
    {
        // Create the connection to the database.
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        // Variable to hold client number passed from the dashboard
        public int searchClient = 0;
        private tblClient clientFound = new tblClient();

        public SearchUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            searchForClient();
            // Check that a client was found in the Client table
            if (clientFound.clientID > 0)
            {
                populateControls();
            }
            else
            {
                MessageBox.Show("No matching Client found for:" + searchClient, "Client Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void searchForClient()
        {
            // Select the record from the database.

            try
            {

                foreach (var client in db.tblClients.Where(d => d.clientID == searchClient))
                {
                    clientFound = client;
                }

            }
            catch (EntityException ex)
            {
                MessageBox.Show(ex.Message , "Search", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;

            }

        } 

        private void populateControls()
        {
            // Populate controls with data from the client record
            txbGymID.Text = clientFound.clientID.ToString();
            txbFirstname.Text = clientFound.firstName;
            txbSurname.Text = clientFound.lastName;
            txbAddress1.Text = clientFound.address1;
            txbAddress2.Text = clientFound.address2;
            txbDOB.Text = clientFound.DOB.ToString().Substring(0, 10);
            txbSex.Text = clientFound.Sex;
            txbPhone.Text = clientFound.phoneNo;
            txbPlan.Text = clientFound.planType;
        }

    }
}
