//-----------------------------------------------------------------------------
// <copyright file="ISudokuService.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using WpfSudoku.Models;

namespace WpfSudoku.Services.Interfaces;

internal interface ISudokuService
{
    Puzzle GeneratePuzzle(int? seed = null);
}

