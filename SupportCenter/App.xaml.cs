using Npgsql;
using NpgsqlTypes;
using SupportCenter.Api;
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

        protected override async void OnExit(ExitEventArgs e)
        {
            
                if (!string.IsNullOrEmpty(Session.CurrentUserLogin))
                {
                    var logService = new LogServiceApi();

                    await logService.SendLogAsync(
                        action: "Выход из программы",
                        objectAction: "Программа",
                        namePc: Session.CurrentPcName,
                        ipPc: Session.CurrentIp,
                        loginUser: Session.CurrentUserLogin
                    );
                }
            }          
        }
    }

