using Npgsql;
using NpgsqlTypes;
using SupportCenter.Api;
using SupportCenter.Classes;
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
    /// Логика взаимодействия для CreateAppealFolderForm.xaml
    /// </summary>
    public partial class CreateAppealFolderForm : Window
    {
        public CreateAppealFolderForm()
        {
            InitializeComponent();
        }

       
        private async void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 1070;

            try
            {
                var api = new FolderApiGet();
                var folder = await api.GetFolderAsync();

                folderDataGrid.ItemsSource = folder;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных: " + ex.Message);
            }


            folderDataGrid.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void selectProgramDg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void selectаFolderDg_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 550;

            folderResponsible? path = folderDataGrid.SelectedItem as folderResponsible;
            
            if (path != null)
            {

                folderAppeal.Text = path.nameFolder;


            }
            else
            {

                MessageBox.Show("Программа не выбрана");

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Application.Current.MainWindow.Width = 550;
        }

        private void createAppealFolder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(folderAppeal.Text)) MessageBox.Show("Программа не выбрана.");
            else if (string.IsNullOrEmpty(commentAppeal.Text)) MessageBox.Show("Комментарий не указан.");

            else
            {








                folderResponsible? path = folderDataGrid.SelectedItem as folderResponsible;

                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("" +
                    "INSERT INTO main.appealfolder " +
                    "(applicant,folder,responsible,status_responsible,executor,status_executor,comment)" +
                    "VALUES ((select user_id from main.users where login_user=@applicantLogin),@idFolder,(select responsible_user from main.folders where id=@idFolder),0,null,0,@comment)", db_connect.GetConnection());
                command.Parameters.Add("@idFolder", NpgsqlDbType.Integer).Value = Convert.ToInt32(path.Id);
                command.Parameters.Add("@applicantLogin", NpgsqlDbType.Text).Value = Session.CurrentUserLogin;
                command.Parameters.Add("@comment", NpgsqlDbType.Text).Value = commentAppeal.Text;



                adapter.SelectCommand = command;
                command.ExecuteReader();
                db_connect.closeConnection();
                MessageBox.Show("Обращение создано.");
                this.Close();

            }
        }
    }
}
