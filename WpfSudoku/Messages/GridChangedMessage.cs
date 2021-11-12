//-----------------------------------------------------------------------------
// <copyright file="GridChangedMessage.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.Messaging.Messages;

using WpfSudoku.Models;

namespace WpfSudoku.Messages;

internal sealed class GridChangedMessage : ValueChangedMessage<Grid>
{
    public GridChangedMessage(Grid grid) : base(grid) { }
}

