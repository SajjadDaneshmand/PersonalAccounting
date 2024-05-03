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
using AccountingApp.Views;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;

namespace AccountingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PersonsForm_Button(object sender, RoutedEventArgs e)
        {
            PersonsForm personsForm = new PersonsForm();
            personsForm.Owner = this;
            personsForm.ShowDialog();
        }

        private void TransactionsForm_Button(object sender, RoutedEventArgs e)
        {
            TransactionForm transactionForm = new TransactionForm();
            transactionForm.Owner = this;
            transactionForm.ShowDialog();
        }

        private void ReceiveWindow(object sender, RoutedEventArgs e)
        {
            var ReportWindow = new Report();
            ReportWindow.Owner = this;
            ReportWindow.TypeId = 1;
            ReportWindow.Title = "Receive Report";
            ReportWindow.ShowDialog();
        }

        private void PaymentWindow(object sender, RoutedEventArgs e)
        {
            var ReportWindow = new Report();
            ReportWindow.Owner = this;
            ReportWindow.TypeId = 2;
            ReportWindow.Title = "Payment Report";
            ReportWindow.ShowDialog();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new UnitOfWork())
            {
                string password = db.UserRepository.Get()
                                                .Select(p => p.Password)
                                                .FirstOrDefault();
                if (password != null)
                {
                    this.Hide();
                    LoginForm loginForm = new LoginForm();
                    loginForm.Owner = this;
                    loginForm.IsCompare = true;
                    loginForm.ShowDialog();

                    if (loginForm.IsOk != true)
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
            this.Show();
        }

        private void ManagePasswordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var managePassword = new LoginForm();
            managePassword.Owner = this;
            managePassword.Title = "Manage Password";
            managePassword.LoginSubmit_Button.Content = "Submit";
            managePassword.ShowDialog();

        }
    }
}
