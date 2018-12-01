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

        int SaveStatus = 0;

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
            stkUserButtons.Visibility = Visibility.Hidden;
            stkAddEdit.Visibility = Visibility.Visible;
            txbUserID.IsEnabled = false;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            tblUser user = new tblUser();
            user.userName = txbUserName.Text.Trim();
            user.userPassword = txbPassword.Text.Trim();
            user.userSecurityLevel = int.Parse(txbSecurity.Text.Trim());

            SaveUser(user);
            if(SaveStatus == 1)
            {

                txbUserName.Text = "";
                txbPassword.Text = "";
                txbSecurity.Text = "";
                stkAddEdit.Visibility = Visibility.Hidden;
                stkUserButtons.Visibility = Visibility.Visible;
                

                MessageBox.Show("New User created", "GMM", MessageBoxButton.OK, MessageBoxImage.Information);

                refreshUserList();
            }
            else
            {
                MessageBox.Show("Unable to Save User", "GMM", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void SaveUser(tblUser user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Added;
            SaveStatus = db.SaveChanges();
        
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

        }
    }
}
