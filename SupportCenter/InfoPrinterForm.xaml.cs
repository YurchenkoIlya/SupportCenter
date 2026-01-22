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

namespace SupportCenter
{
    /// <summary>
    /// Логика взаимодействия для InfoPrinterForm.xaml
    /// </summary>
    public partial class InfoPrinterForm : Window
    {
        public InfoPrinterForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (TabItem tab in printersTabControl.Items)
            {
                tab.Visibility = Visibility.Collapsed;
            }
        }

        private void appealProgramShow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HP430Button_Click(object sender, RoutedEventArgs e)
        {
            printersTabControl.SelectedIndex = 0;
        }

        private void HP428Button_Click(object sender, RoutedEventArgs e)
        {
            printersTabControl.SelectedIndex = 1;
        }

        private void HP426Button_Click(object sender, RoutedEventArgs e)
        {
            printersTabControl.SelectedIndex = 2;
        }
    }
}
