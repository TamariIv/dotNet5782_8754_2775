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
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        IBL bl;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bl">instance of bl</param>
        public StationListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            StationsListView.ItemsSource = bl.GetListOfStations();
        }

        /// <summary>
        /// the function receives an event of double clicking on a station and opens that station details page
        /// </summary>
        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList tmpStation = new StationToList();
            tmpStation = (StationToList)StationsListView.SelectedItem;
            StationWindow sw = new StationWindow(bl, tmpStation);
            sw.Closed += Sw_Closed;
            sw.Show();
        }

        /// <summary>
        /// the function is called before a window is closed and takes care of refreshing the list
        /// </summary>
        private void Sw_Closed(object sender, EventArgs e)
        {
            StationsListView.Items.Refresh();
            StationsListView.ItemsSource = bl.GetListOfStations();
        }

        /// <summary>
        /// the combobox orders the list by the requested order
        /// </summary>
        private void comboChooseSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)comboChooseSort.SelectedItem).Content.ToString() == "Only available stations")
            {
                StationsListView.ItemsSource = from s in bl.GetListOfStations()
                                               where s.AvailableChargeSlots > 0
                                               select s;
            }
            else if (((ComboBoxItem)comboChooseSort.SelectedItem).Content.ToString() == "Available slots: Low to High")
            {
                StationsListView.ItemsSource = from s in bl.GetListOfStations()
                                               orderby s.AvailableChargeSlots
                                               select s;
            }
            else if (((ComboBoxItem)comboChooseSort.SelectedItem).Content.ToString() == "Available slots: High to Low")
            {
                StationsListView.ItemsSource = from s in bl.GetListOfStations()
                                               orderby s.AvailableChargeSlots descending
                                               select s;
            }

        }

        /// <summary>
        /// the function opens an add station page
        /// </summary>
        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            StationToList tmpDrone = new StationToList();
            tmpDrone = (StationToList)StationsListView.SelectedItem;
            StationWindow sw = new StationWindow(bl);
            sw.Closed += Sw_Closed;
            sw.Show();
        }

        /// <summary>
        /// clear the oreders and return to the original list order
        /// </summary>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bl.GetListOfStations();
        }
    }
}
