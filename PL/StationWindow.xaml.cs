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
using System.Windows.Shapes;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private IBL bl;
        Station station;

        public StationWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            station = new Station();
            StationDetailsGrid.Visibility = Visibility.Hidden;
        }

        public StationWindow(IBL bl, BO.StationToList station)
        {
            InitializeComponent();
            this.bl = bl;
            this.station = bl.GetStation(station.Id);
            DataContext = this.station;
            AddStationGrid.Visibility = Visibility.Hidden;
        }

        // STATION DETAILS FUNCTIONS

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateStation(station.Id, txtNameData.Text, Convert.ToInt32(txtAvailableSlotsData.Text));
            }
            catch (NoMatchingIdException)
            {
                MessageBox.Show($"No station with ID {station.Id} could be updated \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoUpdateException)
            {
                MessageBox.Show("No update was made \npress OK to continue", "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new StationWindow(bl, bl.GetStationToList(station.Id)).Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // ADD STATION FUNCTIONS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Station tmpStation = new Station()
                {
                    Id = Convert.ToInt32(txtEnterId.Text),
                    Name = txtEnterName.Text,
                    Location = new Location() { Latitude = Convert.ToDouble(txtEnterLatitude.Text), Longitude = Convert.ToDouble(txtEnterLongitude.Text) },
                    AvailableChargeSlots = Convert.ToInt32(txtEnterAvailableSlots.Text)
                };


                bl.AddStation(tmpStation);
                station = tmpStation;
                MessageBox.Show("Station was added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
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
                MessageBox.Show("Insert details of the station!", "Error Occurred",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listvDronesChargingData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(bl, bl.GetDroneToList(((DroneInCharging)listvDronesChargingData.SelectedItem).Id)).Show();
        }
    }
}
