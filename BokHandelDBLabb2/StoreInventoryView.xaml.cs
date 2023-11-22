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
            //var StoreInventory = await dBContext.Books.Include(b => b.InventoryBalances).Where(b => b.InventoryBalances.Any(i => i.StoreId == CurrenStore.StoreId)).ToListAsync();
            var storeInventory = await dBContext.Books.Where(b => b.InventoryBalances.Any(i => i.StoreId == CurrenStore.StoreId)).Include(b => b.InventoryBalances.Where(i => i.StoreId == CurrenStore.StoreId)).ToListAsync(); // Include only the relevant InventoryBalances.ToListAsync();
            CurrentStoreInventoryListBox.ItemsSource = storeInventory;
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
                // Check if the book already exists in the inventory of the current store
                var existingInventory = dBContext.InventoryBalances
                    .FirstOrDefault(i => i.Isbn == addNewBook.Isbn && i.StoreId == CurrenStore.StoreId);

                if (existingInventory == null)
                {
                    // If the book is not found in the current store, add a new entry
                    var newInventoryBalance = new InventoryBalance
                    {
                        Isbn = addNewBook.Isbn,
                        StoreId = CurrenStore.StoreId,
                        Quantity = 1 // Starting with a quantity of 1
                    };
                    dBContext.InventoryBalances.Add(newInventoryBalance);
                }
                else
                {
                    // If the book exists, just update the quantity
                    existingInventory.Quantity++;
                }

                // Save the changes to the database
                dBContext.SaveChanges();

                // Reload the store inventory to reflect the changes
                LoadStoreInventory();
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
                    if (bookToRemove.Quantity > 1)
                    {
                        bookToRemove.Quantity--;
                    }
                    else
                    {
                        dBContext.InventoryBalances.Remove(bookToRemove);
                    }
                    dBContext.SaveChanges();
                    LoadStoreInventory();
                }
            }
        }

        private void AddANewBookButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.CurrenStore != null)
            {
                AddANewBookView BookInformation = new AddANewBookView(CurrenStore);
                BookInformation.CurrentStore = this.CurrentStore;
                BookInformation.Show();
                this.Close();
            }
        }
    }
}
