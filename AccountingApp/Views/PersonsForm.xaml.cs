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
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;
using AccountingApp.Views.Alerts;

namespace AccountingApp.Views
{
    /// <summary>
    /// Interaction logic for PersonsForm.xaml
    /// </summary>
    public partial class PersonsForm : Window
    {
        public PersonsForm()
        {
            InitializeComponent();
            BindGrid();
        }

        public void BindGrid()
        {
            using (var db = new UnitOfWork())
            {
                Customer_DataGrid.ItemsSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void Refresh_ClickEvent(object sender, RoutedEventArgs e)
        {
            BindGrid();
            Search.Clear();
        }

        private void Search_TextChangeEvent(object sender, TextChangedEventArgs e)
        {
            using (var db = new UnitOfWork())
            {
                var FilteredCustomers = db.CustomerRepository.GetCustomersByFilter(Search.Text);
                Customer_DataGrid.ItemsSource = FilteredCustomers;
            }
        }

        private void LoadAddWindow_ClickEvent(object sender, RoutedEventArgs e)
        {
            var AddPersonFormWindow = new AddPersonForm();
            AddPersonFormWindow.Owner = this;
            AddPersonFormWindow.ShowDialog();
            BindGrid();
        }

        private void DeleteCustomer_ClickEvent(object sender, RoutedEventArgs e)
        {
            Customers selectedCustomer = Customer_DataGrid.SelectedItem as Customers;

            if (selectedCustomer == null)
            {
                GeneralAlert.InfoMessageBox(this, "Please select row...");
                BindGrid();
                return;
            }

            // Ensuring user confidence
            var yesNoForm = new YesNoForm();
            yesNoForm.Owner = this;
            yesNoForm.General_TextBlock.Text = $"Are you sure you want to delete {selectedCustomer.FullName}?";
            yesNoForm.ShowDialog();


            if (yesNoForm.IsYes)
            {
                using (var db = new UnitOfWork())
                {
                    bool checkHasTr = db.TransactionRepository.Get(c => c.CustomerID == selectedCustomer.CustomerID).Any();

                    if (!checkHasTr)
                    {
                        db.CustomerRepository.DeleteCustomer(selectedCustomer);
                        db.Save();
                    }
                    else
                    {
                        GeneralAlert.InfoMessageBox(this, "This person has transaction. you can't remove it while you remove his/her Transactions!");
                    }

                }
            }

            BindGrid();
        }

        private void EditPerson_ClickEvent(object sender, RoutedEventArgs e)
        {
            Customers selectedCustomer = Customer_DataGrid.SelectedItem as Customers;

            if (selectedCustomer == null)
            {
                GeneralAlert.InfoMessageBox(this, "Please select row...");
                BindGrid();
                return;
            }

            var editPerson = new AddPersonForm();
            editPerson.Owner = this;
            editPerson.customerId = selectedCustomer.CustomerID;
            editPerson.Title = "Edit";
            editPerson.FullName_TextBox.Text = selectedCustomer.FullName;
            editPerson.Mobile_TextBox.Text = selectedCustomer.Mobile;
            editPerson.Email_TextBox.Text = selectedCustomer.EMail;
            editPerson.Address_TextBox.Text = selectedCustomer.Address;
            editPerson.ShowDialog();

            BindGrid();




        }

        private void EditViaDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCustomer = Customer_DataGrid.SelectedItem as Customers;
            if (selectedCustomer != null)
            {
                EditPerson_ClickEvent(sender, e);
            }
        }
    }
}
