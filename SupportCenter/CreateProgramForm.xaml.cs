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
    /// Логика взаимодействия для CreateProgramForm.xaml
    /// </summary>
    public partial class CreateProgramForm : Window
    {
        public CreateProgramForm()
        {
            InitializeComponent();
        }

   

        private void createProgramButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(nameProgram.Text)) MessageBox.Show("Не заполнено наименование программы");
            else if (string.IsNullOrWhiteSpace(wayProgram.Text)) MessageBox.Show("Не заполнен путь к программе");
            else
            {



                RedactUserForm redactForm = new RedactUserForm();

                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.program (name_program,way_program) VALUES (@nameFolder,@way_folder)", db_connect.GetConnection());
                command.Parameters.Add("@nameFolder", NpgsqlDbType.Text).Value = nameProgram.Text;
                command.Parameters.Add("@way_folder", NpgsqlDbType.Text).Value = wayProgram.Text;
                adapter.SelectCommand = command;
                command.ExecuteReader();
                db_connect.closeConnection();
                MessageBox.Show("Программа добавлена");

            }
        }
    }
}
