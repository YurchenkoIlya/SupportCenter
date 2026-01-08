using Npgsql;
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


        private void buttonFolder_Click(object sender, RoutedEventArgs e)
        {
            workAreaControl.SelectedIndex = 1;
            loadFolder();

        }
        public void loadFolder()
        {
            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT u.id_appeal, " +            
                        "applicant.user_name AS applicant_username, " +
                        "p.name_folder, " +
                        "responsible.user_name AS responsible_username, " +
                        "u.status_responsible, " +
                        "otp_user.user_name AS otp_username, " +
                        "u.status_executor, " +
                        "u.comment " +
                        "FROM main.appealfolder u " +
                        "LEFT JOIN main.users applicant ON u.applicant = applicant.user_id " +
                        "LEFT JOIN main.users responsible ON u.responsible = responsible.user_id " +
                        "LEFT JOIN main.users otp_user ON u.executor = otp_user.user_id " +                     
                        "LEFT JOIN main.folders p ON u.folder = p.id", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<appealFolder> result = new List<appealFolder>();

            while (reader.Read())
            {
                string? responsibleStatus = null;
                string? executeStatus = null;
                string? otpExecutor = null;




                  switch (Convert.ToString(reader[4]))
                  {
                    case "0": responsibleStatus = "Не согласовано"; break;
                    case "1": responsibleStatus = "Согласовано"; break;
                    case "2": responsibleStatus = "Отказано"; break;

                  }
                  switch (Convert.ToString(reader[6]))
                  {
                      case "0": executeStatus = "Не в работе"; break;
                      case "1": executeStatus = "В работе"; break;
                      case "2": executeStatus = "Выполнено"; break;
                      case "3": executeStatus = "Отменено"; break;

                  }
                  if (Convert.ToString(reader[5]) == "")
                  {
                      otpExecutor = "Нет исполнителя";
                  }
                  else otpExecutor = Convert.ToString(reader[9]);



                result.Add(new appealFolder(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToString(reader[2]), Convert.ToString(reader[3]),
                    responsibleStatus, otpExecutor, executeStatus, Convert.ToString(reader[7])));

            }
            reader.Close();
            appealFolderDataGrid.ItemsSource = result;





        }

        public void loadAppealProgram()
        {

            dbConnect db_connect = new dbConnect();
            DataTable table = new DataTable();
            NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT u.id_appeal_program, " +
                        "p.name_program, " +
                        "u.pc_name, " +
                        "u.ip_pc, " +
                        "user_applicant.user_name AS applicant_user," +
                        "oib_user.user_name AS oib_user_name, " +
                        "u.oib_status_responsible, " +
                        "oit_user.user_name AS oit_user_name, " +
                        "u.oit_status_responsible, " +
                        "otp_user.user_name AS otp_user_name, " +
                        "u.otp_status_executor " +
                        "FROM main.appealprogram u " +
                        "LEFT JOIN main.users oib_user ON u.oib_responsible = oib_user.user_id " +
                        "LEFT JOIN main.users oit_user ON u.oit_responsible = oit_user.user_id " +
                        "LEFT JOIN main.users otp_user ON u.otp_executor = otp_user.user_id " +
                        "LEFT JOIN main.users user_applicant ON u.applicant = user_applicant.user_id " +
                        "LEFT JOIN main.program p ON u.id_program = p.id_program", db_connect.GetConnection());
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<appealProgram> result = new List<appealProgram>();

            while (reader.Read())
            {
                string ?oibStatus = null;
                string ?oitStatus = null;
                string ?otpStatus = null;
                string ?otpExecutor = null;

               
                switch (Convert.ToString(reader[6]))
                {
                    case "0": oibStatus = "Не согласовано"; break;
                    case "1": oibStatus = "Согласовано"; break;
                    case "2": oibStatus = "Отказано"; break;

                }
                switch (Convert.ToString(reader[8]))
                {
                    case "0": oitStatus = "Не согласовано"; break;
                    case "1": oitStatus = "Согласовано"; break;
                    case "2": oitStatus = "Отказано"; break;

                }
                switch (Convert.ToString(reader[10]))
                {
                    case "0": otpStatus = "Не в работе"; break;
                    case "1": otpStatus = "В работе"; break;
                    case "2": otpStatus = "Выполнено"; break;
                    case "3": otpStatus = "Отменено"; break;

                }


           /*     if (Convert.ToString(reader[10]) == "0")
                {
                    otpStatus = "Не в работе";
                }*/
                
                if (Convert.ToString(reader[9]) == "")
                {
                    otpExecutor = "Нет исполнителя";
                }
                else otpExecutor = Convert.ToString(reader[9]);



                    result.Add(new appealProgram(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToString(reader[2]), Convert.ToString(reader[3]), Convert.ToString(reader[4]),
                        Convert.ToString(reader[5]), oibStatus, Convert.ToString(reader[7]), oitStatus, otpExecutor, otpStatus));

            }
            reader.Close();
            appealProgramDataGrid.ItemsSource = result;

           

        }

        private void appealProgramCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateAppealProgramForm createAppealProgramForm = new CreateAppealProgramForm();
            createAppealProgramForm.ShowDialog();
        }

        private void appealProgramShow_Click(object sender, RoutedEventArgs e)
        {
            workAreaControl.SelectedIndex = 0;
            loadAppealProgram();
        }

        private void appealProgramDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            appealProgramDataGrid.Columns[0].Header = "ID";
            appealProgramDataGrid.Columns[1].Header = "ПРОГРАММА";
            appealProgramDataGrid.Columns[2].Header = "КОМПЬЮТЕР";
            appealProgramDataGrid.Columns[3].Header = "IP АДРЕС";
            appealProgramDataGrid.Columns[4].Header = "ИЦИЦИАТОР";
            appealProgramDataGrid.Columns[5].Header = "СОГЛАСУБЮЩИЙ ОИБ";
            appealProgramDataGrid.Columns[6].Header = "СТАТУС";
            appealProgramDataGrid.Columns[7].Header = "СОГЛАСУЮЩИЙ ОИТ";
            appealProgramDataGrid.Columns[8].Header = "СТАТУС";
            appealProgramDataGrid.Columns[9].Header = "ИСПОЛНИТЕЛЬ ОТП";
            appealProgramDataGrid.Columns[10].Header = "ЭТАП ВЫПОЛНЕНИЯ";
            appealProgramDataGrid.Columns[0].Width = 35;
        }

        private void appealProgramView_Click(object sender, RoutedEventArgs e)
        {
            
            


            appealProgram? path = appealProgramDataGrid.SelectedItem as appealProgram;
            if (path != null)
            {
                ViewAppealProgramForm viewAppealProgramForm = new ViewAppealProgramForm();

                viewAppealProgramForm.idAppeal.Text = Convert.ToString(path.idAppeal);

                viewAppealProgramForm.ShowDialog();
                loadAppealProgram();
            }
            else
            {
                MessageBox.Show("Выберите обращение из списка");
            }


                


        }

        private void buttonPrinter_Click(object sender, RoutedEventArgs e)
        {
            workAreaControl.SelectedIndex = 2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadAppealProgram();

            programTabItem.Visibility = Visibility.Collapsed;
            foldersTabItem.Visibility = Visibility.Collapsed;
            PrintersTabItem.Visibility = Visibility.Collapsed;
            
        }

        private void appealFolderView_Click(object sender, RoutedEventArgs e)
        {

            appealFolder? path = appealFolderDataGrid.SelectedItem as appealFolder;
            if (path != null)
            {
                ViewAppealFolderForm viewAppealFolder = new ViewAppealFolderForm();
               

                viewAppealFolder.idAppeal.Text = Convert.ToString(path.idAppeal);

                viewAppealFolder.ShowDialog();
                loadAppealProgram();
            }
            else
            {
                MessageBox.Show("Выберите обращение из списка");
            }

            

        }

        private void appealFolderDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            appealFolderDataGrid.Columns[0].Header = "ID";
            appealFolderDataGrid.Columns[1].Header = "ИНИЦИАТОР";
            appealFolderDataGrid.Columns[2].Header = "ПАПКА";
            appealFolderDataGrid.Columns[3].Header = "ОТВЕТСТВЕННЫЙ";
            appealFolderDataGrid.Columns[4].Header = "СТАТУС СОГЛАСОВАНИЯ";
            appealFolderDataGrid.Columns[5].Header = "ИСПОЛНИТЕЛЬ";
            appealFolderDataGrid.Columns[6].Header = "СТАТУС ВЫПОЛНЕНИЯ";
            appealFolderDataGrid.Columns[7].Header = "КОММЕНТАРИЙ";
            appealFolderDataGrid.Columns[0].Width = 35;
        }
    }
}
