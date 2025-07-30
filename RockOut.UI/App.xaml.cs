using System.Configuration;
using System.Data;
using System.Windows;
using RockOut.Core;

namespace RockOut.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.Init();
        Log.Event("Application started");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Event("Application exited");
        base.OnExit(e);
    }
}

