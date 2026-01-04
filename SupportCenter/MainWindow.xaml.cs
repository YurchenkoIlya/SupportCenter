using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System.Data;
using System.Data.Common;
using System.DirectoryServices.AccountManagement;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SupportCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void loadForm()
        {


            // Получаем доменный логин
            string userAd = WindowsIdentity.GetCurrent().Name;
            userAd = userAd.Replace(@"ZAVOD\", "");
             userNameAdTextBlock.Text = userAd;
            userNameAdTextBlock.Text = "YurchenkoIV";
            // Полкчаем доменное ФИО
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = UserPrincipal.Current;
                string displayName = user.DisplayName;
                nameAdTextBlock.Text = displayName;
            }
            catch
            {
               // nameAdTextBlock.Text = "Юрченко Илья Вадимович"; // заглушка
            }
            // Получаем оменное имя ПК
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            string computerName = (string)collection.Cast<ManagementBaseObject>().First()["Name"];
            namePcTextBlock.Text = computerName;

            // Получаем ИП адрес компьютера
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            ipAdressTextBlock.Text = Convert.ToString(localIP);

            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            try
            {


                connectStatusText.Text = "Сетевое соединение исправно";
                connectStatusGrid.Background = Brushes.Green;
              
                checkUser();



            }
            catch (Exception ex)
            {
                connectStatusText.Text = "Сетевое соединение отсутствует";
                connectStatusGrid.Background = Brushes.Red;

            }

            if (Session.AuthorizationStatus == 0)
            {
                readLog();
                Session.AuthorizationStatus = 1;

            }

            sessionWrite();




        }
        public void sessionWrite()
        {
            
            Session.CurrentUserLogin = userNameAdTextBlock.Text;
            Session.CurrentUserName = nameAdTextBlock.Text;
            Session.CurrentIp = ipAdressTextBlock.Text;
            Session.CurrentPcName = namePcTextBlock.Text;



        }
        public void readLog()
        {
          /*  DateTime currentDateTime = DateTime.Now;



            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.logs " +
                "(date,action,object_action,name_pc,ip_pc,id_user) " +
                "VALUES (date_trunc('second', @date),@action,@object_action,@name_pc,@ip_pc," +
                "(Select user_id from main.users where login_user =@login_user))", db_connect.GetConnection());
            
            
            command.Parameters.Add("@date", NpgsqlDbType.Timestamp).Value = currentDateTime;
            command.Parameters.Add("@action", NpgsqlDbType.Text).Value = "Вход в программу";
            command.Parameters.Add("@object_action", NpgsqlDbType.Text).Value = "Программа";
            command.Parameters.Add("@name_pc", NpgsqlDbType.Text).Value = namePcTextBlock.Text;
            command.Parameters.Add("@ip_pc", NpgsqlDbType.Text).Value = ipAdressTextBlock.Text;
            command.Parameters.Add("@login_user", NpgsqlDbType.Text).Value = userNameAdTextBlock.Text;




            adapter.SelectCommand = command;
            command.ExecuteReader();
            db_connect.closeConnection();


            */

        }
        
        public void checkUser()
        {        

            string loginAd = userNameAdTextBlock.Text;
            

            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("Select * from main.users where login_user= @aL ", db_connect.GetConnection());
            command.Parameters.Add("@aL", NpgsqlDbType.Text).Value = loginAd;
            NpgsqlDataReader reader = command.ExecuteReader();
            adapter.SelectCommand = command;

            int rowcount = 0;
            int role = 0;

            while (reader.Read())
            {

                rowcount++;
                role = Convert.ToInt32(reader[2]);
                

            }
            reader.Close();

            if (rowcount > 0)
            {
                
                Session.CurrentUserLogin = loginAd;
                if (role == 0) adminButton.Visibility = Visibility.Hidden;

            }          
            else
            {
                createUser(loginAd);
                

            }
            


            db_connect.closeConnection();



        }
        public void createUser(string login)
        {
            string userName = nameAdTextBlock.Text;

            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO main.users (login_user,user_role,user_name,activity) VALUES (@aL,@uR,@uN,@uA)", db_connect.GetConnection());
            command.Parameters.Add("@aL", NpgsqlDbType.Text).Value = login;
            command.Parameters.Add("@uR", NpgsqlDbType.Integer).Value = 0;
            command.Parameters.Add("@uN", NpgsqlDbType.Text).Value = userName;
            command.Parameters.Add("@uA", NpgsqlDbType.Integer).Value = 1;
            adapter.SelectCommand = command;
            command.ExecuteReader();
            db_connect.closeConnection();
            MessageBox.Show("ПОЛЬЗОВАТЕЛЬ ЗАРЕГИСТРИРОВАН.");

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadForm();

           
        


      
        }

        private void appealButton_Click(object sender, RoutedEventArgs e)
        {
            AppealForm appealForm = new AppealForm();
            this.Close();
            appealForm.Show();
        }

        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            this.Close();
            adminForm.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void copyNamePcButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, namePcTextBlock.Text);
            MessageBox.Show("Скопировано");
        }

        private void copyIpPcButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, ipAdressTextBlock.Text);
            MessageBox.Show("Скопировано");
        }

        private void copyNameAdButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, userNameAdTextBlock.Text);
            MessageBox.Show("Скопировано");
        }

        private void copyFioButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, nameAdTextBlock.Text);
            MessageBox.Show("Скопировано");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
             $"Логин: {Session.CurrentUserLogin}\n" +
             $"Имя: {Session.CurrentUserName}\n" +
             $"IP-адрес: {Session.CurrentIp}\n" +
             $"Имя ПК: {Session.CurrentPcName}",
             "Данные текущей сессии",
    MessageBoxButton.OK,
    MessageBoxImage.Information
);
        }
    }
}