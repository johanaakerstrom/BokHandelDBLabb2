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
        public List<Author> AuthorList  { get; set; } = new List<Author>();
        public AddANewBookView(Store currentStore)
        {
            InitializeComponent();
            DBConText = new BokHandelDBContext();
            CurrentStore = currentStore;
            AuthorList = DBConText.Authors.ToList();
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

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {

            AddANewBook();
        }

        private async Task AddANewBook() 
        {
            if (CurrentStore != null)
            {
                string AuthorFirstName = FirstNameAuthorText.Text;
                string AuthorLastName = LastNameAuthorText.Text;
                string Title = TitleText.Text;
                var Author = AuthorList.FirstOrDefault(a => a.FirstName == AuthorFirstName);
                decimal Price = PriceText.Text.Length;               
                string Language = LanguageText.Text;
                string ISBN = ISBNText.Text;
                DateTime PublicationDate = DateTime.Now;
                int Quantity = QuantityText.Text.Length;

                var newBook = new Book()
                {
                    Authors = new Author { FirstName = AuthorFirstName, LastName = AuthorLastName},
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
                    Quantity = Quantity
                };

                await DBConText.Books.AddAsync(newBook);
                await DBConText.InventoryBalances.AddAsync(newInventoryBalance);
            }

            await DBConText.SaveChangesAsync();
        }

    }
}
