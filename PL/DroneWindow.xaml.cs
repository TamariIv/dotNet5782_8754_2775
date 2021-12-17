﻿using System;
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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL bl;
        IBL.BO.Drone drone;
        int stationId;

        // add drone ctor
        public DroneWindow(IBL.IBL bl)
        {
            this.bl = bl;
            drone = new IBL.BO.Drone();
            InitializeComponent();

            comboWeightSelcetor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            List<int> listOfStationIds = new List<int>();
            foreach (var s in bl.GetListOfStations(item=> item.AvailableChargeSlots > 0))
                listOfStationIds.Add(s.Id);
            comboStationSelector.ItemsSource = listOfStationIds;

            ActionsGrid.Visibility = Visibility.Hidden;


        }

        // actions with drone ctor
        public DroneWindow(IBL.IBL bl, IBL.BO.DroneToList d)
        {
            this.bl = bl;
            drone = bl.GetDrone(d.Id);
            InitializeComponent();
            ShowDroneData();

            AddDroneGrid.Visibility = Visibility.Hidden;


            switch(d.DroneStatus)
            {
                case IBL.BO.DroneStatus.Available:
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case IBL.BO.DroneStatus.Assigned:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case IBL.BO.DroneStatus.Maintenance:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case IBL.BO.DroneStatus.Delivery:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    break;
            }    
        }

        private void ViewCurrentDrone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void TextBox_TextChanged_DroneId(object sender, TextChangedEventArgs e)
        {
            //if (!(Convert.ToInt32(txtDroneId.Text) > 1000))
            //    txtDroneId.BorderBrush = Brushes.Red;
            drone.Id = Convert.ToInt32(txtDroneId.Text);
        }


        //PreviewKeyDown="TextBox_OnlyNumbers_PreviewKeyDown"
        //private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    txtDroneId.BorderBrush = Brushes.Red;
        //    txtDroneId.BorderBrush = System.Windows.Media.Brushes.Red;
        //    //allow get out of the text box
        //    if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
        //        return;

        //    //allow list of system keys (add other key here if you want to allow)
        //    if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        //        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
        //     || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
        //        return;

        //    char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

        //    //allow control system keys
        //    if (Char.IsControl(c)) return;

        //    //allow digits (without Shift or Alt)
        //    if (Char.IsDigit(c))
        //        if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
        //            return; //let this key be written inside the textbox

        //    //forbid letters and signs (#,$, %, ...)
        //    e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls

        //    return;
        //}

        private void comboWeightSelcetor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)Convert.ToInt32(comboWeightSelcetor.SelectedItem);
            drone.MaxWeight = weight;
        }

        private void TextBox_TextChanged_DroneModel(object sender, TextChangedEventArgs e)
        {
            drone.Model = Convert.ToString(txtDroneModel.Text);
        }

        private void comboStationSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drone.DroneStatus = IBL.BO.DroneStatus.Maintenance;
            stationId = Convert.ToInt32(comboStationSelector.SelectedItem);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddDrone(drone, stationId);
            }
            catch (BL.IdAlreadyExistsException)
            {
                MessageBox.Show("Couldn't add drone \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Drone was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //new DroneListWindow(bl).Show();
            DroneListWindow newWindow = new DroneListWindow(bl);
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        // ACTIONS WITH DRONE FUNCTIONS

        private void btnFinalUpdate_Click(object sender, RoutedEventArgs e)
        {
            Drone tmpDrone = drone;
            tmpDrone.Model = txtModelData.Text;
            try
            {
                bl.UpdateDrone(drone);
            }
            catch(BL.NoUpdateException)
            {
                MessageBox.Show("No update was made \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            drone = tmpDrone;
            MessageBox.Show($"Drone {drone.Id} model was updated successfully \npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.rechargeDrone(drone);
            }
            catch(BL.ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't send drone to charge \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show($"Drone {drone.Id} was sent to charge successfully\npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnSendToDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DroneToParcel(drone.Id);
            }
            catch(BL.ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't pick up parcel \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            MessageBox.Show($"Drone {drone.Id} was sent on delivery successfully\npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnFreeDroneFromCharging_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.FreeDrone(drone.Id, stationId);
            }
            catch(BL.ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't free drone from charging \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            MessageBox.Show($"Drone {drone.Id} was freed from station successfully\npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnPickUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.PickUpParcel(drone);
            }
            catch(BL.NoMatchingIdException)
            {
                MessageBox.Show("No parcel could be picked-up \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            MessageBox.Show($"Drone {drone.Id} picked-up parcel {drone.ParcelInDelivery.Id} successfully\npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnDeliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveryPackage(drone);
            }
            catch (BL.ImpossibleOprationException)
            {
                MessageBox.Show("Parcel can't be delivered \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            MessageBox.Show($"Drone {drone.Id} completed delivery successfully\npress OK to continue", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void ShowDroneData()
        {
            txtIdData.Text = drone.Id.ToString();
            txtModelData.Text = drone.Model;
            txtWeightData.Text = drone.MaxWeight.ToString();
            txtStatusData.Text = drone.DroneStatus.ToString();
            txtLocationData.Text = drone.CurrentLocation.ToString();
            txtBatteryData.Text = ((int)drone.Battery).ToString() + "%";
            txtParcelInDeliveryData.Text = (Convert.ToInt32(drone.ParcelInDelivery.Id) == 0 ? "none" : drone.ParcelInDelivery.ToString());
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
