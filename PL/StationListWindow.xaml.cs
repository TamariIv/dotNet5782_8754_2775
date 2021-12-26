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
        public StationListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            StationsListView.ItemsSource = bl.GetListOfStations();

        }

        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList tmpStation = new StationToList();
            tmpStation = (StationToList)StationsListView.SelectedItem;
            StationWindow sw = new StationWindow(bl, tmpStation);
            sw.Closed += Sw_Closed;
            sw.Show();
        }

        private void Sw_Closed(object sender, EventArgs e)
        {
            StationsListView.Items.Refresh();
        }

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

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            StationToList tmpDrone = new StationToList();
            tmpDrone = (StationToList)StationsListView.SelectedItem;
            StationWindow sw = new StationWindow(bl);
            sw.Closed += Sw_Closed;
            sw.Show();
        }
    }
}
