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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            command.ExecuteNonQuery();
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

            MessageBox.Show("Согласование установлено");
            command.ExecuteNonQuery();
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument flowDoc = messageRichtextbox.Document;

            // Создаём новый параграф с текстом
            Paragraph paragraph = new Paragraph(new Run("Yurchenkoiv: "+messageTextBlock.Text));
            paragraph.Margin = new Thickness(0); // Убираем отступы между строками

            // Добавляем параграф в документ
            flowDoc.Blocks.Add(paragraph);

            // Прокручиваем к концу
            messageRichtextbox.ScrollToEnd();
        }
    }
}
