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

        /// <summary>
        /// add station constructor
        /// </summary>
        /// <param name="bl">instance of bl</param>
        public StationWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            station = new Station();
            StationDetailsGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// station details constructor
        /// </summary>
        /// <param name="bl">instance of bl</param>
        /// <param name="station">the station that is going to be diplayed</param>
        public StationWindow(IBL bl, StationToList station)
        {
            InitializeComponent();
            this.bl = bl;
            this.station = bl.GetStation(station.Id);
            DataContext = this.station;
            AddStationGrid.Visibility = Visibility.Hidden; // hide the grid for adding station
            if(!station.isActive) //if this station is deleted, don't enable the user to delete or update this station
            {
                btnDelete.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }

        // STATION DETAILS FUNCTIONS

        /// <summary>
        /// the function sends the station with the updated field to update function in bl
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateStation(station.Id, txtNameData.Text, Convert.ToInt32(txtAvailableSlotsData.Text));
                MessageBox.Show($"Station {station.Id} was updated successfully", "Success",
                   MessageBoxButton.OK, MessageBoxImage.Information);
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
            catch (ImpossibleOprationException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
            new StationWindow(bl, bl.GetStationToList(station.Id)).Show(); 
        }

        /// <summary>
        /// the function receives the event clicl on the close button and closes the wundow
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ADD STATION FUNCTIONS

        /// <summary>
        /// the function receives the event clicl on the close button and closes the wundow
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// the function collects the data that was inserted, creates a new station with the data and sends to bl to add
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEnterId.Text) || string.IsNullOrEmpty(txtEnterName.Text) || string.IsNullOrEmpty(txtEnterLatitude.Text) || string.IsNullOrEmpty(txtEnterLongitude.Text) || string.IsNullOrEmpty(txtEnterAvailableSlots.Text))
                    throw new EmptyInputException("Insert all details of the station!");
                if (Convert.ToDouble(txtEnterLatitude.Text) < 31.79 || Convert.ToDouble(txtEnterLatitude.Text) > 31.81
                        || Convert.ToInt32(txtEnterLongitude.Text) < 35.1 || Convert.ToInt32(txtEnterLongitude.Text) > 35.21)
                    throw new InvalidInputException("The longitude or latitude are not valid\n Location should be in Jerusalem");

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
            catch(InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Error Occurred", "Error Occurred",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// the function receives an event of double clicking on a drone in the list of dronesthat are being charged
        /// and opens that drone details page
        /// </summary>
        private void listvDronesChargingData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(bl, bl.GetDroneToList(((DroneInCharging)listvDronesChargingData.SelectedItem).Id)).Show();
        }

        /// <summary>
        /// the button deletes the current stations
        /// </summary>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteStation(station.Id);
                MessageBox.Show($"Stsation {station.Id} was deleted succesffuly", "success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (BO.NoMatchingIdException ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
            new StationWindow(bl, bl.GetStationToList(station.Id)).Show();
        }
    }
}
