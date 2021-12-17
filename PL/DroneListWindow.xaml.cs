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

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetListOfDrones();
        }

        private void comboCombineStatusAndWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboMaxWeightSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (IBL.BO.WeightCategories)comboMaxWeightSelector.SelectedItem && d.DroneStatus == (IBL.BO.DroneStatus)comboStatusSelector.SelectedItem);
            else if (comboMaxWeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (IBL.BO.WeightCategories)comboMaxWeightSelector.SelectedItem);
            else if (comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == (IBL.BO.DroneStatus)comboStatusSelector.SelectedItem);
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
            DroneToList tmpDrone = new DroneToList();
            tmpDrone = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl, tmpDrone).ShowDialog();
           
        }

        private void DronesListView_KeyDown(object sender, KeyEventArgs e)
        {
             
        }
    }
}
