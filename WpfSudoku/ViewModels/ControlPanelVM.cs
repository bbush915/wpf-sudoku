//-----------------------------------------------------------------------------
// <copyright file="ControlPanelVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

using WpfSudoku.Messages;

namespace WpfSudoku.ViewModels;

internal sealed class ControlPanelVM : ObservableRecipient, IRecipient<ActiveCellChangedMessage>
{
    private CellVM? _activeCell = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ControlPanelVM" /> class.
    /// </summary>
    public ControlPanelVM()
    {
        this.Items = Enumerable
            .Range(0, 9)
            .Select(n => new ControlItemVM() { Index = n, UpdateCell = CurryUpdateCell(n + 1) })
            .ToArray();

        this.IsActive = true;
    }

    public ControlItemVM[] Items { get; private set; }

    public void Receive(ActiveCellChangedMessage message)
    {
        this._activeCell = message.Value;

        foreach (var item in this.Items)
        {
            if (this._activeCell == null)
            {
                item.IsEnabled = false;
                item.IsActive = false;
            }
            else
            {
                // NOTE - We don't enable the control item for the given cells.

                item.IsEnabled =  !this._activeCell.IsGiven;

                // NOTE - We activate the control item if the value matches or 
                // the associated pencil mark was set.

                item.IsActive = (this._activeCell.Value == (item.Index + 1).ToString()) || this._activeCell.PencilMarks.Get(item.Index);
            }
        }
    }

    private Action<bool> CurryUpdateCell(int value)
    {
        void UpdateCell(bool isActive)
        {
            if (this._activeCell == null)
            {
                return;
            }

            this._activeCell.Update(value, isActive);
        }

        return UpdateCell;
    }
}

