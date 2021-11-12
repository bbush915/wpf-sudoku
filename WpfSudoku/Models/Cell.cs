//-----------------------------------------------------------------------------
// <copyright file="Cell.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

namespace WpfSudoku.Models;

internal record struct Cell(int Value, bool IsGiven);
