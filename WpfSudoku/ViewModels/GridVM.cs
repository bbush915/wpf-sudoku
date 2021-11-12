//-----------------------------------------------------------------------------
// <copyright file="GridVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

using System.Linq;

using WpfSudoku.Messages;
using WpfSudoku.Models;

namespace WpfSudoku.ViewModels;

internal sealed class GridVM : ObservableRecipient, IRecipient<GridChangedMessage>, IRecipient<ActiveCellChangedMessage>
{
    private Grid _grid = new();

    private CellVM[] _cells;
    private int? _activeCellIndex = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GridVM" /> class.
    /// </summary>
    public GridVM() : base()
    {
        this._cells = this._grid.Cells.Select((cell, index) => new CellVM() { Cell = cell, Index = index }).ToArray();

        this.IsActive = true;
    }

    public CellVM[] Cells
    {
        get => this._cells;
        set => this.SetProperty(ref this._cells, value);
    }

    public void Receive(GridChangedMessage message)
    {
        this._grid = message.Value;

        this.Cells = this._grid.Cells.Select((cell, index) => new CellVM() { Cell = cell, Index = index }).ToArray();
    }

    public void Receive(ActiveCellChangedMessage message)
    {
        if (this._activeCellIndex.HasValue)
        {
            this.Cells[this._activeCellIndex.Value].IsActive = false;
        }

        this._activeCellIndex = (message.Value == null) ? null : message.Value.Index;
    }
}

