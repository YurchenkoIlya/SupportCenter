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
            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand("Select *  from main.users", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<Users> result = new List<Users>();

            while (reader.Read())
            {
                string roleUser = null;
                string activity = null;


                if (Convert.ToString(reader[2]) == "0")
                    roleUser = "Пользователь";
                else roleUser = "Администратор";
                if (Convert.ToString(reader[4]) == "1")
                    activity = "Включена";
                else activity = "Выключена";


                result.Add(new Users(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), roleUser, Convert.ToString(reader[3]), Convert.ToString(activity)));

            }
            reader.Close();

            selectResponsibleDataGrid.ItemsSource = result;
            selectResponsibleDataGrid.Columns[0].Header = "ID";
            selectResponsibleDataGrid.Columns[1].Header = "ЛОГИН";
            selectResponsibleDataGrid.Columns[2].Visibility = Visibility.Collapsed;
            selectResponsibleDataGrid.Columns[3].Header = "ФИО";
            selectResponsibleDataGrid.Columns[4].Visibility = Visibility.Collapsed;
            selectResponsibleDataGrid.Columns[0].Width = 50;

            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 170;
        }

        private void createFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameFolder.Text)) MessageBox.Show("Наименование папки не заполнено.");
            else if(string.IsNullOrEmpty(wayFolder.Text)) MessageBox.Show("Путь к папке не заполнен.");
            else if (string.IsNullOrEmpty(responsibleTextBox.Text)) MessageBox.Show("Не выбран ответственный за папку.");
            else
            {







                RedactUserForm redactForm = new RedactUserForm();
                Users? path = selectResponsibleDataGrid.SelectedItem as Users;

                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.folders (name_folder,responsible_user,way_folder) VALUES (@nameFolder,@responsible_user,@way_folder)", db_connect.GetConnection());
                command.Parameters.Add("@nameFolder", NpgsqlDbType.Text).Value = nameFolder.Text;
                command.Parameters.Add("@responsible_user", NpgsqlDbType.Bigint).Value = path.Id;
                command.Parameters.Add("@way_folder", NpgsqlDbType.Text).Value = wayFolder.Text;
                adapter.SelectCommand = command;
                command.ExecuteReader();
                db_connect.closeConnection();
                MessageBox.Show("ПАПКА СОЗДАНА");

            }
        }

        private void selectResponsible_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 460;

        }

        private void selectResponsibleButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Height = 170;

            RedactUserForm redactForm = new RedactUserForm();
            Users? path = selectResponsibleDataGrid.SelectedItem as Users;
            if (path != null) {

                responsibleTextBox.Text = path.name;


            }
            else
            {



            }
        }
    }
}
