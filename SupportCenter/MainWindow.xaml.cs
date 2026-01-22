using Npgsql;
using NpgsqlTypes;
using SupportCenter.Classes;
using System.Data;
using System.Data.Common;
using System.DirectoryServices.AccountManagement;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;
using SupportCenter.Api;
using System.Windows.Threading;

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
            _ = UpdateConnectionStatusAsync();
            InitializeConnectionChecker();
        }
        private void InitializeConnectionChecker()
        {
            connectionTimer = new DispatcherTimer();
            connectionTimer.Interval = TimeSpan.FromSeconds(5); // проверка каждые 5 секунд
            connectionTimer.Tick += ConnectionTimer_Tick;
            connectionTimer.Start();
        }
        private DispatcherTimer connectionTimer;
        private async void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            bool isConnected = await CheckDbConnectionAsync();

            if (isConnected)
            {
                connectStatusText.Text = "Сетевое соединение исправно";
                connectStatusGrid.Background = Brushes.Green;
            }
            else
            {
                connectStatusText.Text = "Сетевое соединение отсутствует, позвоните на 5005";
                connectStatusGrid.Background = Brushes.Red;
            }
            
        }
        private async Task<bool> CheckDbConnectionAsync()
        {
            try
            {
                dbConnect db_connect = new dbConnect();
                db_connect.openConnection();
                // Можно выполнить простую команду, чтобы убедиться в доступности
                using var cmd = new Npgsql.NpgsqlCommand("SELECT 1", db_connect.GetConnection());
                await cmd.ExecuteScalarAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task UpdateConnectionStatusAsync()
        {
            bool isConnected = await CheckDbConnectionAsync();

            if (isConnected)
            {
                connectStatusText.Text = "Сетевое соединение исправно";
                connectStatusGrid.Background = Brushes.Green;
            }
            else
            {
                connectStatusText.Text = "Сетевое соединение отсутствует, позвоните на 5005";
                connectStatusGrid.Background = Brushes.Red;

                appealButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB15507"));             
                adminButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB15507"));
                appealButton.IsEnabled = false;
                adminButton.IsEnabled = false;
            }
        }
        public async Task loadForm()
        {

            string displayName=null;
            string userAd =null;
            string computerName=null;
            string localIP = null;
            
            // Получаем доменный логин


            userAd = WindowsIdentity.GetCurrent().Name;
            userAd = userAd.Replace(@"ZAVOD\", "");           
            // Полкчаем доменное ФИО
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = UserPrincipal.Current;
                displayName = user.DisplayName;
                
            }
            catch
            {
                nameAdTextBlock.Text = "Юрченко Илья Вадимович"; // заглушка
            }
            // Получаем оменное имя ПК
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            computerName = (string)collection.Cast<ManagementBaseObject>().First()["Name"];

            // Получаем ИП адрес компьютера
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

         /*   Session.CurrentUserLogin = userAd;
            Session.CurrentUserName = displayName;
            Session.CurrentIp = localIP;
            Session.CurrentPcName = computerName;*/

            Session.CurrentUserLogin = "YurchenkoIV";
            Session.CurrentUserName = "Юрченко Илья Вадимович";
            Session.CurrentIp = "192.168.1.1";
            Session.CurrentPcName = "111111-1111";





            
            
            
            if (Session.AuthorizationStatus == 0)
            {
                await readLog();
                Session.AuthorizationStatus = 1;

            }


            namePcTextBlock.Text = Session.CurrentPcName;
            ipAdressTextBlock.Text = Session.CurrentIp;
            userNameAdTextBlock.Text = Session.CurrentUserLogin;
            nameAdTextBlock.Text = Session.CurrentUserName;

            checkRole();

        }
        public void checkRole()
        {
            
            
         
            dbConnect db_connect = new dbConnect();
            db_connect.openConnection();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand("Select user_role from main.users where login_user= @loginUser ", db_connect.GetConnection());
            command.Parameters.Add("@loginUser", NpgsqlDbType.Text).Value = Session.CurrentUserLogin;
            
            var role = command.ExecuteScalar();
            Session.Role = Convert.ToInt32(role);



        }
        public async Task readLog()
        {
            if (!string.IsNullOrEmpty(Session.CurrentUserLogin))
            {
                var logService = new LogServiceApi();

                await logService.SendLogAsync(
                    action: "Вход в программу",
                    objectAction: "Программа",
                    namePc: Session.CurrentPcName,
                    ipPc: Session.CurrentIp,
                    loginUser: Session.CurrentUserLogin
                );
            }

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
            string role;

            if (Session.Role == 1) role = "Администратор";
            else role = "Пользователь";
            MessageBox.Show(
             $"Логин: {Session.CurrentUserLogin}\n" +
             $"Имя: {Session.CurrentUserName}\n" +
             $"IP-адрес: {Session.CurrentIp}\n" +
             $"Имя ПК: {Session.CurrentPcName}\n"+
             $"Роль: {role}\n",
             "Данные текущей сессии",
    MessageBoxButton.OK,
    MessageBoxImage.Information
);
        } 
        private void regulations_Click(object sender, RoutedEventArgs e)
        {
            RegulationsForm regulationsForm = new RegulationsForm();
            regulationsForm.Show();
            this.Close();
        }

        private void instrumentButton_Click(object sender, RoutedEventArgs e)
        {
            ToolsForm toolsForm = new ToolsForm();
            toolsForm.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        
            connectionTimer?.Stop();
            connectionTimer = null;
        
    }
    }
}