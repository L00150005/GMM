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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMM
{
    /// <summary>
    /// Interaction logic for ClientUI.xaml
    /// </summary>
    public partial class ClientUI : Page
    {
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        List<tblClient> clients = new List<tblClient>();

        int SaveStatus = 0;

        public ClientUI()
        {
            InitializeComponent();
        }


        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            
                stkAddEdit.Visibility = Visibility.Visible;
                stkClientButtons.Visibility = Visibility.Hidden;
      
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


            // Load data from our client table to the grid
            refreshClientList();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            tblClient client = new tblClient();
            client.firstName = txbFirstname.Text.Trim();
            client.lastName = txbSurname.Text.Trim();
            client.phoneNo = txbPhone.Text.Trim();
            client.address1 = txbAddress1.Text.Trim();
            client.address2 = txbAddress2.Text.Trim();
            client.DOB = DateTime.Parse(txbDOB.Text.Trim());

            SaveUser(client);
            if (SaveStatus == 1)
            {

                txbFirstname.Text = "";
                txbSurname.Text = "";
                txbPhone.Text = "";
                txbAddress1.Text = "";
                txbAddress2.Text = "";
                txbDOB.Text = "";
                stkAddEdit.Visibility = Visibility.Hidden;
                stkClientButtons.Visibility = Visibility.Visible;


                MessageBox.Show("New Client created", "GMM", MessageBoxButton.OK, MessageBoxImage.Information);

                refreshClientList();
            }
            else
            {
                MessageBox.Show("Unable to Save Client", "GMM", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveUser(tblClient client)
        {
            db.Entry(client).State = System.Data.Entity.EntityState.Added;
            SaveStatus = db.SaveChanges();

        }

        private void refreshClientList()
        {
            // Load data from our clients table to the list
            clients.Clear();

            
            foreach (var client in db.tblClients)
            {
                clients.Add(client);
            }

            lstClients.ItemsSource = clients;
            lstClients.Items.Refresh();

        }
    }
}
