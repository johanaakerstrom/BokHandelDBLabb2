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
    /// Interaction logic for AddANewBookView.xaml
    /// </summary>
    public partial class AddANewBookView : Window
    {
        public BokHandelDBContext DBConText {  get; set; }
        public Store CurrentStore { get; set; }
        public AddANewBookView(Store currentStore)
        {
            InitializeComponent();
            DBConText = new BokHandelDBContext();
            CurrentStore = currentStore;
            LoadAllStores();
        }

        public void LoadAllStores()
        {
            var allStores = DBConText.Stores.ToList();
            SelectedStoreListBox.ItemsSource = allStores;
        }
        private void SelectedStoreListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentStore = SelectedStoreListBox.SelectedItem as Store;
        }

        private async Task AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentStore != null)
            {
                string Author = AuthorText.Text = string.Empty;
                string Title = TitleText.Text = string.Empty;
                int Price;
                if (int.TryParse(PriceText.Text, out Price))
                {
                    MessageBox.Show("Invalid Price");
                }
                string Language = LanguageText.Text = string.Empty;
                string ISBN = ISBNText.Text = string.Empty;
                DateTime PublicationDate = DateTime.Now;

                var newBook = new Book()
                {
                    Authors = new Author(),
                    Title = Title,
                    Language = Language,
                    Price = Price,
                    Isbn = ISBN,
                    PublicationDate = PublicationDate
                };

                var newInventoryBalance = new InventoryBalance()
                {
                    Isbn = newBook.Isbn,
                    StoreId = CurrentStore.StoreId,
                    Quantity = 1
                };

                await DBConText.Books.AddAsync(newBook);
                await DBConText.InventoryBalances.AddAsync(newInventoryBalance);
            }
            
            await DBConText.SaveChangesAsync();
        }

    }
}
