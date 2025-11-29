using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SupportCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void App_Exit(object sender, ExitEventArgs e)
        {
            /*    DateTime currentDateTime = DateTime.Now;



                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.logs " +
                    "(date,action,object_action,name_pc,ip_pc,id_user) " +
                    "VALUES (date_trunc('second', @date),@action,@object_action,@name_pc,@ip_pc," +
                    "(Select user_id from main.users where login_user =@login_user))", db_connect.GetConnection());


                command.Parameters.Add("@date", NpgsqlDbType.Timestamp).Value = currentDateTime;
                command.Parameters.Add("@action", NpgsqlDbType.Text).Value = "Выход из программы";
                command.Parameters.Add("@object_action", NpgsqlDbType.Text).Value = "Программа";
                command.Parameters.Add("@name_pc", NpgsqlDbType.Text).Value = namePcTextBlock.Text;
                command.Parameters.Add("@ip_pc", NpgsqlDbType.Text).Value = ipAdressTextBlock.Text;
                command.Parameters.Add("@login_user", NpgsqlDbType.Text).Value = userNameAdTextBlock.Text;




                adapter.SelectCommand = command;
                command.ExecuteReader();
                db_connect.closeConnection();*/






        }

    }
}
