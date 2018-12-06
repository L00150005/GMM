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

        int intSaveStatus = 0;

        tblClient selectedClient = new tblClient();

        enum dbOperation
        {
            Add,
            Edit
        }

        dbOperation dbOp = new dbOperation();

        public ClientUI()
        {
            InitializeComponent();
        }


        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = dbOperation.Add;
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
            if (dbOp == dbOperation.Add)
            {
                tblClient client = new tblClient();
                client.firstName = txbFirstname.Text.Trim();
                client.lastName = txbSurname.Text.Trim();
                client.phoneNo = txbPhone.Text.Trim();
                client.address1 = txbAddress1.Text.Trim();
                client.address2 = txbAddress2.Text.Trim();
                client.DOB = DateTime.Parse(txbDOB.Text.Trim());

                SaveUser(client);
                if (intSaveStatus == 1)
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
            else
            {
                // This is the update of an existing client.

                foreach (var client in db.tblClients.Where(d => d.clientID == selectedClient.clientID))
                {
                    selectedClient.clientID = int.Parse(txbGymID .Text.Trim());
                    selectedClient.firstName = txbFirstname.Text.Trim();
                    selectedClient.lastName = txbSurname.Text.Trim();
                    selectedClient.address1 = txbAddress1.Text.Trim();
                    selectedClient.address2 = txbAddress2.Text.Trim();
                    selectedClient.phoneNo = txbPhone.Text.Trim().ToString();
                    selectedClient.DOB = DateTime.Parse(txbDOB.Text.Trim());

                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshClientList();
                        stkAddEdit.Visibility = Visibility.Hidden;
                        stkClientButtons.Visibility = Visibility.Visible;

                        MessageBox.Show("Client Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update Client", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Cancel out of the Add or update record.
            stkAddEdit.Visibility = Visibility.Hidden;
            stkClientButtons.Visibility = Visibility.Visible;

        }

        private void SaveUser(tblClient client)
        {
            db.Entry(client).State = System.Data.Entity.EntityState.Added;
            intSaveStatus = db.SaveChanges();

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

        private void lstClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbOp = dbOperation.Edit;

            stkAddEdit.Visibility = Visibility.Visible;
            stkClientButtons.Visibility = Visibility.Hidden;
            selectedClient = clients.ElementAt(lstClients.SelectedIndex);
            if (selectedClient.clientID > 0)
            {
                txbGymID.Text = selectedClient.clientID.ToString();
                txbFirstname.Text = selectedClient.firstName;
                txbSurname.Text = selectedClient.lastName;
                txbAddress1.Text = selectedClient.address1;
                txbAddress2.Text = selectedClient.address2;
                txbDOB.Text =  selectedClient.DOB.ToString();
                txbPhone.Text = selectedClient.phoneNo.ToString();


            }

        }
    }
}
