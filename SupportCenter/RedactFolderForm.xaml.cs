using Npgsql;
using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// Логика взаимодействия для RedactFolderForm.xaml
    /// </summary>
    public partial class RedactFolderForm : Window
    {
        public RedactFolderForm()
        {
            InitializeComponent();
        }

        private void createFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void selectResponsible_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var api = new UsersApiClient();
                var users = await api.GetUsersAsync();
                selectResponsibleDataGrid.ItemsSource = users;

                selectResponsibleDataGrid.Columns[0].Header = "ID";
                selectResponsibleDataGrid.Columns[1].Header = "ЛОГИН";
                selectResponsibleDataGrid.Columns[2].Visibility = Visibility.Collapsed;
                selectResponsibleDataGrid.Columns[3].Header = "ФИО";
                selectResponsibleDataGrid.Columns[4].Visibility = Visibility.Collapsed;
                selectResponsibleDataGrid.Columns[0].Width = 50;

                Application.Current.MainWindow = this;
                Application.Current.MainWindow.Height = 530;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 250;
        }

        private void selectResponsibleButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 250;

            RedactUserForm redactForm = new RedactUserForm();
            Users? path = selectResponsibleDataGrid.SelectedItem as Users;
            if (path != null)
            {

                responsibleTextBox.Text = path.name;


            }
            else
            {

                MessageBox.Show("Пользователь не выбран.");

            }
        }

        private async void RedactFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new folderResponsible
            {
                Id = Convert.ToInt32(idFolder.Text),
                nameFolder = nameFolder.Text,
                responsibleUser = responsibleTextBox.Text,
                wayFolder = wayFolder.Text,
                accessGroup = accessGroup.Text
            };

            var api = new FolderApiRedact();
            string result = await api.UpdateFolderAsync(dto);

            MessageBox.Show(result, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
    }
}
