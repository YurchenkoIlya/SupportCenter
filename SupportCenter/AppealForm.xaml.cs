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
    /// Логика взаимодействия для AppealForm.xaml
    /// </summary>
    public partial class AppealForm : Window
    {
        public AppealForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();  
            this.Close();
        }

        private void buttonProgram_Click(object sender, RoutedEventArgs e)
        {
            workAreaControl.SelectedIndex = 0;
        }

        private void buttonFolder_Click(object sender, RoutedEventArgs e)
        {
            workAreaControl.SelectedIndex = 1;
        }
    }
}
