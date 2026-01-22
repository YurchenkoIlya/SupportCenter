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
    /// Логика взаимодействия для RegulationsForm.xaml
    /// </summary>
    public partial class RegulationsForm : Window
    {
        public RegulationsForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            regulationsTabControl.SelectedIndex = 0;
        }

        private void programButton_Click(object sender, RoutedEventArgs e)
        {
            regulationsTabControl.SelectedIndex = 1;
        }

        private void folderButton_Click(object sender, RoutedEventArgs e)
        {
            regulationsTabControl.SelectedIndex = 2;
        }

        private void printerButton_Click(object sender, RoutedEventArgs e)
        {
            regulationsTabControl.SelectedIndex = 3;
        }

        private void othersButton_Click(object sender, RoutedEventArgs e)
        {
            regulationsTabControl.SelectedIndex = 4;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            infoTabItem.Visibility = Visibility.Collapsed;
            programTabItem.Visibility = Visibility.Collapsed;
            foldersTabItem.Visibility = Visibility.Collapsed;
            printerTabItem.Visibility = Visibility.Collapsed;
            othersTabItem.Visibility = Visibility.Collapsed;
        }
    }
}
