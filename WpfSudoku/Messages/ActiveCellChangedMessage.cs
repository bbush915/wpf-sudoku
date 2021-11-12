//-----------------------------------------------------------------------------
// <copyright file="ActiveCellChangedMessage.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.Messaging.Messages;

using WpfSudoku.ViewModels;

namespace WpfSudoku.Messages;

internal sealed class ActiveCellChangedMessage : ValueChangedMessage<CellVM?>
{
    public ActiveCellChangedMessage(CellVM? cell) : base(cell) { }
}

