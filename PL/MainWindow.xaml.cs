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
            FirstWindow.Visibility = Visibility.Visible;
            managerPageGrid.Visibility = Visibility.Hidden;
            CustomerGrid.Visibility = Visibility.Hidden;
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
            try
            {
                if (!mybl.GetListOfCustomers().Where(c => c.Name == txtEnterName.Text).Any())
                    throw new NoMatchingIdException($"no customer named {txtEnterName.Text} was found");
                CustomerToList tmpCustomer = mybl.GetListOfCustomers().First(c => c.Name == txtEnterName.Text);
                Customer customer = mybl.GetCustomer(tmpCustomer.Id);
                new CustomerWindow(mybl, customer).Show();

            }
            catch(NoMatchingIdException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lblNoAccount_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            new SignUpWindow(mybl).Show();
        }

        private void managerBtn_Click(object sender, RoutedEventArgs e)
        {
            FirstWindow.Visibility = Visibility.Hidden;
            managerPageGrid.Visibility = Visibility.Visible;
        }

        private void customerBtn_Click(object sender, RoutedEventArgs e)
        {
            FirstWindow.Visibility = Visibility.Hidden;
            CustomerGrid.Visibility = Visibility.Visible;
        }
    }
}
