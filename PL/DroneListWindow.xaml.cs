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

        private void comboCombineStatusAndWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cboxStatusSort.IsChecked = false;
            if (comboMaxWeightSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (WeightCategories)comboMaxWeightSelector.SelectedItem && d.DroneStatus == (DroneStatus)comboStatusSelector.SelectedItem);
            else if (comboMaxWeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.MaxWeight == (WeightCategories)comboMaxWeightSelector.SelectedItem);
            else if (comboStatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetListOfDrones().Where(d => d.DroneStatus == (DroneStatus)comboStatusSelector.SelectedItem);
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneToList tmpDrone = new DroneToList();
            tmpDrone = (DroneToList)DronesListView.SelectedItem;
            DroneWindow dw = new DroneWindow(bl);
            dw.Closed += Dw_Closed;
            dw.Show();            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList tmpDrone = new DroneToList();
            tmpDrone = (DroneToList)DronesListView.SelectedItem;
            DroneWindow  dw = new DroneWindow(bl, tmpDrone);
            dw.Closed += Dw_Closed;
            dw.Show();           
        }

        private void Dw_Closed(object sender, EventArgs e)
        {
            DronesListView.Items.Refresh();
            comboCombineStatusAndWeight_SelectionChanged(this, null);
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cboxStatusSort.IsChecked = false;
            DronesListView.ItemsSource = bl.GetListOfDrones();
        }

        private void cboxStatusSort_Checked(object sender, RoutedEventArgs e)
        {
            var kukus = from dr in bl.GetListOfDrones()
                        group dr by dr.DroneStatus into g
                        select g;

           // DronesListView.ItemsSource = kukus;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            view.GroupDescriptions.Add(groupDescription);


            //var result = from d in bl.GetListOfDrones()
            //             group d by d.DroneStatus into g
            //             select new { status = g.Key, Drones = g };
            //DronesListView.ItemsSource = result;

            //DronesListView.ItemsSource = from item in bl.GetListOfDrones()
            //                             group item by item.DroneStatus;

            //DronesListView.ItemsSource = from item in bl.GetListOfDrones()
            //             group item by item.DroneStatus;
            //DronesListView.ItemsSource = (CollectionView)DronesListView.ItemsSource;
        }

        private void cboxStatusSort_Unchecked(object sender, RoutedEventArgs e)
        {

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            view.GroupDescriptions.Clear();
        }
    }
}
