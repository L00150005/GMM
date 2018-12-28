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
    /// Interaction logic for ReportUI.xaml
    /// </summary>
    public partial class ReportUI : Page

    {
        // Create a connection to teh database
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        List<tblClient> clients = new List<tblClient>();

        // Holds the number of Clients returned
        int numClients = 0;

        public ReportUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Add the plan types to the combo
            foreach (var PlanType in db.tblPlans)
            {
                cmbPlan.Items.Add(PlanType.planName);

            }

        }



        private void refreshPlanList()
        {
            // Load data from our clients table to the list
            clients.Clear();
            foreach (var client in db.tblClients.Where(d => d.planType == cmbPlan.Text))
            {
                clients.Add(client);
            }
            numClients = clients.Count;

            lstClients.ItemsSource = clients;
            lstClients.Items.Refresh();

        }


        private void btnGeneratePlan_Click(object sender, RoutedEventArgs e)
        {
            refreshPlanList();
            lblCount.Content =  "Number of clients: " + numClients;
        }
    }
}
