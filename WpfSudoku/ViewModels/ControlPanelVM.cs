//-----------------------------------------------------------------------------
// <copyright file="ControlPanelVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WpfSudoku.ViewModels;

internal sealed class ControlPanelVM : ObservableObject
{
    private ControlItemVM[] _items;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlPanelVM" /> class.
    /// </summary>
    public ControlPanelVM()
    {
        this._items = Enumerable.Range(1, 9).Select(n => new ControlItemVM() { Label = n.ToString() }).ToArray();
    }

    public ControlItemVM[] Items
    {
        get => this._items;
        set => this.SetProperty(ref this._items, value);
    }
}

