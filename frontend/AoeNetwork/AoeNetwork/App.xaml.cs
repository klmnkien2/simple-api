using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AoeNetwork
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p => p.ProcessName == proc.ProcessName).Count();
            if (count > 1)
            {
                App.Current.Shutdown();
            }
        }
    }
}
