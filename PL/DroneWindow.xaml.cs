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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL bl;
        IBL.BO.Drone drone;
        int stationId;
        public DroneWindow(IBL.IBL bl)
        {
            this.bl = bl;
            drone = new IBL.BO.Drone();
            InitializeComponent();

            DataContext = drone;
            comboWeightSelcetor.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));

            List<int> listOfStationIds = new List<int>();
            foreach (var s in bl.GetListOfStations(item=> item.AvailableChargeSlots > 0))
                listOfStationIds.Add(s.Id);
            comboStationSelector.ItemsSource = listOfStationIds;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            drone.Id = Convert.ToInt32(txtDroneId.Text); 
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.WeightCategories weight = (IBL.BO.WeightCategories)Convert.ToInt32(comboWeightSelcetor.SelectedItem);
            drone.MaxWeight = weight;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            drone.Model = Convert.ToString(txtDroneModel.Text);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
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
                    MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            MessageBox.Show(bl.GetDroneToList(drone.Id).ToString(), "Drone Details");
            new DroneListWindow(bl).Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
