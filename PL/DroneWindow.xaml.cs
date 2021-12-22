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
using BlApi;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL bl;
        Drone drone;
        int stationId;

        // add drone ctor
        public DroneWindow(IBL bl)
        {
            this.bl = bl;
            drone = new BO.Drone();
            InitializeComponent();

            comboWeightSelcetor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            List<int> listOfStationIds = new List<int>();
            foreach (var s in bl.GetListOfStationsWithAvailableChargeSlots())
                listOfStationIds.Add(s.Id);
            comboStationSelector.ItemsSource = listOfStationIds;

            ActionsGrid.Visibility = Visibility.Hidden;
        }

        // actions with drone ctor
        public DroneWindow(IBL bl, DroneToList d)
        {
            this.bl = bl;
            drone = bl.GetDrone(d.Id);
            InitializeComponent();
            ShowDroneData();

            AddDroneGrid.Visibility = Visibility.Hidden;

            int status = 0;
            if (d.ParcelInDeliveryId == 0)
            {
                if (d.DroneStatus == DroneStatus.Available)
                    status = 1;
                else status = 2;
            }
            else
            {
                Parcel tmpParcel = bl.GetParcel(d.ParcelInDeliveryId);
                if (tmpParcel.PickedUp == null)
                    status = 3;
                else status = 4;
            }

            switch(status)
            {
                case 1:
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case 3:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case 2:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    btnDeliverParcel.Visibility = Visibility.Hidden;
                    break;

                case 4:
                    btnSendToCharge.Visibility = Visibility.Hidden;
                    btnSendToDelivery.Visibility = Visibility.Hidden;
                    btnFreeDroneFromCharging.Visibility = Visibility.Hidden;
                    btnPickUpParcel.Visibility = Visibility.Hidden;
                    break;
            }    
        }



        // ADD DRONE WINDOW FUNCTIONS 

        /// <summary>
        /// the event saves id the user input in a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged_DroneId(object sender, TextChangedEventArgs e)
        {
            drone.Id = Convert.ToInt32(txtDroneId.Text);
        }

        /// <summary>
        /// function will only allow user to enter numbers
        /// other chars won't e shown in the textbosx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// the event saves the weight the user chose in drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboWeightSelcetor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)Convert.ToInt32(comboWeightSelcetor.SelectedItem);
            drone.MaxWeight = weight;
        }

        /// <summary>
        /// the event saves the model user input in a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged_DroneModel(object sender, TextChangedEventArgs e)
        {
            drone.Model = Convert.ToString(txtDroneModel.Text);
        }

        /// <summary>
        /// the event saves the station the user chose in drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboStationSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            drone.DroneStatus = DroneStatus.Maintenance;
            stationId = Convert.ToInt32(comboStationSelector.SelectedItem);
        }

        /// <summary>
        /// the event sends the new drone to the addDrone method in BL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddDrone(drone, stationId);
                MessageBox.Show("Drone was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
                new DroneListWindow(bl).Show();

                //DroneListWindow newWindow = new DroneListWindow(bl);
                //Application.Current.MainWindow = newWindow;
                //newWindow.Show();
                //this.Close();
            }
            catch (IdAlreadyExistsException)
            {
                MessageBox.Show("Couldn't add drone \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoMatchingIdException)
            {
                MessageBox.Show("Couldn't add drone \npress OK to continue, else press Cancel", "Error Occurred",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //new DroneListWindow(bl).Show();
            //new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
            //DroneListWindow dw = new DroneListWindow(bl)/*.ShowDialog()*/;
            //dw.Closed += dw.RefreshListView;
            //dw.Show();
            //DroneListWindow dlw = new DroneListWindow(bl);
            //dlw.Closed += Dlw_Closed;
        }




        // ACTIONS WITH DRONE FUNCTIONS

        private void btnFinalUpdate_Click(object sender, RoutedEventArgs e)
        {
            Drone tmpDrone = drone;
            tmpDrone.Model = txtModelData.Text;
            try
            {
                bl.UpdateDrone(drone);
                drone = tmpDrone;
                MessageBox.Show($"Drone {drone.Id} model was updated successfully \npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(NoUpdateException)
            {
                MessageBox.Show("No update was made \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.rechargeDrone(drone.Id);
                MessageBox.Show($"Drone {drone.Id} was sent to charge successfully\npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't send drone to charge \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();

        }

        private void btnSendToDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DroneToParcel(drone.Id);
                MessageBox.Show($"Drone {drone.Id} was sent on delivery successfully\npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't pick up parcel \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnFreeDroneFromCharging_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.FreeDrone(drone.Id);
                MessageBox.Show($"Drone {drone.Id} was freed from station successfully\npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(ImpossibleOprationException)
            {
                MessageBox.Show("Couldn't free drone from charging \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnPickUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.PickUpParcel(drone);
                MessageBox.Show($"Drone {drone.Id} picked-up parcel {drone.ParcelInDelivery.Id} successfully\npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(NoMatchingIdException)
            {
                MessageBox.Show("No parcel could be picked-up \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new DroneWindow(bl, bl.GetDroneToList(drone.Id)).Show();
        }

        private void btnDeliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveryPackage(drone);
                MessageBox.Show($"Drone {drone.Id} completed delivery successfully\npress OK to continue", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ImpossibleOprationException)
            {
                MessageBox.Show("Parcel can't be delivered \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Error \npress OK to continue, else press Cancel", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            txtParcelInDeliveryData.Text = Convert.ToInt32(drone.ParcelInDelivery.Id) == 0 ? "none" : drone.ParcelInDelivery.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //new DroneListWindow(bl).Show();
        }


    }
}
