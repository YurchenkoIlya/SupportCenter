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
    /// Логика взаимодействия для ViewAppealFolderForm.xaml
    /// </summary>
    public partial class ViewAppealFolderForm : Window
    {
        public ViewAppealFolderForm()
        {
            InitializeComponent();
        }
        public void loadForm()
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
                        "LEFT JOIN main.folders p ON u.folder = p.id where u.id_appeal = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<appealFolder> result = new List<appealFolder>();

            if (reader.Read())
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

            var Appeal = result[0];

            nameFolder.Text = Appeal.idFolder;
            applicantFolder.Text = Appeal.idApplicant;
            responsibleFolder.Text = Appeal.idResponsible;
            executorFolder.Text = Appeal.executor;
            commentTextBlock.Text = Appeal.comment;





        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            loadForm();

            responsibleTabItem.Visibility = Visibility.Collapsed;
            executeTabItem.Visibility = Visibility.Collapsed;
            chatTabItem.Visibility = Visibility.Collapsed;
            historyTabItem.Visibility = Visibility.Collapsed;



        }

        private void selectResponsibleSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 0;
        }

        private void selectExecutorSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 1;
        }

        private void selectHistorySpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 3;


            dbConnect db_connect = new dbConnect();
            NpgsqlCommand command = new NpgsqlCommand(
                "SELECT u.id_log, u.date_log, u.id_appeal, u.action, u.appeal_type, o.user_name " +
                "FROM main.log_appeal u " +
                "LEFT JOIN main.users o " +
                "ON u.id_user = o.user_id " +
                "WHERE u.id_appeal = @idAppeal AND u.appeal_type = 2",
    db_connect.GetConnection());
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);
            db_connect.openConnection();
            command.ExecuteNonQuery();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            List<HistoryAppealDto> result = new List<HistoryAppealDto>();

            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    DateTime date = reader.GetDateTime(1);
                    string formattedDate = date.ToString("dd.MM.yyyy HH:mm");


                    result.Add(new HistoryAppealDto(
                        idLog: reader.GetInt32(0),
                        dateLog: formattedDate,
                        idAppeal: Convert.ToInt32(idAppeal.Text),
                        action: reader.GetString(3),
                        appealType: 2,
                        userName: reader.GetString(5)
                    ));
                }

                historyAppealDataGrid.ItemsSource = result;
            }
        }

        private void selectChatSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 2;
        }
        public void logRead(string action)
        {
            DateTime currentDateTime = DateTime.Now;

            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.log_appeal" +
                "(date_log,id_appeal,action,appeal_type,id_user) " +
                "values (date_trunc('second', @date),@idAppeal,@action,@appeal_type,(select user_id from main.users where login_user = @loginUser))", db_connect.GetConnection());
            command.Parameters.Add("@date", NpgsqlDbType.Timestamp).Value = currentDateTime;
            command.Parameters.Add("@loginUser", NpgsqlDbType.Text).Value = Session.CurrentUserLogin;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);
            command.Parameters.Add("@action", NpgsqlDbType.Text).Value = action;
            command.Parameters.Add("@appeal_type", NpgsqlDbType.Integer).Value = 2;


            command.ExecuteNonQuery();



        }

        private void acceptOibResponsible_Click(object sender, RoutedEventArgs e)
        {
            int oibStatus = Convert.ToInt32(oibStatusResponsble.SelectedIndex);




            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealfolder SET status_responsible = @oibStatus WHERE id_appeal = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@oibStatus", NpgsqlDbType.Integer).Value = oibStatus;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            MessageBox.Show("Согласование установлено");
            command.ExecuteNonQuery();
            loadForm();
            string action = "Изменено согласование ответственного: " + oibStatusResponsble.Text;
            logRead(action);
        }
    }
}
