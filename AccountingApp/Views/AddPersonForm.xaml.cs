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
using AccountingApp.Views.Alerts;
using Accounting.DataLayer;
using System.Text.RegularExpressions;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Services;

namespace AccountingApp.Views
{
    /// <summary>
    /// Interaction logic for AddPersonForm.xaml
    /// </summary>
    public partial class AddPersonForm : Window
    {

        string InfoWindowDefaultText = "Please check the input (Email or Mobile not valid!)";
        string InfoWindowEmptyValue = "Person's name can't be empty!";

        public int customerId = 0;

        public AddPersonForm()
        {
            InitializeComponent();
        }

        private void Submit_ClickEvent(object sender, RoutedEventArgs e)
        {

            // Person Details Array
            string[] PersonDetail =
            {
                FullName_TextBox.Text.Trim(),
                Mobile_TextBox.Text.Trim(),
                Email_TextBox.Text.Trim(),
                Address_TextBox.Text.Trim(),
            };

            // Check item validation
            if (PersonDetail[0] == "")
            {
                GeneralAlert.InfoMessageBox(this, InfoWindowEmptyValue);
                return;
            }


            if (!CheckMobile(PersonDetail[1]))
            {

                GeneralAlert.InfoMessageBox(this, InfoWindowDefaultText);
                
                return;
            }
            else if (!CheckEmail(PersonDetail[2]) && PersonDetail[2] != "")
            {
                GeneralAlert.InfoMessageBox(this, InfoWindowDefaultText);

                return;
            }

            // Insert Into Database

            Customers customers = new Customers()
            {
                FullName = PersonDetail[0],
                Mobile = PersonDetail[1],
                EMail = PersonDetail[2],
                Address = PersonDetail[3],
            };

            using (var db = new UnitOfWork())
            {
                if (customerId == 0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                }
                else
                {
                    customers.CustomerID = customerId;
                    db.CustomerRepository.UpdateCustomer(customers);
                }
                
                db.Save();
            }

            this.Close();

        }

        private bool CheckMobile(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckEmail(string Email)
        {
            string pattern = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\" +
                "x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9]" +
                "(?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-" +
                "9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c" +
                "\\x0e-\\x7f])+)\\])";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(Email);

        }

        private void NumberValidation_TextBox(object sender, TextCompositionEventArgs e)
        {
            string pattern = "^[0-9]+$";
            Regex regex = new Regex(pattern);
            
        }

        private void HandelCopyPaste(object sender, TextChangedEventArgs e)
        {
            // Remove non-numeric characters from the text
            Mobile_TextBox.Text = Regex.Replace(Mobile_TextBox.Text, "[^0-9]", "");
            Mobile_TextBox.SelectionStart = Mobile_TextBox.Text.Length;
        }
    }
}
