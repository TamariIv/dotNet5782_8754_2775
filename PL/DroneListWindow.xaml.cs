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
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));
            comboMaxWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }


        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)comboStatusSelector.SelectedItem;
            DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == status); 
        }

        private void comboMaxWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)comboMaxWeightSelector.SelectedItem;
            DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == weight);
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetListOfDrones();
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
