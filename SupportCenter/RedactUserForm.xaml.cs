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

        private async void acceptRedactButton_Click(object sender, RoutedEventArgs e)
        {
            int role = 0;
            int activity = 0;

            if (roleUserComboBox.SelectedItem?.ToString() == "Пользователь")
                role = 0;
            else
                role = 1;

            if (activityCheckBox.IsChecked == true)
                activity = 1;
            else
                activity = 0;

            var dto = new EditUserDto
            {
                Id = Convert.ToInt32(idUserTextBlock.Text),
                role = role == 0 ? "Пользователь" : "Администратор",
                ActivityFlag = activity == 1
            };

            var api = new UserApiRedact();
            string result = await api.UpdateUserAsync(dto);

            MessageBox.Show(result, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}
