﻿using BlApi;
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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        IBL bl;
        Customer customer;
        public CustomerWindow(IBL bl)
        {
            this.bl = bl;
            customer = new Customer();
            InitializeComponent();

            // hide action mode grid
            ActionsGrid.Visibility = Visibility.Hidden;
        }
        public CustomerWindow(IBL bl, Customer c)
        {
            InitializeComponent();
            this.bl = bl;
            customer = c;
            DataContext = customer;
            ParcelsSentListView.ItemsSource = customer.Send;
            ParcelsReceivedListView.ItemsSource = customer.Receive;
            AddCustomerGrid.Visibility = Visibility.Hidden;

        }
        //public void RefreshList(object obj, EventArgs e)
        //{
        //    //    // ParcelsSentListView.ItemsSource = newCustomer.ParcelsFromCustomer;
        //    //    //ParcelsReceivedListView.ItemsSource = newCustomer.ParcelsToCustomer;
        //    DataContext = customer;
        //}
        private void btnFinalUpdate_Click(object sender, RoutedEventArgs e)
        {
            Customer tmpCustomer = customer;
            tmpCustomer.Name = txtNameData.Text;
            tmpCustomer.Phone = txtPhoneData.Text;
            try
            {
                bl.UpdateCustomer(tmpCustomer);
                customer = tmpCustomer;
                MessageBox.Show($"Customer {customer.Id} was updated successfully \npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NoUpdateException)
            {
                MessageBox.Show("No update was made \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
            new CustomerWindow(bl, bl.GetCustomer(customer.Id)).Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddCustomer(customer);
                MessageBox.Show("Customer was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (IdAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoMatchingIdException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Insert details of the customer!", "Error Occurred",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //ADD function
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged_CustomerId(object sender, TextChangedEventArgs e)
        {
            customer.Id = Convert.ToInt32(txtCustomerId.Text);
        }

        private void TextBox_TextChanged_CustomerName(object sender, TextChangedEventArgs e)
        {
            customer.Name = Convert.ToString(txtCustomerName.Text);
        }

        private void TextBox_TextChanged_CustomerPhone(object sender, TextChangedEventArgs e)
        {
            customer.Phone = Convert.ToString(txtCustomerPhone.Text);
        }

        private void TextBox_TextChanged_CustomerLong(object sender, TextChangedEventArgs e)
        {
            customer.Location.Longitude = Convert.ToInt32(txtCustomerLong.Text);
        }
        private void TextBox_TextChanged_CustomerLat(object sender, TextChangedEventArgs e)
        {
            customer.Location.Latitude = Convert.ToInt32(txtCustomerLong.Text);
        }

        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls

            return;
        }
        private void ReceivedlistView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer tmpParcel = (ParcelInCustomer)ParcelsReceivedListView.SelectedItem;
            Parcel p = bl.GetParcel(tmpParcel.Id);
            new ParcelWindow(bl, p).Show();
        }
        private void DeliveredlistView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelInCustomer tmpParcel = (ParcelInCustomer)ParcelsSentListView.SelectedItem;
            Parcel p = bl.GetParcel(tmpParcel.Id);
            new ParcelWindow(bl, p).Show();
        }
    }
}
