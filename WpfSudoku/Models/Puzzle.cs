//-----------------------------------------------------------------------------
// <copyright file="Puzzle.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

namespace WpfSudoku.Models;

internal record struct Puzzle
{
    public Grid Grid { get; init; } = new Grid();
    public int[] Solution { get; init; } = new int[81];
}

