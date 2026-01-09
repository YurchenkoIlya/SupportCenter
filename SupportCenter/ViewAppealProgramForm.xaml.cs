using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;

namespace SupportCenter
{
    /// <summary>
    /// Логика взаимодействия для ViewAppealProgramForm.xaml
    /// </summary>
    public partial class ViewAppealProgramForm : Window
    {
        public ViewAppealProgramForm()
        {
            InitializeComponent();
        }

        private void selectResponsibleSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealProgram.SelectedIndex = 0;
        }

        private void selectExecutorSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealProgram.SelectedIndex = 1;
        }

        private void selectHistorySpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealProgram.SelectedIndex = 3;
        }

        private void selectChatSpace_Click(object sender, RoutedEventArgs e)
        {
            menuAppealProgram.SelectedIndex = 2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadForm();

            responsibleTabItem.Visibility = Visibility.Collapsed;
            executorTabItem.Visibility = Visibility.Collapsed;
            chatTabItem.Visibility = Visibility.Collapsed;
            historyTabItem.Visibility = Visibility.Collapsed;

           
            logRead("Открыл обращение");
        }
        public void loadForm()
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
                        "LEFT JOIN main.program p ON u.id_program = p.id_program where id_appeal_program =@idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);
            db_connect.openConnection();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            NpgsqlDataReader reader = command.ExecuteReader();
            List<appealProgram> result = new List<appealProgram>();

            if (reader.Read())
            {
                string? oibStatus = null;
                string? oitStatus = null;
                string? otpStatus = null;
                string? otpExecutor = null;


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

                if (Convert.ToString(reader[9]) == "")
                {
                    otpExecutor = "Нет исполнителя";
                }
                else otpExecutor = Convert.ToString(reader[9]);



                result.Add(new appealProgram(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]), Convert.ToString(reader[2]), Convert.ToString(reader[3]), Convert.ToString(reader[4]),
                    Convert.ToString(reader[5]), oibStatus, Convert.ToString(reader[7]), oitStatus, otpExecutor, otpStatus));

                var Appeal = result[0];

                nameProgram.Text = Appeal.idProgram;
                namePc.Text = Appeal.pcName;
                ipPc.Text = Appeal.ipPc;
                applicantProgram.Text = Appeal.applicant;
                executorProgram.Text = Appeal.otpExecutor;


                switch (oibStatus)
                {


                    case "Не согласовано": oibStatusResponsble.SelectedIndex = 0; break;
                    case "Согласовано": oibStatusResponsble.SelectedIndex = 1; break;
                    case "Отказано": oibStatusResponsble.SelectedIndex = 2; break;

                }
                switch (oitStatus)
                {


                    case "Не согласовано": oitStatusResponsble.SelectedIndex = 0; break;
                    case "Согласовано": oitStatusResponsble.SelectedIndex = 1; break;
                    case "Отказано": oitStatusResponsble.SelectedIndex = 2; break;

                }
                if (otpExecutor != "Нет исполнителя") executorProgram.Text = otpExecutor;

                switch (otpStatus)
                {


                    case "Не в работе": otpStatusResponsble.SelectedIndex = 0; break;
                    case "В работе": otpStatusResponsble.SelectedIndex = 1; break;
                    case "Выполнено": otpStatusResponsble.SelectedIndex = 2; break;
                    case "Отменено": otpStatusResponsble.SelectedIndex = 3; break;

                }



                if (Session.AuthorizationStatus != 1)
                {
                    if (otpStatusResponsble.SelectedIndex == 0)
                    {




                        if (Session.CurrentUserName != oitResponsible.Text)
                        {
                            oitStatusResponsble.IsEnabled = false;
                            acceptOitResponsible.Visibility = Visibility.Hidden;


                        }
                        if (Session.CurrentUserName != oibResponsible.Text)
                        {
                            oitStatusResponsble.IsEnabled = false;
                            acceptOitResponsible.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        oitStatusResponsble.IsEnabled = false;
                        acceptOitResponsible.Visibility = Visibility.Hidden;
                        oitStatusResponsble.IsEnabled = false;
                        acceptOitResponsible.Visibility = Visibility.Hidden;
                    }



                    if (Session.CurrentUserName != executorProgram.Text || oibStatusResponsble.SelectedIndex != 1 || oitStatusResponsble.SelectedIndex != 1)
                    {
                        otpStatusResponsble.IsEnabled = false;
                        acceptOtpResponsible.Visibility = Visibility.Hidden;



                    }
                    else
                    {
                        otpStatusResponsble.IsEnabled = true;
                        acceptOtpResponsible.Visibility = Visibility.Visible;

                    }

                }

            }
            reader.Close();














        }

        private void acceptOibResponsible_Click(object sender, RoutedEventArgs e)
        {
            int oibStatus = Convert.ToInt32(oibStatusResponsble.SelectedIndex);




            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealprogram SET oib_status_responsible = @oibStatus WHERE id_appeal_program = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@oibStatus", NpgsqlDbType.Integer).Value = oibStatus;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            MessageBox.Show("Согласование установлено");
            command.ExecuteNonQuery();
            loadForm();
            string action = "Сменил этап согласования ОИБ на : " + oibStatusResponsble.Text;
            logRead(action);
        }

        private void acceptOitResponsible_Click(object sender, RoutedEventArgs e)
        {
            int oitStatus = Convert.ToInt32(oitStatusResponsble.SelectedIndex);




            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealprogram SET oit_status_responsible = @oitStatus WHERE id_appeal_program = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@oitStatus", NpgsqlDbType.Integer).Value = oitStatus;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            MessageBox.Show("Согласование установлено");
            string action = "Сменил этап согласования ОИТ на : " + oitStatusResponsble.Text;
            logRead(action);
            command.ExecuteNonQuery();
            loadForm();
        }

        private void acceptOtpResponsible_Click(object sender, RoutedEventArgs e)
        {
            int otpStatus = Convert.ToInt32(otpStatusResponsble.SelectedIndex);



            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealprogram SET otp_status_executor = @otpStatus WHERE id_appeal_program = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@otpStatus", NpgsqlDbType.Integer).Value = otpStatus;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            
            
            
            
            
            MessageBox.Show("Этап выполнения изменен.");
            string action = "Сменил этап выполнения обращения на: "+otpStatusResponsble.Text;
            logRead(action);
            command.ExecuteNonQuery();
            loadForm();
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument flowDoc = messageRichtextbox.Document;

            // Создаём новый параграф с текстом
            Paragraph paragraph = new Paragraph(new Run("Yurchenkoiv: " + messageTextBlock.Text));
            paragraph.Margin = new Thickness(0); // Убираем отступы между строками

            // Добавляем параграф в документ
            flowDoc.Blocks.Add(paragraph);

            // Прокручиваем к концу
            messageRichtextbox.ScrollToEnd();
        }

        private void giveAppeal_Click(object sender, RoutedEventArgs e)
        {
            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.appealprogram " +
                "SET otp_executor = (select user_id from main.users where login_user = @otpExecutor ) " +
                "WHERE id_appeal_program = @idAppeal", db_connect.GetConnection());
            command.Parameters.Add("@otpExecutor", NpgsqlDbType.Text).Value = Session.CurrentUserLogin;
            command.Parameters.Add("@idAppeal", NpgsqlDbType.Integer).Value = Convert.ToInt32(idAppeal.Text);

            MessageBox.Show("Взято в работу.");
            command.ExecuteNonQuery();



            string action = "Взял обращение в исполнение";
            logRead(action);
            loadForm();

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
            command.Parameters.Add("@appeal_type", NpgsqlDbType.Integer).Value = 1;


            command.ExecuteNonQuery();

            

        }
    }
}
