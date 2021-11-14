//-----------------------------------------------------------------------------
// <copyright file="ControlItemVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WpfSudoku.ViewModels;

internal sealed class ControlItemVM : ObservableObject
{
    private bool _isActive = default;
    private bool _isEnabled = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlItemVM" /> class.
    /// </summary>
    public ControlItemVM()
    {
        this.ToggleCommand = new RelayCommand(this.Toggle);
    }

    public int Index { get; init; } = default;

    public Action<bool> UpdateCell { get; init; } = (_) => { };

    public string Label => (this.Index + 1).ToString();

    public RelayCommand ToggleCommand { get; private set; }

    public bool IsActive
    {
        get => this._isActive;
        set => this.SetProperty(ref this._isActive, value);
    }

    public bool IsEnabled
    {
        get => this._isEnabled;
        set => this.SetProperty(ref this._isEnabled, value);
    }

    private void Toggle()
    {
        this.IsActive = !this.IsActive;

        this.UpdateCell(this.IsActive);
    }
}

