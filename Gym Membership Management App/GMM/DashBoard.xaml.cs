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
using System.Windows.Shapes;

namespace GMM
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminUI adminUI = new AdminUI();
            frmMain.Navigate(adminUI);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ReportUI reportUI = new ReportUI();
            frmMain.Navigate(reportUI);
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            ClientUI clientUI = new ClientUI();
            frmMain.Navigate(clientUI);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUI searchUI = new SearchUI();
            frmMain.Navigate(searchUI);
        }
    }
}
