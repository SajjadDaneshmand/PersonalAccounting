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
    /// Interaction logic for GeneralAlert.xaml
    /// </summary>
    public partial class GeneralAlert : Window
    {
        public GeneralAlert()
        {
            InitializeComponent();
        }

        private void CloseWindow_ClickEvent(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        public static void InfoMessageBox(Window owner, string text)
        {
            var alert = new GeneralAlert();
            alert.Owner = owner;
            alert.Title = "Info";
            alert.General_TextBlock.Text = text;
            alert.ShowDialog();

        }
    }
}
