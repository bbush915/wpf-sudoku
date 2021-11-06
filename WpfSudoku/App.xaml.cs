using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

using WpfSudoku.ViewModels;
using WpfSudoku.Views;

namespace WpfSudoku;

public partial class App : Application
{
    public static new App? Current => Application.Current as App;

    public IConfiguration? Configuration { get; private set; }
    public IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var configurationBuilder = new ConfigurationBuilder();

        this.Configuration = configurationBuilder.Build();

        var services = new ServiceCollection();

        services
            .AddTransient(typeof(MainWindowV))
            .AddTransient(typeof(MainWindowVM));

        this.ServiceProvider = services.BuildServiceProvider();

        var mainWindow = this.ServiceProvider.GetRequiredService<MainWindowV>();
        mainWindow.Show();
    }
}

