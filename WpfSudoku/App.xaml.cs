//-----------------------------------------------------------------------------
// <copyright file="App.xaml.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

using WpfSudoku.Services;
using WpfSudoku.Services.Interfaces;
using WpfSudoku.ViewModels;
using WpfSudoku.Views;

namespace WpfSudoku;

public sealed partial class App : Application
{
    public static new App Current => (App)Application.Current;

    public IServiceProvider ServiceProvider { get; private set; } = new ServiceCollection().BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services
            .AddTransient<ISudokuService, SudokuService>()
            .AddTransient<MainWindowV>()
            .AddTransient<MainWindowVM>()
            .AddTransient<GridVM>()
            .AddTransient<ControlPanelVM>();

        this.ServiceProvider = services.BuildServiceProvider();

        var mainWindow = this.ServiceProvider.GetRequiredService<MainWindowV>();
        mainWindow.Show();
    }
}

