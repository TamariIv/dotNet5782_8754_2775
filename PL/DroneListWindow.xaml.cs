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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL bl;
        public DroneListWindow(IBL bl)
        {
            this.bl = bl;
            InitializeComponent();
            DronesListView.ItemsSource = bl.GetListOfDrones();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            comboMaxWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetListOfDrones();
            //DronesListView.ItemsSource = from item in bl.GetListOfDrones()
            //                             group item by item.DroneStatus;
        }

        public void comboCombineStatusAndWeight_SelectionChanged(/*object sender, SelectionChangedEventArgs e*/)
        {
            if (comboMaxWeightSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (WeightCategories)comboMaxWeightSelector.SelectedItem && d.DroneStatus == (DroneStatus)comboStatusSelector.SelectedItem);
            else if (comboMaxWeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (WeightCategories)comboMaxWeightSelector.SelectedItem);
            else if (comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == (DroneStatus)comboStatusSelector.SelectedItem);
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new DroneWindow(bl).Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList tmpDrone = new DroneToList();
            tmpDrone = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl, tmpDrone).ShowDialog();
            this.Close();
           
        }

        private void DronesListView_KeyDown(object sender, KeyEventArgs e)
        {
             
        }

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboCombineStatusAndWeight_SelectionChanged();
        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboCombineStatusAndWeight_SelectionChanged();
        }

        private void btnClearWeight_Click(object sender, RoutedEventArgs e)
        {
            //comboMaxWeightSelector.SelectedItem = null;
            DronesListView.ItemsSource = bl.GetListOfDrones();
            //comboCombineStatusAndWeight_SelectionChanged();
        }
    }
}
