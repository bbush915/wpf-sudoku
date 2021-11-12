//-----------------------------------------------------------------------------
// <copyright file="GridV.xaml.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

using System.Windows.Controls;

using WpfSudoku.ViewModels;

namespace WpfSudoku.Views.UserControls;

internal sealed partial class GridV : UserControl
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GridV" /> class.
    /// </summary>
    public GridV()
    {
        this.InitializeComponent();

        this.DataContext = App.Current.ServiceProvider.GetRequiredService<GridVM>();
    }
}

