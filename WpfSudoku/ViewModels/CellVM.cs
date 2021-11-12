//-----------------------------------------------------------------------------
// <copyright file="CellVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

using System.Windows.Input;

using WpfSudoku.Messages;
using WpfSudoku.Models;

namespace WpfSudoku.ViewModels;

internal sealed class CellVM : ObservableObject
{
    private bool _isActive = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CellVM" /> class.
    /// </summary>
    public CellVM()
    {
        this.ActivateCommand = new RelayCommand(this.Activate);
    }

    public Cell Cell { get; init; } = new();

    public int Index { get; init; }

    public string Value
    {
        get => (this.Cell.Value > 0) ? this.Cell.Value.ToString() : string.Empty;
    }

    public bool IsGiven
    {
        get => this.Cell.IsGiven;
    }

    public bool IsActive
    {
        get => this._isActive;
        set => this.SetProperty(ref this._isActive, value);
    }

    public ICommand ActivateCommand { get; private set; }

    private void Activate()
    {
        if (this.IsActive)
        {
            this.IsActive = false;
            WeakReferenceMessenger.Default.Send(new ActiveCellChangedMessage(null));
        }
        else
        {
            this.IsActive = true;
            WeakReferenceMessenger.Default.Send(new ActiveCellChangedMessage(this));
        }

    }
}

