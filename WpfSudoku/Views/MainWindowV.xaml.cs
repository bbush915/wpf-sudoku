//-----------------------------------------------------------------------------
// <copyright file="MainWindowV.xaml.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using WpfSudoku.ViewModels;

namespace WpfSudoku.Views;

public sealed partial class MainWindowV : Window
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MainWindowV" /> class.
    /// </summary>
    public MainWindowV()
    {
        this.InitializeComponent();

        this.DataContext = App.Current.ServiceProvider.GetRequiredService<MainWindowVM>();
    }
}

