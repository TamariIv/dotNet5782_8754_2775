using BlApi;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        IBL bl;
        public CustomerListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            CustomersListView.ItemsSource = bl.GetListOfCustomers();

        }

        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList tempCustomer = new CustomerToList();
            tempCustomer = (CustomerToList)CustomersListView.SelectedItem;
            CustomerWindow cw = new CustomerWindow(bl,bl.GetCustomer(tempCustomer.Id));
            cw.Closed += Cw_Closed;
            cw.Show();
        }
        private void Cw_Closed(object sender, EventArgs e)
        {
            CustomersListView.Items.Refresh();
            CustomersListView.ItemsSource = bl.GetListOfCustomers();
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           CustomerToList customer = new CustomerToList();
            customer = (CustomerToList)CustomersListView.SelectedItem;
            CustomerWindow cw = new CustomerWindow(bl);
            cw.Closed += Cw_Closed;
            cw.Show();
        }
    }
}
