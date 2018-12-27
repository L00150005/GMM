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
    /// Interaction logic for PlanUI.xaml
    /// </summary>
    public partial class PlanUI : Page
    {
        // Create a connection to the database

        databaseLibrary.theGymDBEntities db = new databaseLibrary.theGymDBEntities("metadata=res://*/GymManagementModel.csdl|res://*/GymManagementModel.ssdl|res://*/GymManagementModel.msl;provider=System.Data.SqlClient;provider connection string = 'data source=192.168.0.184;initial catalog=theGymDB;user id = bcl; password = Galway95; pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");
        List<tblPlan> plans = new List<tblPlan>();

        // Holds the result of adding or updating a record to our database, 0 - fail, 1 - succeed
        int intSaveStatus = 0;

        tblPlan selectedPlan = new tblPlan();

        enum dbOperation
        {
            Add,
            Edit
        }

        dbOperation dbOp = new dbOperation();
        // Holds error message when validating that controls have values entered
        String errMsg = "";

        public PlanUI()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data from our Plan table to the grid
            refreshPlanList();
            cmbDuration.SelectedIndex = 0;
            resetControls();

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            dbOp = dbOperation.Add;
            stkPlanButtons.Visibility = Visibility.Hidden;
            stkAddEdit.Visibility = Visibility.Visible;
        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Cancel out of the Add or update record.
            stkAddEdit.Visibility = Visibility.Hidden;
            stkPlanButtons.Visibility = Visibility.Visible;
            resetControls();
        }



        private void SavePlan(tblPlan plan)
        {
            // Save the plan to the database
            try
            {
                db.Entry(plan).State = System.Data.Entity.EntityState.Added;
                intSaveStatus = db.SaveChanges();
            }
            catch(EntityException ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

        }


        private void refreshPlanList()
        {
            // Refresh the data in the list of plans
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
            // We are creating a new record in the plan database table
            if (dbOp == dbOperation.Add)
            {
                if (ValidateInput() == true)
                {
                    tblPlan plan = new tblPlan();
                    plan.planName = txbPlanName.Text.Trim();
                    plan.planDescription = txbPlanDesc.Text.Trim();
                    plan.planPrice = int.Parse(txbPrice.Text.Trim());
                    plan.planTerm = cmbDuration.Text;

                    SavePlan(plan);
                    
                    if (intSaveStatus == 1)
                    {

                        resetControls();
                        MessageBox.Show("New Plan created", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                        refreshPlanList();
                    }
                    else
                    {
                        MessageBox.Show("Unable to Save Plan", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(errMsg, "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // This is the update of an existing plan.
                if (ValidateInput() == true)
                {
                    foreach (var plan in db.tblPlans.Where(d => d.planID == selectedPlan.planID))
                    {
                        selectedPlan.planID = int.Parse(txbPlanID.Text.Trim());
                        selectedPlan.planName = txbPlanName.Text.Trim();
                        selectedPlan.planDescription = txbPlanDesc.Text.Trim();
                        selectedPlan.planPrice = int.Parse(txbPrice.Text.Trim());
                        selectedPlan.planTerm = cmbDuration.Text;

                    }

                    intSaveStatus = db.SaveChanges();

                    if (intSaveStatus == 1)
                    {
                        refreshPlanList();

                        MessageBox.Show("Plan Updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Update Plan", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show(errMsg, "Update", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lstPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Populate the controls with the new data when we select a different plan from the list.
            dbOp = dbOperation.Edit;

            stkAddEdit.Visibility = Visibility.Visible;
            stkPlanButtons.Visibility = Visibility.Hidden;
            selectedPlan = plans.ElementAt(lstPlans.SelectedIndex);
            if (selectedPlan.planID > 0)
            { 
                txbPlanID.Text = selectedPlan.planID.ToString();
                txbPlanName.Text=selectedPlan.planName;
                txbPlanDesc.Text=selectedPlan.planDescription;
                txbPrice.Text = selectedPlan.planPrice.ToString();
                cmbDuration.Text = selectedPlan.planTerm;
                
            }
        

        }

        private void resetControls()
        {
            // Reset controls to default values.
            txbPlanID.Text = "";
            txbPlanName.Text = "";
            txbPlanDesc.Text = "";
            txbPrice.Text = "";
            cmbDuration.SelectedIndex = 0;

            stkAddEdit.Visibility = Visibility.Hidden;
            stkPlanButtons.Visibility = Visibility.Visible;

        }

        private Boolean ValidateInput()
        {
            // Validate that the user has input values in each of the controls.
            // Return an error string to tell user which control needs a value
            // Return true if all controls have values.
            Boolean isValid = false;

            if (txbPlanName.Text == "")
            {
                errMsg = "Please enter a Plan Name";
                isValid = false;
            }
            else if (txbPlanDesc.Text == "")
            {
                errMsg = "Please enter a Description";
                isValid = false;
            }
            else if (txbPrice.Text == "")
            {
                errMsg = "Please enter a Price";
                isValid = false;
            }
            else if(cmbDuration.SelectedIndex == 0)
            {
                errMsg = "Please select a Plan Term";
                isValid = false;
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private void cmbDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      
    }
}
