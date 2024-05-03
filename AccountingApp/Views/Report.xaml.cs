using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Services;
using AccountingApp.Views.Alerts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AccountingApp.Views
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public int TypeId;

        public Report()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void BindComboBox()
        {
            using (var db = new UnitOfWork())
            {
                var Names = db.CustomerRepository.GetCustomerIdNames();
                Names.Add(new CustomerIdName { Id = 0, Name = "All Persons" });
                Names.Reverse();
                Counterparty_ComboBox.Items.Clear();
                Counterparty_ComboBox.ItemsSource = Names;
            }
        }

        public void BindGrid()
        {

            using (var db = new UnitOfWork())
            {

                var result = db.TransactionRepository.GetTransactionsWithName()
                                                     .Where
                                                     (n =>
                                                        n.TrType == TypeId &&
                                                        n.Date.Date >= FromDate_DataPicker.SelectedDate && n.Date.Date <= ToDate_DataPicker.SelectedDate
                                                        )
                                                     .ToList();

                AmountSum_Label.Content = (Int64)result.Select(n => n.Amount).Sum();
                Transactions_DataGrid.ItemsSource = result;
            }
        }

        public void SetFiltersToDefault()
        {
            Counterparty_ComboBox.SelectedValue = 0;
        }
        public void Filter()
        {

            if ((int)Counterparty_ComboBox.SelectedValue == 0)
            {
                BindGrid();
            }
            else
            {
                using (var db = new UnitOfWork())
                {
                    var result = db.TransactionRepository.GetTransactionsWithName()
                                         .Where
                                         (n =>
                                            n.TrType == TypeId &&
                                            n.CustomerID == (int)Counterparty_ComboBox.SelectedValue &&
                                            n.Date.Date.Date >= FromDate_DataPicker.SelectedDate && n.Date.Date <= ToDate_DataPicker.SelectedDate

                                         )
                                         .ToList();

                    AmountSum_Label.Content = (Int64)result.Select(n => n.Amount).Sum();
                    Transactions_DataGrid.ItemsSource = result;
                }
            }
        }

        private void LoadTrEditWindow_ClickEvent(object sender, RoutedEventArgs e)
        {
            var selectedRow = (TransactionsNames)Transactions_DataGrid.SelectedItem;

            if (selectedRow == null)
            {
                GeneralAlert.InfoMessageBox(this, "please Select Row!");
                return;
            }

            TransactionForm transactionForm = new TransactionForm();
            transactionForm.Owner = this;
            transactionForm.TrId = selectedRow.ID;
            transactionForm.Date = selectedRow.Date;
            transactionForm.Title = "Edit Transaction";
            transactionForm.ReadOnlyName_TextBox.Text = selectedRow.Name;
            transactionForm.CustomerID = selectedRow.CustomerID;
            transactionForm.ReadOnlyName_TextBox.Text = selectedRow.Name;

            int integerAmount = (int)selectedRow.Amount;

            transactionForm.AmountTextBox.Text = integerAmount.ToString();
            transactionForm.TrDescription.Text = selectedRow.Description;
            if (selectedRow.TrType == 1)
            {
                transactionForm.Receive_RadioButton.IsChecked = true;
            }
            else
            {
                transactionForm.Payment_RadioButton.IsChecked = true;
            }

            transactionForm.ShowDialog();
            BindGrid();
        }

        private void DeleteTr_ClickEvent(object sender, RoutedEventArgs e)
        {
            var selectedRow = (TransactionsNames)Transactions_DataGrid.SelectedItem;

            if (selectedRow != null)
            {
                using (var db = new UnitOfWork())
                {
                    db.TransactionRepository.Delete(selectedRow.ID);
                    db.Save();
                }

                GeneralAlert.InfoMessageBox(this, "Successfully Removerd!");
                BindGrid();
            }
            else
            {
                GeneralAlert.InfoMessageBox(this, "Please Select Row!");
            }


        }

        private void RefreshTr_ClickEvent(object sender, RoutedEventArgs e)
        {
            BindGrid();
            SetFiltersToDefault();
        }

        private void EditViaDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadTrEditWindow_ClickEvent(sender, e);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FromDate_DataPicker.SelectedDate = new DateTime(DateTime.Today.Year, 01, 01);
            ToDate_DataPicker.SelectedDate = DateTime.Today;

            BindGrid();
            BindComboBox();
        }


        private void FilterSubmit_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }
    }
}
