using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SupportCenter
{
    /// <summary>
    /// Логика взаимодействия для CreateFolderForm.xaml
    /// </summary>
    public partial class CreateFolderForm : Window
    {
        public CreateFolderForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 200;

        }

        private void createFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameFolder.Text)) MessageBox.Show("Наименование папки не заполнено.");
            else if(string.IsNullOrEmpty(wayFolder.Text)) MessageBox.Show("Путь к папке не заполнен.");
            else if (string.IsNullOrEmpty(accessGroup.Text)) MessageBox.Show("Не указана группа доступа к папке.");
            else if (string.IsNullOrEmpty(responsibleTextBox.Text)) MessageBox.Show("Не выбран ответственный за папку.");
            else
            {







                RedactUserForm redactForm = new RedactUserForm();
                Users? path = selectResponsibleDataGrid.SelectedItem as Users;

                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.folders (name_folder,responsible_user,way_folder,access_group) VALUES (@nameFolder,@responsible_user,@way_folder,@access_group)", db_connect.GetConnection());
                command.Parameters.Add("@nameFolder", NpgsqlDbType.Text).Value = nameFolder.Text;
                command.Parameters.Add("@responsible_user", NpgsqlDbType.Bigint).Value = path.Id;
                command.Parameters.Add("@way_folder", NpgsqlDbType.Text).Value = wayFolder.Text;
                command.Parameters.Add("@access_group", NpgsqlDbType.Text).Value = accessGroup.Text;
                adapter.SelectCommand = command;
                command.ExecuteReader();
                db_connect.closeConnection();
                MessageBox.Show("ПАПКА СОЗДАНА");
                this.Close();
          
            }
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
                Application.Current.MainWindow.Height = 490;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }
        }

        private async void selectResponsibleButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 200;

            RedactUserForm redactForm = new RedactUserForm();
            Users? path = selectResponsibleDataGrid.SelectedItem as Users;
            if (path != null) {

                responsibleTextBox.Text = path.name;


            }
            else
            {

                MessageBox.Show("Пользователь не выбран00");

            }
        }
    }
}
