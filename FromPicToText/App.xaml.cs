using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FromPicToText
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
                if (e.Args[0].Split('.').Last() == "png")
                    StartPath.path = e.Args[0];
                else
                {
                    MessageBox.Show("questo file non è compatibile", "Errore nell'apertura del file", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Environment.Exit(1);
                }

            base.OnStartup(e);
        }
    }
}
