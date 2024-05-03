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

namespace AccountingApp.Views.Alerts
{
    /// <summary>
    /// Interaction logic for YesNoForm.xaml
    /// </summary>
    public partial class YesNoForm : Window
    {
        public bool IsYes;
        public YesNoForm()
        {
            InitializeComponent();
            IsYes = false;
        }

        private void Yes_ClickEvent(object sender, RoutedEventArgs e)
        {
            IsYes = true;
            this.Close();
        }

        private void No_ClickEvent(object sender, RoutedEventArgs e)
        {
            IsYes = false;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                No_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.Enter)
            {
                Yes_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
