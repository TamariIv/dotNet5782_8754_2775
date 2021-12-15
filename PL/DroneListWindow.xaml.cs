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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL bl;
        public DroneListWindow(IBL.IBL bl)
        {
            this.bl = bl;
            InitializeComponent();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            comboMaxWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }


        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)comboStatusSelector.SelectedItem;
            combineWeightAndStatus();
            //DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == status);
        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)comboMaxWeightSelector.SelectedItem;
            combineWeightAndStatus();
            //DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == weight);
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetListOfDrones();
        }

        private void combineWeightAndStatus()
        {
            //if (weight != default && status != default)
            //    DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == status && d.MaxWeight == weight);
            //else if (weight != default)
            //    DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == weight);
            //else
            //    DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == status);
            DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (IBL.BO.WeightCategories)comboMaxWeightSelector.SelectedItem && d.DroneStatus == (IBL.BO.DroneStatus)comboStatusSelector.SelectedItem);
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IBL.BO.Drone tmpDrone = new IBL.BO.Drone();
            tmpDrone = (Drone)DronesListView.SelectedItem;
            new DroneWindow(bl, bl.GetDrone(Convert.ToInt32(DronesListView.SelectedItem))).ShowDialog();
        }

        private void DronesListView_KeyDown(object sender, KeyEventArgs e)
        {
             
        }
    }
}
