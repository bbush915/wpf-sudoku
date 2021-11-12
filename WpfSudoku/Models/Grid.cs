//-----------------------------------------------------------------------------
// <copyright file="Grid.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

namespace WpfSudoku.Models;

internal record struct Grid
{
    public Cell[] Cells { get; init; } = new Cell[81];
}

