using Npgsql;
using SupportCenter.Api;
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
            loadUserDataGrid();
            

            
            

        }
        private async void loadUserDataGrid()
        {
            workTabControl.SelectedIndex = 0;

            try
            {
                var api = new UsersApiClient();
                var users = await api.GetUsersAsync();


                var queryResult = users.AsQueryable();

               
                if (activityCheckBox.IsChecked == true)
                    queryResult = queryResult.Where(u => u.activity == "Включена");
                if (fioFilterTextbox.Text != "")
                    queryResult = queryResult.Where(u =>
                 (u.name != null && u.name.IndexOf(fioFilterTextbox.Text, StringComparison.OrdinalIgnoreCase) >= 0));
                if (roleFilterComboBox.Text != "Все роли")
                    queryResult = queryResult.Where(u => u.role == roleFilterComboBox.Text);
             


                usersDataGrid.ItemsSource = queryResult;

                _ = Dispatcher.BeginInvoke(new Action(() =>
                {
                    
                    if (usersDataGrid.Columns.Count >= 5)
                    {

                        usersDataGrid.Columns[0].Header = "ID";
                        usersDataGrid.Columns[1].Header = "ЛОГИН";
                        usersDataGrid.Columns[2].Header = "РОЛЬ";
                        usersDataGrid.Columns[3].Header = "ФИО";
                        usersDataGrid.Columns[4].Header = "СТАТУС УЧЕТНОЙ ЗАПИСИ";
                        usersDataGrid.Columns[0].Width = 50;

                    }
                }), System.Windows.Threading.DispatcherPriority.Background);





            }


            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }

           



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
                redactForm.loginUserTextblock.Text = Convert.ToString(path.Login);

                redactForm.roleUserComboBox.Items.Add("Пользователь");
                redactForm.roleUserComboBox.Items.Add("Администратор");
                // redactForm.roleUserComboBox.SelectedIndex = 1;
                if (path.role == "Пользователь") redactForm.roleUserComboBox.SelectedValue = "Пользователь";
                else redactForm.roleUserComboBox.SelectedValue = "Администратор";






                if (path.activity == "Включена") redactForm.activityCheckBox.IsChecked = true;
                else redactForm.activityCheckBox.IsChecked = false;



            }
            



            redactForm.ShowDialog();
            loadUserDataGrid();

        }

        private void foldersButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 1;
            loadFolder();
           
          

        }
        public async void loadFolder()
        {

            workTabControl.SelectedIndex = 1;
            

            try
            {
                var api = new FolderApiGet();
                var folder = await api.GetFolderAsync();

                var queryResult = folder.AsQueryable();

                if (nameFilterTextBox.Text != "")
                    queryResult = queryResult.Where(u =>
                 (u.nameFolder != null && u.nameFolder.IndexOf(nameFilterTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0));
                if (responsibleFilterTextBox.Text != "")
                    queryResult = queryResult.Where(u =>
                 (u.responsibleUser != null && u.responsibleUser.IndexOf(responsibleFilterTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0));

                folderDataGrid.ItemsSource = queryResult;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }

        }

        private void folderDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            folderDataGrid.Columns[0].Header = "ID";
            folderDataGrid.Columns[1].Header = "НАИМЕНОВАНИЕ ПАПКИ";
            folderDataGrid.Columns[2].Header = "ОТВЕСТВЕННЫЙ ЗА ПАПКУ";
            folderDataGrid.Columns[3].Header = "ПУТЬ К ПАПКЕ";
            folderDataGrid.Columns[4].Header = "ГРУППА ДОСТУПА AD";
            folderDataGrid.Columns[0].Width = 50;
        }

        private void createFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateFolderForm createFolderForm = new CreateFolderForm();
            createFolderForm.ShowDialog();
            loadFolder();
        }

        private void programButton_Click(object sender, RoutedEventArgs e)
        {
            workTabControl.SelectedIndex = 2;

           loadProgram();

        }
        public async void loadProgram()
        {
            workTabControl.SelectedIndex = 2;

            try
            {
                var api = new ProgramApiGet();
                var program = await api.GetProgramAsync();
                programDataGrid.ItemsSource = program;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }


        }

        private void programDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            programDataGrid.Columns[0].Header = "ID";
            programDataGrid.Columns[1].Header = "НАИМЕНОВАНИЕ ПРОГРАММЫ";
            programDataGrid.Columns[2].Header = "ПУТЬ К ПРОГРАММЕ";
            programDataGrid.Columns[0].Width = 50;
            programDataGrid.Columns[1].Width = 200;
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
            loadProgram();
           
           
            
        }

        private void activityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            loadUserDataGrid();
        }

        private void activityCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            loadUserDataGrid();
        }

        private void fioFilterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadUserDataGrid();
        }

        private void roleFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadUserDataGrid();
        }

        private void nameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadFolder();
        }

        private void responsibleFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadFolder();
        }
    }
}
