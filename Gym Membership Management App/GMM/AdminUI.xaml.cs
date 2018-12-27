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
    /// Interaction logic for AdminUI.xaml
    /// </summary>
    public partial class AdminUI : Page
    {
        // Create a connection to the database
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        

        List<tblUser> users = new List<tblUser>();

        // Holds the result of adding or updating a record to our database, 0 - fail, 1 - succeed
        int intSaveStatus = 0;

        tblUser selectedUser = new tblUser();

        // Holds error message when validating that controls have values entered
        String errMsg = "";

        enum dbOperation
        {
            Add,
            Edit
        }

        dbOperation dbOp = new dbOperation();

        public AdminUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data from our users table to the grid
            refreshUserList();
            cboSecurity.SelectedIndex = 0;

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = dbOperation.Add;
            // Setup screen for adding a new user
            stkUserButtons.Visibility = Visibility.Hidden;
            stkAddEdit.Visibility = Visibility.Visible;
            txbUserID.IsEnabled = false;

        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Cancel out of the Add or update record.
            resetControls();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Add a new record to the users table
            if (dbOp == dbOperation.Add)
            {
                tblUser user = new tblUser();
                user.userName = txbUserName.Text.Trim();
                user.userPassword = txbPassword.Text.Trim();
                user.userFullname = txbName.Text.Trim();
                user.userSecurityLevel = cboSecurity.SelectedIndex;


                if (ValidateInput() == true)
                {
                    SaveUser(user);
                    if (intSaveStatus == 1)
                    {

                        refreshUserList();
                        resetControls();

                        MessageBox.Show("New User created", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                        
                    }
                    else
                    {
                        MessageBox.Show("Unable to Save User", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(errMsg, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                    errMsg = "";
                }

            }
            else
            {
                // This is the update of an existing user.
                if (ValidateInput() == true)
                {

                    foreach (var user in db.tblUsers.Where(d => d.userID == selectedUser.userID))
                    {
                        selectedUser.userID = int.Parse(txbUserID.Text.Trim());
                        selectedUser.userName = txbUserName.Text.Trim();
                        selectedUser.userPassword = txbPassword.Text.Trim();
                        selectedUser.userFullname = txbName.Text.Trim();
                        selectedUser.userSecurityLevel = cboSecurity.SelectedIndex;
                    }

                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshUserList();
                        resetControls();

                        MessageBox.Show("User Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update User", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                else
                {
                    MessageBox.Show(errMsg, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                    errMsg = "";
                }
            }
        }

        private void SaveUser(tblUser user)
        {
            try
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Added;
                intSaveStatus = db.SaveChanges();
            }
            catch(EntityException ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        
        }

        private void refreshUserList()
        {
            users.Clear();

            foreach (var user in db.tblUsers)
            {
                users.Add(user);
            }

            lstUsers.ItemsSource = users;
            lstUsers.Items.Refresh();

        }



        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Bypass any records that might have been deleted.
            if (lstUsers.SelectedIndex >= 0)
            {
                dbOp = dbOperation.Edit;
                // Populate the controls with the new data when we select a different user from the list.
                stkAddEdit.Visibility = Visibility.Visible;
                stkUserButtons.Visibility = Visibility.Hidden;
                selectedUser = users.ElementAt(lstUsers.SelectedIndex);
                if (selectedUser.userID > 0)
                {
                    txbUserID.Text = selectedUser.userID.ToString();
                    txbUserName.Text = selectedUser.userName;
                    txbPassword.Text = selectedUser.userPassword;
                    txbName.Text = selectedUser.userFullname;
                    cboSecurity.SelectedIndex = selectedUser.userSecurityLevel;
                }

            }
        }

        private void cboSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            ComboBoxItem item = (ComboBoxItem)combo.SelectedItem;        
        }

        private void resetControls()
        {
            // Reset controls to default values.
            txbUserID.Text = "";
            txbUserName.Text = "";
            txbPassword.Text = "";
            txbName.Text = "";
            cboSecurity.SelectedIndex = 0;
            stkAddEdit.Visibility = Visibility.Hidden;
            stkUserButtons.Visibility = Visibility.Visible;
        }

        private Boolean ValidateInput()
        {
            // Validate that the user has input values in each of the controls.
            // Return an error string to tell user which control needs a value
            // Return true if all controls have values.
            Boolean isValid = false;
            
            if (txbUserName.Text == "")
            {
                errMsg = "Please enter a Username";
                isValid = false;
            }
            else if(txbPassword.Text == "")
            {
                errMsg = "Please enter a Password";
                isValid = false;
            }
            else if(txbName.Text == "")
            {
                errMsg = "Please enter a Name";
                isValid = false;
            }
            else if(cboSecurity.SelectedIndex == 0)
            {
                errMsg = "Please enter a Security Level";
                isValid = false;
            }
            else
            {
                isValid = true ;
            }

            // Password must be a min of 8 characters
            int passwordLen = txbPassword.Text.Length;
            if(passwordLen < 8)
            {
                errMsg = "Password must be at least 8 characters long";
                isValid = false;
            }

            return isValid;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Variable to check if delete was a success
            int saveSuccess = 0;

            // This sub deletes a user from the Users table in our database
            MessageBox.Show("Are you sure you want to delete User: " + selectedUser.userID, "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) ;

            try
            {
                db.tblUsers.RemoveRange(db.tblUsers.Where(d => d.userID == selectedUser.userID));
                saveSuccess =  db.SaveChanges();
            }
            catch(EntityException ex)
            {
                MessageBox.Show(ex.Message, "Delete", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            refreshUserList();
            resetControls();
            if (saveSuccess == 1)
            {
                MessageBox.Show("User deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
