using Accounting.DataLayer.Context;
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
using System.Security.Cryptography;
using Accounting.utility;
using AccountingApp.Views.Alerts;
using Accounting.DataLayer;

namespace AccountingApp.Views
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public bool IsOk;
        public bool IsCompare;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginSubmit_ClickEvent(object sender, RoutedEventArgs e)
        {
            if (IsCompare == true)
            {
                if (compare())
                {
                    IsOk = true;
                    this.Close();
                }
                return;
            }

            using (var db = new UnitOfWork())
            {
                var password = db.UserRepository.Get().FirstOrDefault();
                
                if (password == null)
                {
                    if (insertPassword())
                    {
                        IsOk = true;
                        this.Close();
                    }
                    return;
                }
                else
                {
                    if (updatePassword())
                    {
                        IsOk = true;
                        this.Close();
                    }
                    return;
                }
            }
        }


        private bool compare()
        {
            using (var db = new UnitOfWork())
            {
                string password = db.UserRepository.Get().Select(p => p.Password).First();
                string inputPasswordHashed = Utils.ComputeSHA256Hash(PasswordBox.Password);

                if (password == inputPasswordHashed)
                {
                    return true;
                }
                else
                {
                    GeneralAlert.InfoMessageBox(this, "Wrong Password!");
                    return false;
                }

            }
        }

        private bool insertPassword()
        {
            string inputPassword = PasswordBox.Password.Trim();
            if (inputPassword == "")
            {
                GeneralAlert.InfoMessageBox(this, "PasswordBox can't be empty!");
                return false;
            }

            string hashedPassword = Utils.ComputeSHA256Hash(inputPassword);

            using (var db = new UnitOfWork())
            {
                var user = new User { Password = hashedPassword };
                db.UserRepository.Insert(user);
                db.Save();
                GeneralAlert.InfoMessageBox(this, "Successfully Password Created!");
            }

            return true;
        }

        private bool updatePassword()
        {
            string inputPassword = PasswordBox.Password.Trim();

            using (var db = new UnitOfWork())
            {
                var password = db.UserRepository.Get().First();

                if (inputPassword == "")
                {
                    GeneralAlert.InfoMessageBox(this, "PasswordBox can't be empty!");
                    return false;
                }

                string hashedPassword = Utils.ComputeSHA256Hash(inputPassword);

                password.Password = hashedPassword;

                db.UserRepository.Update(password);
                db.Save();
                GeneralAlert.InfoMessageBox(this, "Successfully Password Update!");
            }

            return true;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginSubmit_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox.Focus();
        }
    }
}
