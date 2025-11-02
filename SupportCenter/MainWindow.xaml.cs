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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Получаем доменный логин
            string userAd = WindowsIdentity.GetCurrent().Name;           
            userAd = userAd.Replace(@"ZAVOD\", "");
            userNameAdTextBlock.Text = userAd;
      
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
                nameAdTextBlock.Text = "Компьютер не в домене";
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
        }
    }
}