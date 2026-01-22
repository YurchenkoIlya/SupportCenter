using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System;
using System.CodeDom.Compiler;
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
    /// Логика взаимодействия для CreateAppealPrintForm.xaml
    /// </summary>
    public partial class CreateAppealPrintForm : Window
    {
        public CreateAppealPrintForm()
        {
            InitializeComponent();
        }

        private void selectProgram_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(modelPrinter.Text)) MessageBox.Show("Не заполнена модель принтера.");
            else if (string.IsNullOrWhiteSpace(ipPrinter.Text)) MessageBox.Show("Не заполнен ип адрес принтера.");
            else if (string.IsNullOrWhiteSpace(comment.Text)) MessageBox.Show("Не заполнен комментарий к обращению.");
            else
            {



               

                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("" +
                    "INSERT INTO main.appealprinter " +
                    "(ip_pc,name_pc,model_printer,ip_printer,executor,status_execute,comment,applicant)" +
                    "VALUES (@ipPC,@namePC,@modelPrinter,@ipPrinter,null,@statusExecute,@comment,(select user_id from main.users where login_user = @applicantLogin))", db_connect.GetConnection());
                command.Parameters.Add("@ipPC", NpgsqlDbType.Text).Value = Session.CurrentIp;
                command.Parameters.Add("@namePC", NpgsqlDbType.Text).Value = Session.CurrentPcName;
                command.Parameters.Add("@modelPrinter", NpgsqlDbType.Text).Value = modelPrinter.Text;
                command.Parameters.Add("@ipPrinter", NpgsqlDbType.Text).Value = ipPrinter.Text;
                command.Parameters.Add("@statusExecute", NpgsqlDbType.Integer).Value = 0;
                command.Parameters.Add("@comment", NpgsqlDbType.Text).Value = comment.Text;
                command.Parameters.Add("@applicantLogin", NpgsqlDbType.Text).Value = Session.CurrentUserLogin;


                adapter.SelectCommand = command;
                command.ExecuteNonQuery();
                db_connect.closeConnection();
                MessageBox.Show("Обращение создано.");
                this.Close();

            }
        }

        private void modelInfoPrinter_Click(object sender, RoutedEventArgs e)
        {
            InfoPrinterForm infoPrinterForm = new InfoPrinterForm();
            infoPrinterForm.ShowDialog();
        }
    }
    
}
