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
    /// Interaction logic for AdminUI.xaml
    /// </summary>
    public partial class AdminUI : Page
    {
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        List<tblUser> users = new List<tblUser>();

        int intSaveStatus = 0;

        tblUser selectedUser = new tblUser();

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
            
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = dbOperation.Add;

            stkUserButtons.Visibility = Visibility.Hidden;
            stkAddEdit.Visibility = Visibility.Visible;
            txbUserID.IsEnabled = false;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dbOp == dbOperation.Add)
            {


                tblUser user = new tblUser();
                user.userName = txbUserName.Text.Trim();
                user.userPassword = txbPassword.Text.Trim();
                user.userSecurityLevel = int.Parse(txbSecurity.Text.Trim());

                SaveUser(user);
                if (intSaveStatus == 1)
                {

                    txbUserName.Text = "";
                    txbPassword.Text = "";
                    txbSecurity.Text = "";
                    stkAddEdit.Visibility = Visibility.Hidden;
                    stkUserButtons.Visibility = Visibility.Visible;


                    MessageBox.Show("New User created", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                    refreshUserList();
                }
                else
                {
                    MessageBox.Show("Unable to Save User", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                // This is the update of an existing user.

                foreach (var user in db.tblUsers.Where(d => d.userID == selectedUser.userID))
                {
                    selectedUser.userID = int.Parse(txbUserID.Text.Trim());
                    selectedUser.userName = txbUserName.Text.Trim();
                    selectedUser.userPassword = txbPassword.Text.Trim();
                    selectedUser.userSecurityLevel = int.Parse(txbSecurity.Text.Trim());

                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshUserList();
                        stkAddEdit.Visibility = Visibility.Hidden;
                        stkUserButtons.Visibility = Visibility.Visible;

                        MessageBox.Show("User Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update User", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
        }

        private void SaveUser(tblUser user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Added;
            intSaveStatus = db.SaveChanges();
        
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

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Cancel out of the Add or update record.
            stkAddEdit.Visibility = Visibility.Hidden;
            stkUserButtons.Visibility = Visibility.Visible;
        }

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbOp = dbOperation.Edit;

            stkAddEdit.Visibility = Visibility.Visible;
            stkUserButtons.Visibility = Visibility.Hidden;
            selectedUser = users.ElementAt(lstUsers.SelectedIndex);
            if(selectedUser.userID > 0)
            {
                txbUserID.Text = selectedUser.userID.ToString();
                txbUserName.Text = selectedUser.userName;
                txbPassword.Text = selectedUser.userPassword;
                txbSecurity.Text = selectedUser.userSecurityLevel.ToString();
            }
            
        }
    }
}
