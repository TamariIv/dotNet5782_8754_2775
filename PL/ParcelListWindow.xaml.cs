using BlApi;
using BO;
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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        IBL bl;
        public ParcelListWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;

            ParcelsListView.ItemsSource = bl.GetListofParcels();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            comboPrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList tmpParcel = new ParcelToList();
            tmpParcel = (ParcelToList)ParcelsListView.SelectedItem;
            ParcelWindow pw = new ParcelWindow(bl, bl.GetParcel(tmpParcel.Id));
            pw.Closed += Pw_Closed;
            pw.Show();
        }
        private void Pw_Closed(object sender, EventArgs e)
        {
            ParcelsListView.Items.Refresh();
            ParcelsListView.ItemsSource = bl.GetListofParcels();

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelToList parcel = new ParcelToList();
            parcel = (ParcelToList)ParcelsListView.SelectedItem;
            ParcelWindow pw = new ParcelWindow(bl);
            pw.Closed += Pw_Closed;
            pw.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void comboCombineAllFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboWeightSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null && comboPrioritySelector.SelectedItem != null)
                ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.Weight == (WeightCategories)comboWeightSelector.SelectedItem && p.ParcelStatus == (ParcelStatus)comboStatusSelector.SelectedItem && p.Priority == (Priorities)comboPrioritySelector.SelectedItem);
            else if (comboWeightSelector.SelectedItem != null && comboStatusSelector.SelectedItem != null)
                ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.Weight == (WeightCategories)comboWeightSelector.SelectedItem && p.ParcelStatus == (ParcelStatus)comboStatusSelector.SelectedItem);
            else if (comboWeightSelector.SelectedItem != null && comboPrioritySelector.SelectedItem != null)
                ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.Weight == (WeightCategories)comboWeightSelector.SelectedItem && p.Priority == (Priorities)comboPrioritySelector.SelectedItem);
            else if (comboStatusSelector.SelectedItem != null && comboPrioritySelector.SelectedItem != null)
                ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.ParcelStatus == (ParcelStatus)comboStatusSelector.SelectedItem && p.Priority == (Priorities)comboPrioritySelector.SelectedItem);
            else
            {
                if (comboWeightSelector.SelectedItem != null)
                    ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.Weight == (WeightCategories)comboWeightSelector.SelectedItem);
                if (comboStatusSelector.SelectedItem != null)
                    ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.ParcelStatus == (ParcelStatus)comboStatusSelector.SelectedItem);
                if (comboPrioritySelector.SelectedItem != null)
                    ParcelsListView.ItemsSource = bl.GetListofParcels().Where(p => p.Priority == (Priorities)comboPrioritySelector.SelectedItem);
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //comboWeightSelector. = Content;
            //comboStatusSelector.SelectedItem = Content;
            //comboPrioritySelector.SelectedItem = Content;
            ParcelsListView.ItemsSource = bl.GetListofParcels();
        }
    }
}
