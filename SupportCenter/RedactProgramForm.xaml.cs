using SupportCenter.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// Логика взаимодействия для RedactProgramForm.xaml
    /// </summary>
    public partial class RedactProgramForm : Window
    {
        private ProgramDto _selectedProgram;

        public RedactProgramForm(ProgramDto selectedProgram)
        {

            InitializeComponent();
            _selectedProgram = selectedProgram;
        }

        private async void createProgramButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new ProgramDto
            {
                id_program = _selectedProgram.id_program,
                name_program = nameProgram.Text,
                way_program = wayProgram.Text,
            };



            var api = new ProgramApiRedact();
            string result = await api.UpdateProgramAsync(dto);

            MessageBox.Show(result, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameProgram.Text = _selectedProgram.name_program;
            wayProgram.Text = _selectedProgram.way_program;

        }
    }
}
