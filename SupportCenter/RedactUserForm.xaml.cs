using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для RedactUserForm.xaml
    /// </summary>
    public partial class RedactUserForm : Window
    {
        public RedactUserForm()
        {
            InitializeComponent();
        }

        private void adminButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void acceptRedactButton_Click(object sender, RoutedEventArgs e)
        {
            int role = 0;
            int activity = 0;

            if (roleUserComboBox.SelectedItem == "Пользователь") role = 0;
            else role = 1;

            if(activityCheckBox.IsChecked == true) activity = 1;
            else activity = 0;


                dbConnect db_connect = new dbConnect();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            db_connect.openConnection();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE main.users SET user_role=@role,activity=@activity where user_id=@id", db_connect.GetConnection());
            command.Parameters.Add("@id", NpgsqlDbType.Bigint).Value = Convert.ToInt32(idUserTextBlock.Text);          
            command.Parameters.Add("@role", NpgsqlDbType.Bigint).Value = role;
            command.Parameters.Add("@activity", NpgsqlDbType.Bigint).Value = activity;

            adapter.SelectCommand = command;
            command.ExecuteReader();
            MessageBox.Show("Пользователь отредактирован");
            db_connect.closeConnection();
            this.Close();
        }
    }
}
