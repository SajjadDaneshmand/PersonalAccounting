using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Services;
using AccountingApp.Views.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AccountingApp.Views
{
    /// <summary>
    /// Interaction logic for TransactionForm.xaml
    /// </summary>
    public partial class TransactionForm : Window
    {
        public int CustomerID;
        public int TrId;
        public DateTime Date;

        public TransactionForm()
        {
            InitializeComponent();
            BindGrid();
        }

        private void NumberValidation_TextBox(object sender, TextCompositionEventArgs e)
        {
            string pattern = "[^0-9]+";
            Regex regex = new Regex(pattern);
            e.Handled = regex.IsMatch(e.Text);
        }
        public void BindGrid()
        {
            using (var db = new UnitOfWork())
            {
                IdNames_DataGrid.ItemsSource = db.CustomerRepository.GetCustomerIdNames();
            }
        }

        private void NameSelectorEvent(object sender, MouseButtonEventArgs e)
        {
            var selectedCustomer = IdNames_DataGrid.SelectedItem as CustomerIdName;
            if (selectedCustomer != null)
            {
                ReadOnlyName_TextBox.Text = selectedCustomer.Name;
                CustomerID = selectedCustomer.Id;
            }

        }

        private void SubmitTr_ClickEvent(object sender, RoutedEventArgs e)
        {

            // Trim inputs
            AmountTextBox.Text = AmountTextBox.Text.Trim();
            TrDescription.Text = TrDescription.Text.Trim();
            Int64 MaxMoneyNum = 922337203685477;
            Int64 Amount;

            int TrType;
            if (Payment_RadioButton.IsChecked == true)
            {
                TrType = 2;
            }
            else
            {
                TrType = 1;
            }



            if (CustomerID == 0)
            {
                GeneralAlert.InfoMessageBox(this, "Please Select Person!");
                return;
            }

            if (AmountTextBox.Text == "")
            {
                GeneralAlert.InfoMessageBox(this, "Please Enter Amount!");
                return;
            }

            Amount = Int64.Parse(AmountTextBox.Text);

            if (Amount > MaxMoneyNum)
            {
                GeneralAlert.InfoMessageBox(this, $"Amount is too big! The most number can accept are {MaxMoneyNum}");
                return;
            }

            Transactions transactions = new Transactions()
            {
                CustomerID = CustomerID,
                TrType = TrType,
                Amount = Amount,
                Description = TrDescription.Text,
            };

            using (var db = new UnitOfWork())
            {
                if (TrId == 0)
                {
                    transactions.Date = DateTime.Now;
                    db.TransactionRepository.Insert(transactions);
                }
                else
                {
                    transactions.ID = TrId;
                    transactions.Date = Date;
                    db.TransactionRepository.Update(transactions);
                    
                }

                db.Save();
            }

            
            this.Close();


        }

        private void HandelCopyPaste(object sender, TextChangedEventArgs e)
        {
            // Remove non-numeric characters from the text
            AmountTextBox.Text = Regex.Replace(AmountTextBox.Text, "[^0-9]", "");
            AmountTextBox.SelectionStart = AmountTextBox.Text.Length;
        }
    }
}
