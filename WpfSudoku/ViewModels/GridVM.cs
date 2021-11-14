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
    private Grid _grid;
    private CellVM[] _cells;

    private int? _activeCellIndex = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GridVM" /> class.
    /// </summary>
    public GridVM() : base()
    {
        this._grid = new();
        this._cells = GetCells(this._grid);

        this.IsActive = true;
    }

    public CellVM[] Cells
    {
        get => this._cells;
        set => this.SetProperty(ref this._cells, value);
    }

    public void Receive(GridChangedMessage message)
    {
        // NOTE - When the grid changes, we regenerate our cells.

        this._grid = message.Value;
        this.Cells = GetCells(this._grid);
    }

    public void Receive(ActiveCellChangedMessage message)
    {
        // NOTE - Deactivate the previous active cell, if necessary.

        if (this._activeCellIndex != null)
        {
            this.Cells[this._activeCellIndex.Value].IsActive = false;
        }

        // NOTE - Store the index of the current active cell.

        this._activeCellIndex = message.Value?.Index;
    }

    private static CellVM[] GetCells(Grid grid) => grid.Cells.Select((cell, index) => new CellVM(cell) { Index = index }).ToArray();
}

