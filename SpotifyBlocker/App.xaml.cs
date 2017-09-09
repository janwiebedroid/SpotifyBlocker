using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpotifyBlocker
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string AppGuid =
            ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
        protected override void OnStartup(StartupEventArgs e)
        {
            if (Process.GetProcessesByName("spotify").Length <= 0)
            {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe");
                while (Process.GetProcessesByName("spotify").Length <= 0)
                {
                    Thread.Sleep(1000);
                }
                Thread.Sleep(2000);
            }
            string mutexId = $"Local\\{{{AppGuid}}}";

            using (Mutex mutex = new Mutex(false, mutexId))
            {
                if (mutex.WaitOne(TimeSpan.Zero))
                {
                    base.OnStartup(e);
                    mutex.ReleaseMutex();
                }
                else
                {
                    //TOOD: implement show original instance
                }
            }


        }
    }
}
