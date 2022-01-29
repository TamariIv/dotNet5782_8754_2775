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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL bl;
        Parcel parcel;
        public ParcelWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            parcel = new Parcel();

            ViewGrid.Visibility = Visibility.Hidden;

            // initialize source of the choose weight combobox and priorities
            comboWeightSelcetor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            comboPrioritySelcetor.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        /// <summary>
        /// constructor for view the details of specific parcel from the parcels list
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="p"></param>
        public ParcelWindow(IBL bl, Parcel p)
        {
            InitializeComponent();
            this.bl = bl;
            parcel = p;
            DataContext = parcel;

            //check the dates: 
            //if the date is null, show //

            if (parcel.Requested == null)
                txtRequestedDateData.Text = "//";

            if (parcel.Scheduled == null)
                txtScheduledDateData.Text = "//";

            if (parcel.PickedUp == null)
                txtPickedUpDateData.Text = "//";

            if (parcel.Delivered == null)
                txtDeliveredDateData.Text = "//";

            txtSenderData.Text = Convert.ToString(p.Sender.Id);
            txtTargetData.Text = Convert.ToString(p.Target.Id);

            //if the parcel still wasn't assigned, the ID of drone will be "none".
            txtDroneInParcel.Text = p.Scheduled != null ? Convert.ToString(p.AssignedDrone.Id) : "none";

            //if the parcel still wasn't assigned, hide the label that says to the user to press double tap for more details
            if (p.Scheduled == null) lblDoubleClick.Visibility = Visibility.Hidden;

            //hide the view of adding parcel
            AddParcelGrid.Visibility = Visibility.Hidden;
        }


        private void TextBox_TextChanged_SenderId(object sender, TextChangedEventArgs e)
        {
            CustomerInParcel customerInParcel = new CustomerInParcel();
            customerInParcel.Id = Convert.ToInt32(txtSenderId.Text);
            parcel.Sender = customerInParcel;
        }

        private void TextBox_TextChanged_ReceiverId(object sender, TextChangedEventArgs e)
        {
            CustomerInParcel customerInParcel = new CustomerInParcel();
            customerInParcel.Id = Convert.ToInt32(txtTargetId.Text);
            parcel.Target = customerInParcel;
        }

        private void comboWeightSelcetor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Convert.ToInt32(comboWeightSelcetor.SelectedItem);
            parcel.Weight = weight;
        }

        private void comboPrioritySelcetor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Priorities priority = (Priorities)Convert.ToInt32(comboPrioritySelcetor.SelectedItem);
            parcel.Priority = priority;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSenderId.Text) || string.IsNullOrEmpty(txtTargetId.Text) || string.IsNullOrEmpty(comboWeightSelcetor.Text) || string.IsNullOrEmpty(comboPrioritySelcetor.Text))
                    throw new EmptyInputException("Insert all details of the parcel!");
                bl.AddParcel(parcel);
                MessageBox.Show("Parcel was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
            catch (EmptyInputException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// when the user press double-tap, the window with the details of this drone will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailsOfDrone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtDroneInParcel.Text == "none")
                    throw new NoMatchingObjectException("There isn't assigned drone to this parcel");
                Drone tmpDrone = bl.GetDrone(Convert.ToInt32(txtDroneInParcel.Text));
                new DroneWindow(bl, bl.GetDroneToList(tmpDrone.Id)).Show();
            }
            catch (NoMatchingIdException)
            {
                MessageBox.Show("There isn't assigned drone to this parcel", "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoMatchingObjectException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void DetailsOfSender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer tmpCustomer = bl.GetCustomer(Convert.ToInt32(txtSenderData.Text));
            new CustomerWindow(bl, bl.GetCustomer(tmpCustomer.Id)).Show();
        }
        private void DetailsOfTarget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer tmpCustomer = bl.GetCustomer(Convert.ToInt32(txtTargetData.Text));
            new CustomerWindow(bl, bl.GetCustomer(tmpCustomer.Id)).Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteParcel(parcel.Id);
                MessageBox.Show("Parcel was deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch(BO.NoMatchingIdException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
