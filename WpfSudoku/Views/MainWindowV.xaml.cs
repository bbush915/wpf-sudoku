﻿//-----------------------------------------------------------------------------
// <copyright file="MainWindowV.xaml.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using System.Windows;

using WpfSudoku.ViewModels;

namespace WpfSudoku.Views;

public partial class MainWindowV : Window
{
    public MainWindowV()
    {
        this.InitializeComponent();

        this.DataContext = App.Current.ServiceProvider.GetRequiredService<MainWindowVM>();
    }
}

