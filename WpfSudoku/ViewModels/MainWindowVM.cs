//-----------------------------------------------------------------------------
// <copyright file="MainWindowVM.cs">
//     Copyright (c) 2021 by Bryan Bush. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

using System.Linq;
using System.Windows.Input;

using WpfSudoku.Messages;
using WpfSudoku.Models;
using WpfSudoku.Services.Interfaces;

namespace WpfSudoku.ViewModels;

internal sealed class MainWindowVM
{
    private readonly ISudokuService _sudokuService;

    private Puzzle _puzzle;

    public MainWindowVM(ISudokuService sudokuService)
    {
        this._sudokuService = sudokuService;

        this.CheckCommand = new RelayCommand(this.Check);
        this.GenerateCommand = new RelayCommand(this.Generate);
        this.SolveCommand = new RelayCommand(this.Solve);
    }

    public ICommand CheckCommand { get; }

    public ICommand GenerateCommand { get; }

    public ICommand SolveCommand { get; }

    private void Check()
    {
        var cells = this._puzzle.Grid.Cells;

        var isCorrect = true;

        for (var i = 0; i < 81; i++)
        {
            if (!cells[i].IsGiven && (cells[i].Value > 0) && (cells[i].Value != this._puzzle.Solution[i]))
            {
                isCorrect = false;
            }
        }
    }

    private void Generate()
    {
        this._puzzle = this._sudokuService.GeneratePuzzle();

        WeakReferenceMessenger.Default.Send(new GridChangedMessage(this._puzzle.Grid));
    }

    private void Solve()
    {
        var grid = new Grid()
        {
            Cells = this._puzzle.Grid.Cells.ToArray()
        };

        for (var i = 0; i < 81; i++)
        {
            grid.Cells[i].Value = this._puzzle.Solution[i];
        }

        WeakReferenceMessenger.Default.Send(new GridChangedMessage(grid));
        WeakReferenceMessenger.Default.Send(new ActiveCellChangedMessage(null));
    }
}

