//-----------------------------------------------------------------------------
// <copyright file="CellVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

using System.Collections;
using System.Linq;
using System.Windows.Input;

using WpfSudoku.Messages;
using WpfSudoku.Models;

namespace WpfSudoku.ViewModels;

internal sealed class CellVM : ObservableObject
{
    private Cell _cell;

    private bool _isActive = default;

    public CellVM(Cell cell)
    {
        this._cell = cell;

        this.PencilMarks = new BitArray(9);
        
        if (this._cell.Value > 0)
        {
            this.PencilMarks.Set(this._cell.Value - 1, true);
        }

        this.ToggleCommand = new RelayCommand(this.Toggle);
    }

    public int Index { get; init; }

    public bool ShowValue => this._cell.IsGiven || (this._cell.Value > 0);

    public string Value => this._cell.Value.ToString();
     
    public bool ShowPencilMarks => !this.ShowValue;

    public BitArray PencilMarks { get; private set; }

    public bool IsGiven =>  this._cell.IsGiven;

    public bool IsActive
    {
        get => this._isActive;
        set => this.SetProperty(ref this._isActive, value);
    }

    public ICommand ToggleCommand { get; private set; }

    public void Update(int value, bool isActive)
    {
        // NOTE - The desired behavior is that we convert to pencil marks if we
        // set more than one value, and back to values if we remove all but one
        // pencil mark.
        //
        // Initially, we both set the value and the pencil mark, then clean up
        // based on the pencil mark count.

        this._cell.Value = isActive ? value : 0;
        this.PencilMarks.Set(value - 1, isActive);

        var pencilMarkCount = this.PencilMarks.Cast<bool>().Count(x => x == true);

        if (pencilMarkCount == 0 || pencilMarkCount > 1)
        {
            this._cell.Value = 0;
        }
        else
        {
            // NOTE - We use the sole pencil mark to set the value.

            var newValue = this.PencilMarks.Cast<bool>().Select((value, index) => (Value: value, Index: index)).Single(x => x.Value == true).Index + 1;
            this._cell.Value = newValue;
        }

        this.OnPropertyChanged(nameof(this.ShowValue));
        this.OnPropertyChanged(nameof(this.Value));
        this.OnPropertyChanged(nameof(this.ShowPencilMarks));
        this.OnPropertyChanged(nameof(this.PencilMarks));
    }

    private void Toggle()
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

