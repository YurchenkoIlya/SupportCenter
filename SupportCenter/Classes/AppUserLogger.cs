using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class AppUserLogger
    {
        private readonly LogService _logger;
        private string _userAd;
        private string _displayName;
        private string _computerName;
        private string _ipAddress;
        private int _userId;

        public string UserAd => _userAd;
        public string DisplayName => _displayName;
        public string ComputerName => _computerName;
        public string IpAddress => _ipAddress;
        public int UserId => _userId;

        public AppUserLogger(LogService logger, int userId)
        {
            _logger = logger;
            _userId = userId;

            InitializeUserInfo();
        }

        private void InitializeUserInfo()
        {
            // Получаем доменный логин
            _userAd = WindowsIdentity.GetCurrent().Name.Replace(@"ZAVOD\", "");
            // Для теста
            //_userAd = "YurchenkoIV";

            // Получаем доменное ФИО
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = UserPrincipal.Current;
                _displayName = user.DisplayName;
            }
            catch
            {
                _displayName = _userAd; // заглушка, если не удаётся получить ФИО
            }

            // Получаем имя ПК
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem");
                ManagementObjectCollection collection = searcher.Get();
                _computerName = (string)collection.Cast<ManagementBaseObject>().First()["Name"];
            }
            catch
            {
                _computerName = Environment.MachineName;
            }

            // Получаем IP
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                _ipAddress = host.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "?";
            }
            catch
            {
                _ipAddress = "?";
            }
        }

        public async Task LogLoginAsync()
        {
            await _logger.LogAsync("Login", _computerName, _ipAddress, _userId, "Application started");
        }

        public async Task LogLogoutAsync()
        {
            await _logger.LogAsync("Logout", _computerName, _ipAddress, _userId, "Application closed");
        }
    }
}
