using Npgsql;
using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace SupportCenter
{
    /// <summary>
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {      
        public AdminForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void usersButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 0;

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

            usersDataGrid.ItemsSource = result;
            usersDataGrid.Columns[0].Header = "ID";
            usersDataGrid.Columns[1].Header = "ЛОГИН";
            usersDataGrid.Columns[2].Header = "РОЛЬ";
            usersDataGrid.Columns[3].Header = "ФИО";
            usersDataGrid.Columns[4].Header = "СТАТУС УЧЕТНОЙ ЗАПИСИ";
            usersDataGrid.Columns[0].Width = 50;
            

            
            

        }

       

        private void cancelButton_Копировать5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void redactUser_Click(object sender, RoutedEventArgs e)
        {
            RedactUserForm redactForm = new RedactUserForm();
            Users? path = usersDataGrid.SelectedItem as Users;

            if (path != null)
            {

                redactForm.idUserTextBlock.Text = Convert.ToString(path.Id);
                redactForm.loginUserTextblock.Text = Convert.ToString(path.login);

                redactForm.roleUserComboBox.Items.Add("Пользователь");
                redactForm.roleUserComboBox.Items.Add("Администратор");
                // redactForm.roleUserComboBox.SelectedIndex = 1;
                if (path.role == "Пользователь") redactForm.roleUserComboBox.SelectedValue = "Пользователь";
                else redactForm.roleUserComboBox.SelectedValue = "Администратор";






                if (path.activity == "Включена") redactForm.activityCheckBox.IsChecked = true;
                else redactForm.activityCheckBox.IsChecked = false;



            }



            
            redactForm.ShowDialog();


        }

        private void folderButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 1;
        }

        private void foldersButton_Click(object sender, RoutedEventArgs e)
        {
            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand
                ("Select u.id,u.name_folder,o.user_name,u.way_folder " +
                "from main.folders u " +
                "LEFT JOIN main.users o " +
                "ON u.responsible_user = o.user_id", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<folderResponsible> result = new List<folderResponsible>();

            while (reader.Read())
            {

                result.Add(new folderResponsible(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToString(reader[2]), Convert.ToString(reader[3])));

            }
            reader.Close();
            folderDataGrid.ItemsSource = result;
          

        }

        private void folderDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            folderDataGrid.Columns[0].Header = "ID";
            folderDataGrid.Columns[1].Header = "НАИМЕНОВАНИЕ ПАПКИ";
            folderDataGrid.Columns[2].Header = "ОТВЕСТВЕННЫЙ ЗА ПАПКУ";
            folderDataGrid.Columns[3].Header = "ПУТЬ К ПАПКЕ";
            folderDataGrid.Columns[0].Width = 50;
        }

        private void createFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateFolderForm createFolderForm = new CreateFolderForm();
            createFolderForm.ShowDialog();
        }

        private void programButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 2;

            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand
                ("Select u.id_program,u.name_program,o.user_name,u.way_program " +
                "from main.program u " +
                "LEFT JOIN main.users o " +
                "ON u.responsible_program = o.user_id", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<programResponsible> result = new List<programResponsible>();

            while (reader.Read())
            {

                result.Add(new programResponsible(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToString(reader[2]), Convert.ToString(reader[3])));

            }
            reader.Close();
            programDataGrid.ItemsSource = result;

        }

        private void programDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            programDataGrid.Columns[0].Header = "ID";
            programDataGrid.Columns[1].Header = "НАИМЕНОВАНИЕ ПРОГРАММЫ";
            programDataGrid.Columns[2].Header = "ОТВЕСТВЕННЫЙ ЗА ПРОГРАММУ";
            programDataGrid.Columns[3].Header = "ПУТЬ К ПРОГРАММЕ";
            programDataGrid.Columns[0].Width = 50;
            programDataGrid.Columns[1].Width = 100;
            programDataGrid.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 3;

            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand
                ("Select u.id_log,u.date,u.object_action,u.action,o.user_name,u.name_pc,u.ip_pc " +
                "from main.logs u " +
                "LEFT JOIN main.users o " +
                "ON u.id_user = o.user_id", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<historyProgram> result = new List<historyProgram>();

            while (reader.Read())
            {

                DateTime date = Convert.ToDateTime(reader[1]);
                string formattedDate = date.ToString("dd.MM.yyyy HH:mm:ss");

                result.Add(new historyProgram(Convert.ToInt32(reader[0]), formattedDate, Convert.ToString(reader[2]), Convert.ToString(reader[3]), Convert.ToString(reader[4]), Convert.ToString(reader[5]), Convert.ToString(reader[6])));

            }
            reader.Close();
           historyDataGrid.ItemsSource = result;

        }

        private void historyDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            historyDataGrid.Columns[0].Header = "ID";
            historyDataGrid.Columns[1].Header = "ДАТА";
            historyDataGrid.Columns[2].Header = "ОБЪЕКТ";
            historyDataGrid.Columns[3].Header = "ДЕЙСТВИЕ";
            historyDataGrid.Columns[4].Header = "ФИО";
            historyDataGrid.Columns[5].Header = "ИМЯ КОМПЬЮТЕРА";
            historyDataGrid.Columns[6].Header = "ИП КОМПЬЮТЕРА";
            historyDataGrid.Columns[0].Width = 50;
            historyDataGrid.Columns[1].Width = 120;
            historyDataGrid.Columns[2].Width = 80;
        }

        private void createProgram_Click(object sender, RoutedEventArgs e)
        {
            CreateProgramForm createProgram = new CreateProgramForm();
            createProgram.ShowDialog();
            
        }
    }
}
