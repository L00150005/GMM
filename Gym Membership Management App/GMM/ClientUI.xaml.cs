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
    /// Interaction logic for ClientUI.xaml
    /// </summary>
    public partial class ClientUI : Page
    {
        // Create a connection to the database
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        List<tblClient> clients = new List<tblClient>();
        List<tblPlan> plans = new List<tblPlan>();

        // Holds the result of adding or updating a record to our database, 0 - fail, 1 - succeed
        int intSaveStatus = 0;

        tblClient selectedClient = new tblClient();

        enum DbOperation
        {
            Add,
            Edit
        }

        DbOperation dbOp = new DbOperation();

       

        // Holds error message when validating that controls have values entered
        String errMsg = "";

        public ClientUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data from our client table to the grid
            refreshClientList();

            // Add the plan types that the user can signup to when joining the gym
            cboPlanType.Items.Add("Please select");
            foreach (var PlanType in db.tblPlans)
            {
                cboPlanType.Items.Add(PlanType.planName);

            }

            resetControls();
            cboSex.SelectedIndex = 0;
        }


        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = DbOperation.Add;
            stkAddEdit.Visibility = Visibility.Visible;
            stkClientButtons.Visibility = Visibility.Hidden;

        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Cancel out of the Add or update record.
            stkAddEdit.Visibility = Visibility.Hidden;
            stkClientButtons.Visibility = Visibility.Visible;
            resetControls();
        }

       

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Create a nw record in the client table
            if (dbOp == DbOperation.Add)
            {
                if (ValidateInput() == true)
                { 
                
                    tblClient client = new tblClient();
                    client.firstName = txbFirstname.Text.Trim();
                    client.lastName = txbSurname.Text.Trim();
                    client.phoneNo = txbPhone.Text.Trim();
                    client.address1 = txbAddress1.Text.Trim();
                    client.address2 = txbAddress2.Text.Trim();
                    client.DOB = DateTime.Parse(txbDOB.Text.Trim());
                    client.Sex = cboSex.Text;
                    client.planType = cboPlanType.Text;
                

                    SaveUser(client);
                    if (intSaveStatus == 1)
                    {
                        resetControls();

                        MessageBox.Show("New Client created", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                        refreshClientList();
                    }
                    else
                    {
                        MessageBox.Show("Unable to Save Client", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(errMsg, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // This is the update of an existing client.

                foreach (var client in db.tblClients.Where(d => d.clientID == selectedClient.clientID))
                {
                    selectedClient.clientID = int.Parse(txbGymID.Text.Trim());
                    selectedClient.firstName = txbFirstname.Text.Trim();
                    selectedClient.lastName = txbSurname.Text.Trim();
                    selectedClient.address1 = txbAddress1.Text.Trim();
                    selectedClient.address2 = txbAddress2.Text.Trim();
                    selectedClient.phoneNo = txbPhone.Text.Trim().ToString();
                    selectedClient.DOB = DateTime.Parse(txbDOB.Text.Trim());
                    selectedClient.Sex = cboSex.Text;
                    selectedClient.planType = cboPlanType.Text;
                }
                if (ValidateInput() == true)
                {
                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshClientList();

                        MessageBox.Show("Client Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update Client", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show(errMsg, "Update", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }





        private void SaveUser(tblClient client)
        {
            try
            {
                db.Entry(client).State = System.Data.Entity.EntityState.Added;
                intSaveStatus = db.SaveChanges();
            }
            catch(EntityException ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

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
            dbOp = DbOperation.Edit;

            stkAddEdit.Visibility = Visibility.Visible;
            stkClientButtons.Visibility = Visibility.Hidden;
            selectedClient = clients.ElementAt(lstClients.SelectedIndex);
            if (selectedClient.clientID > 0)
            {
                // Populate the controls with the new data when we select a different client from the list.
                txbGymID.Text = selectedClient.clientID.ToString();
                txbFirstname.Text = selectedClient.firstName;
                txbSurname.Text = selectedClient.lastName;
                txbAddress1.Text = selectedClient.address1;
                txbAddress2.Text = selectedClient.address2;
                txbDOB.Text = selectedClient.DOB.ToString().Substring(0,10);
                txbPhone.Text = selectedClient.phoneNo.ToString();
                if (selectedClient.Sex.Trim() == "Male")
                {
                    cboSex.SelectedIndex = 1;
                }
                else
                {
                    cboSex.SelectedIndex = 2;
                }
                cboPlanType.Text = selectedClient.planType;
                

            }

        }
        private void resetControls()
        {
            // Reset controls to default values.
            txbGymID.Text = "";
            txbFirstname.Text = "";
            txbSurname.Text = "";
            txbPhone.Text = "";
            txbAddress1.Text = "";
            txbAddress2.Text = "";
            txbDOB.Text = "";
            cboSex.SelectedIndex = 0;
            cboPlanType.SelectedIndex = 0;
            stkAddEdit.Visibility = Visibility.Hidden;
            stkClientButtons.Visibility = Visibility.Visible;
        }

        private Boolean ValidateInput()
        {
            // Validate that the user has input values in each of the controls.
            // Return an error string to tell user which control needs a value
            // Return true if all controls have values.
            Boolean isValid = false;

            if (txbFirstname.Text == "")
            {
                errMsg = "Please enter a Firstname";
                isValid = false;
            }
            else if (txbSurname.Text == "")
            {
                errMsg = "Please enter a Surname";
                isValid = false;
            }
            else if (txbAddress1.Text == "")
            {
                errMsg = "Please enter Address 1";
                isValid = false;
            }
            else if (txbAddress2.Text == "")
            {
                errMsg = "Please enter Address 2";
                isValid = false;
            }
            else if (txbDOB.Text == "")
            {
                errMsg = "Please enter a Date of Birth";
                isValid = false;
            }
            else if (txbPhone.Text == "")
            {
                errMsg = "Please enter a Phone Number";
                isValid = false;
            }
            else if (cboSex.SelectedIndex == 0)
            {
                errMsg = "Please enter a Sex";
                isValid = false;
            }
            else if (cboPlanType.SelectedIndex == 0)
            {
                errMsg = "Please enter a Plan Type";
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private void cboPlanType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            ComboBoxItem item = (ComboBoxItem)combo.SelectedItem;

        }


    }
}
