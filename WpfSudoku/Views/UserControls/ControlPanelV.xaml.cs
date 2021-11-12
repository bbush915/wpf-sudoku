//-----------------------------------------------------------------------------
// <copyright file="ControlPanelV.xaml.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using WpfSudoku.ViewModels;

namespace WpfSudoku.Views.UserControls;

internal sealed partial class ControlPanelV : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlPanelV" /> class.
    /// </summary>
    public ControlPanelV()
    {
        this.InitializeComponent();

        this.DataContext = App.Current.ServiceProvider.GetRequiredService<ControlPanelVM>();
    }
}

