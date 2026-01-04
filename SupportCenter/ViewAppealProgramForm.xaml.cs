using System;
using System.Collections.Generic;
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
    }
}
