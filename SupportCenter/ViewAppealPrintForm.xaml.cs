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
using System.Xml.Linq;

namespace SupportCenter
{
    /// <summary>
    /// Логика взаимодействия для ViewAppealPrintForm.xaml
    /// </summary>
    public partial class ViewAppealPrintForm : Window
    {
        private int _idAppeal;
        public ViewAppealPrintForm(int idAppeal)
        {
            InitializeComponent();
            _idAppeal = idAppeal;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            loadForm();
         
            executeTabItem.Visibility = Visibility.Collapsed;
            chatTabItem.Visibility = Visibility.Collapsed;
            historyTabItem.Visibility = Visibility.Collapsed;


        }

        private void loadForm()
        {
            dbConnect db_connect = new dbConnect();

            using var command = new NpgsqlCommand(
                @"SELECT u.id_appeal, u.ip_pc, u.name_pc, u.model_printer, u.ip_printer,
                 executor.user_name AS executor_username,
                 u.status_execute,
                 u.comment,
                 applicant.user_name AS applicant_username
          FROM main.appealprinter u
          LEFT JOIN main.users executor ON u.executor = executor.user_id
          LEFT JOIN main.users applicant ON u.applicant = applicant.user_id
          WHERE u.id_appeal = @idAppeal",
                db_connect.GetConnection()
            );

            command.Parameters.AddWithValue("@idAppeal", NpgsqlTypes.NpgsqlDbType.Integer, _idAppeal);

            db_connect.openConnection();

            using var reader = command.ExecuteReader();

            if (reader.Read()) // одно обращение
            {
                idAppeal.Text = reader.GetInt32(0).ToString();
                ipPc.Text = reader.GetString(1);
                namePC.Text = reader.GetString(2);
                modelPrinter.Text = reader.GetString(3);
                ipPrinter.Text = reader.GetString(4);
                applicantAppeal.Text = reader.GetString(8);
                commentTextBlock.Text = reader.IsDBNull(7) ? "" : reader.GetString(7);
                executor.Text = reader.IsDBNull(5) ? "Не выбран" : reader.GetString(5);

                switch (reader.GetInt32(6))
                {
                    case 0: otpStatusResponsble.SelectedIndex = 0; break;
                    case 1: otpStatusResponsble.SelectedIndex = 1; break;
                    case 2: otpStatusResponsble.SelectedIndex = 2; break;
                    case 3: otpStatusResponsble.SelectedIndex = 3; break;


                }

                

            }



        }

        private void selectExecutorSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 0;

        }

        private void selectHistorySpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 2;

            dbConnect db_connect = new dbConnect();
            NpgsqlCommand command = new NpgsqlCommand(
                "SELECT u.id_log, u.date_log, u.id_appeal, u.action, u.appeal_type, o.user_name " +
                "FROM main.log_appeal u " +
                "LEFT JOIN main.users o " +
                "ON u.id_user = o.user_id " +
                "WHERE u.id_appeal = @idAppeal AND u.appeal_type = 3",
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
                        appealType: 1,
                        userName: reader.GetString(5)
                    ));
                }
            }


            historyAppealDataGrid.ItemsSource = result;
        }

        private void selectChatSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealFolder.SelectedIndex = 1;
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
            command.Parameters.Add("@appeal_type", NpgsqlDbType.Integer).Value = 3;


            command.ExecuteNonQuery();



        }

        private void acceptOtpResponsible_Click(object sender, RoutedEventArgs e)
        {
            int otpStatus = Convert.ToInt32(otpStatusResponsble.SelectedIndex);



            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealprinter SET status_execute = @otpStatus WHERE id_appeal = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@otpStatus", NpgsqlDbType.Integer).Value = otpStatus;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            MessageBox.Show("Этап выполнения изменен.");
            string action = "Изменен этап выолнения на: " + otpStatusResponsble.Text;
            logRead(action);

            command.ExecuteNonQuery();

            loadForm();

        }
    }
}
