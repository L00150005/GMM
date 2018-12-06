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
    /// Interaction logic for PlanUI.xaml
    /// </summary>
    public partial class PlanUI : Page
    {
        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<tblPlan> plans = new List<tblPlan>();

        int intSaveStatus = 0;

        tblPlan selectedPlan = new tblPlan();

        enum dbOperation
        {
            Add,
            Edit
        }

        dbOperation dbOp = new dbOperation();

        public PlanUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data from our Plan table to the grid
            refreshPlanList();     

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = dbOperation.Add;
            stkPlanButtons.Visibility = Visibility.Hidden;
            stkAddEdit.Visibility = Visibility.Visible;
        }

        private void SavePlan(tblPlan plan)
        {
            db.Entry(plan).State = System.Data.Entity.EntityState.Added;
            intSaveStatus = db.SaveChanges();

        }


        private void refreshPlanList()
        {
            plans.Clear();

            foreach (var plan in db.tblPlans)
            {
                plans.Add(plan);
            }

            lstPlans.ItemsSource = plans;
            lstPlans.Items.Refresh();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dbOp == dbOperation.Add)
            {
                tblPlan plan = new tblPlan();
                plan.planName = txbPlanName.Text.Trim();
                plan.planDescription = txbPlanDesc.Text.Trim();
                plan.planPrice = int.Parse(txbPrice.Text.Trim());
                plan.planTerm = "1";

                SavePlan(plan);
                if (intSaveStatus == 1)
                {
                    txbPlanName.Text = "";
                    txbPlanDesc.Text = "";
                    txbPrice.Text = "";

                    stkAddEdit.Visibility = Visibility.Hidden;
                    stkPlanButtons.Visibility = Visibility.Visible;


                    MessageBox.Show("New Plan created", "GMM", MessageBoxButton.OK, MessageBoxImage.Information);

                    refreshPlanList();
                }
                else
                {
                    MessageBox.Show("Unable to Save Plan", "GMM", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // This is the update of an existing plan.

                foreach (var plan in db.tblPlans.Where(d => d.planID == selectedPlan.planID))
                {
                    selectedPlan.planID = int.Parse(txbPlanID.Text.Trim());
                    selectedPlan.planName = txbPlanName.Text.Trim();
                    selectedPlan.planDescription = txbPlanDesc.Text.Trim();
                    selectedPlan.planPrice = int.Parse(txbPrice.Text.Trim());
                    //selectedPlan.planTerm = int.Parse(txbp.Text.Trim());
                    selectedPlan.planTerm = "3";

                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshPlanList();
                        stkAddEdit.Visibility = Visibility.Hidden;
                        stkPlanButtons.Visibility = Visibility.Visible;

                        MessageBox.Show("Plan Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update Plan", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void lstPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbOp = dbOperation.Edit;

            stkAddEdit.Visibility = Visibility.Visible;
            stkPlanButtons.Visibility = Visibility.Hidden;
            selectedPlan = plans.ElementAt(lstPlans.SelectedIndex);
            if (selectedPlan.planID > 0)
            {
                selectedPlan.planID = int.Parse(txbPlanID.Text.Trim());
                selectedPlan.planName = txbPlanName.Text.Trim();
                selectedPlan.planDescription = txbPlanDesc.Text.Trim();
                selectedPlan.planPrice = int.Parse(txbPrice.Text.Trim());
                selectedPlan.planTerm = "1";
            }

        }
    }
}
