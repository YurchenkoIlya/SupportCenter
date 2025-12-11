using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для CreateProgramForm.xaml
    /// </summary>
    public partial class CreateProgramForm : Window
    {
        public CreateProgramForm()
        {
            InitializeComponent();
        }

      /*  private void Window_Loaded(object sender, RoutedEventArgs e)
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
        }*/

        private void createProgramButton_Click(object sender, RoutedEventArgs e)
        {
            RedactUserForm redactForm = new RedactUserForm();
            Users? path = selectResponsibleDataGrid.SelectedItem as Users;

            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.program (name_program,responsible_program,way_program) VALUES (@nameFolder,@responsible_user,@way_folder)", db_connect.GetConnection());
            command.Parameters.Add("@nameFolder", NpgsqlDbType.Text).Value = nameFolder.Text;
            command.Parameters.Add("@responsible_user", NpgsqlDbType.Bigint).Value = path.Id;
            command.Parameters.Add("@way_folder", NpgsqlDbType.Text).Value = wayFolder.Text;
            adapter.SelectCommand = command;
            command.ExecuteReader();
            db_connect.closeConnection();
            MessageBox.Show("Программа добавлена");
        }
    }
}
