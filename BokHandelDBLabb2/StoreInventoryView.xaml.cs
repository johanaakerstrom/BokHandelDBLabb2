using BokHandelDBLabb2.Data;
using BokHandelDBLabb2.Models;
using Microsoft.EntityFrameworkCore;
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

namespace BokHandelDBLabb2
{
    /// <summary>
    /// Interaction logic for StoreInventoryView.xaml
    /// </summary>
    public partial class StoreInventoryView : Window
    {
        BokHandelDBContext dBContext;
        public Store CurrenStore { get; set; }
        Store CurrentStore;
        
        public Book Book { get; set; }
        public StoreInventoryView(Store data)
        {
            CurrenStore = data;
            InitializeComponent();
            dBContext = new BokHandelDBContext();
            ShowStoreName();
            LoadStoreInventory();
            
        }

        public async Task LoadStoreInventory()
        {       
            var StoreInventory = await dBContext.Books.Include(b => b.InventoryBalances).Where(b => b.InventoryBalances.Any(i => i.StoreId == CurrenStore.StoreId)).ToListAsync();
            CurrentStoreInventoryListBox.ItemsSource = StoreInventory;
            var allBooks = await dBContext.Books.Include(b => b.Authors).ToListAsync();
            AllBooksListBox.ItemsSource = allBooks;
            
        }

        public void ShowStoreName()
        {
            CurrenStoreNameText.Text = $"{CurrenStore.StoreName}'s Inventory";
        }

        private void BackToStoreMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow backToMenu = new MainWindow();
            backToMenu.Show();
            this.Close();
        }

        private void AddBookToStoreButton_Click(object sender, RoutedEventArgs e)
        {
            var addNewBook = AllBooksListBox.SelectedItem as Book;
            if (addNewBook != null)
            {
                var existingInventory = dBContext.InventoryBalances.FirstOrDefault(i => i.Isbn == addNewBook.Isbn && i.StoreId == CurrenStore.StoreId);
                if(existingInventory == null)
                {
                    var newInventoryBalance = new InventoryBalance()
                    {
                        Isbn = addNewBook.Isbn,
                        StoreId = CurrenStore.StoreId,
                        Quantity = 1
                    };
                    dBContext.InventoryBalances.Add(newInventoryBalance);
                    dBContext.SaveChanges();
                    LoadStoreInventory();
                }
                else
                {
                    
                    existingInventory.Quantity++;
                    dBContext.SaveChanges();
                    LoadStoreInventory();
                }
            }
                
        }

        private void DeleteBookFromStore_Click(object sender, RoutedEventArgs e)
        {
            var removeBook = CurrentStoreInventoryListBox.SelectedItem as Book;
            if (removeBook != null)
            {
                var bookToRemove = dBContext.InventoryBalances.FirstOrDefault(b => b.Isbn == removeBook.Isbn);
                if (bookToRemove != null)
                {
                    dBContext.InventoryBalances.Remove(bookToRemove);
                    dBContext.SaveChanges();
                    LoadStoreInventory();
                }
                else 
                {
                    bookToRemove.Quantity--;
                    dBContext.SaveChanges();
                }
            }
        }
    }
}
