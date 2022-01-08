using BlApi;
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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL mybl;
        public MainWindow()
        {
            mybl = BlFactory.GetBl();
            InitializeComponent();
        }

        private void showListOfDronesBtn_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(mybl).Show();
        }

        private void btnShowListOfStations_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(mybl).Show();
        }

        private void btnShowListOfCustomers_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(mybl).Show();
        }
        private void btnShowListOfParcels_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(mybl).Show();
        }


        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //try
            {
                Customer tmpCustomer = mybl.GetCustomer(mybl.GetListOfCustomers().First(c => c.Name == txtEnterName.Text).Id);
                //if (passboxCustomerPassword.Password == tmpCustomer.Password)
                    

            }
        }
    }
}
