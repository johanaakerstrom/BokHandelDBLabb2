using BokHandelDBLabb2.Data;
using BokHandelDBLabb2.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BokHandelDBLabb2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BokHandelDBContext dBContext;
        public List<Store> stores = new List<Store>();
        public MainWindow()
        {
            InitializeComponent();
            dBContext = new BokHandelDBContext();
            LoadStores();
        }

        public void LoadStores()
        {
            var allStores = dBContext.Stores.ToList();
            AllStoresListBox.ItemsSource = allStores;
        }

        public void AllStores()
        {
            List<Store> allStoreNames = new List<Store>();
            allStoreNames.AddRange(stores);
            AllStoresListBox.ItemsSource = allStoreNames;
        }

        private void StoreInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStore = AllStoresListBox.SelectedItem as Store;
            if (selectedStore != null)
            {

                StoreInventoryView showStoreInventory = new StoreInventoryView(selectedStore);
                showStoreInventory.Show();
                this.Close();
            } else
            {
                MessageBox.Show("You need to select a store to see the inventory");
            }
        }
    }
}
