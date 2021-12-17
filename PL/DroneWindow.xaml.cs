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

            //DataContext = drone;
            comboWeightSelcetor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            List<int> listOfStationIds = new List<int>();
            foreach (var s in bl.GetListOfStationsWithAvailableChargeSlots())
                listOfStationIds.Add(s.Id);
            comboStationSelector.ItemsSource = listOfStationIds;

            btnFinalUpdate.Visibility = Visibility.Hidden;
            btnSendToCharge.Visibility = Visibility.Hidden;
            btnSendToDelivery.Visibility = Visibility.Hidden;
            btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
            btnPickUpParcel.Visibility = Visibility.Hidden;
            btnDeliverParcel.Visibility = Visibility.Hidden;
            txtUpdateDroneModel.Visibility = Visibility.Hidden;
            lblNewModel.Visibility = Visibility.Hidden;
        }

        // actions with drone ctor
        public DroneWindow(IBL.IBL bl, IBL.BO.DroneToList d)
        {
            this.bl = bl;
            drone = bl.GetDrone(d.Id);
            InitializeComponent();

            ShowDroneData();

            //ViewCurrentDrone.i

            // hide comboboxes for weight and charging station
            comboStationSelector.IsEnabled = false;
            comboStationSelector.Visibility = Visibility.Hidden;
            comboWeightSelcetor.IsEnabled = false;
            comboWeightSelcetor.Visibility = Visibility.Hidden;
            txtDroneId.Visibility = Visibility.Hidden;
            txtDroneModel.Visibility = Visibility.Hidden;
            lblChooseStation.Visibility = Visibility.Hidden;
            lblChooseWeight.Visibility = Visibility.Hidden;
            lblEnterId.Visibility = Visibility.Hidden;
            lblEnterModel.Visibility = Visibility.Hidden;

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

        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            txtDroneId.BorderBrush = Brushes.Red;
            txtDroneId.BorderBrush = System.Windows.Media.Brushes.Red;
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

        private void comboWeightSelcetor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)Convert.ToInt32(comboWeightSelcetor.SelectedItem);
            drone.MaxWeight = weight;
        }

        private void TextBox_TextChanged_DroneModel(object sender, TextChangedEventArgs e)
        {
            drone.Model = Convert.ToString(txtDroneModel.Text);
        }

        private void TextBox_TextChanged_UpdateModel(object sender, TextChangedEventArgs e)
        {
            drone.Model = txtUpdateDroneModel.Text;
            bl.UpdateDrone(drone);
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
            catch (IdAlreadyExistsException)
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

        private void btnFinalUpdate_Click(object sender, RoutedEventArgs e)
        {
            drone.Model = txtUpdateDroneModel.Text;
            try
            {
                bl.UpdateDrone(drone);
            }
            catch(NoUpdateException)
            {
                MessageBox.Show("No update was made \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.rechargeDrone(drone.Id);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't send drone to charge \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnSendToDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DroneToParcel(drone.Id);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't pick up parcel \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnFreeDroneFromCharging_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.FreeDrone(drone.Id, stationId);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't free drone from charging \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnPickUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.PickUpParcel(drone);
            }
            catch(NoMatchingIdException)
            {
                MessageBox.Show("No parcel could be picked-up \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnDeliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveryPackage(drone);
            }
            catch (ImpossibleOprationException)
            {
                MessageBox.Show("Parcel can't be delivered \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void ShowDroneData()
        {
            txtIdData.Text = drone.Id.ToString();
            txtModelData.Text = drone.Model;
            txtWeightData.Text = drone.MaxWeight.ToString();
            txtStatusData.Text = drone.DroneStatus.ToString();
            txtLocationData.Text = drone.CurrentLocation.ToString();
            txtBatteryData.Text = drone.Battery.ToString() + "%";
            txtParcelInDeliveryData.Text = (Convert.ToInt32(drone.ParcelInDelivery.Id) == 0 ? "none" : drone.ParcelInDelivery.ToString());

        }



        private void txtIdData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtIdData.Text = drone.Id.ToString();
        }

        private void txtModelData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtModelData.Text = drone.Model;
        }

        private void txtWeightData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtWeightData.Text = drone.MaxWeight.ToString();
        }

        private void txtStatusData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtStatusData.Text = drone.DroneStatus.ToString();
        }

        private void txtLocationData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLocationData.Text = drone.CurrentLocation.ToString();
        }

        private void txtBatteryData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBatteryData.Text = drone.Battery.ToString() + "%";
        }

        private void txtParcelInDeliveryData_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtParcelInDeliveryData.Text = (Convert.ToInt32(drone.ParcelInDelivery.Id) == 0 ? "none" : drone.ParcelInDelivery.ToString()); 
        }











        //private void ViewCurrentDrone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}


    }
}
