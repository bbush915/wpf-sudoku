//-----------------------------------------------------------------------------
// <copyright file="ControlItemVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WpfSudoku.ViewModels;

internal sealed class ControlItemVM : ObservableObject
{
    private bool _isActive = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlItemVM" /> class.
    /// </summary>
    public ControlItemVM()
    {
        this.Label = string.Empty;
    }

    public string Label { get; init; }

    public bool IsActive
    {
        get => this._isActive;
        set => this.SetProperty(ref this._isActive, value);
    }
}

