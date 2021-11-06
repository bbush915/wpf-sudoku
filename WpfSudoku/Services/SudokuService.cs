using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfSudoku.Services;

internal static class SudokuService
{
    public static (List<int> clues, List<int> solution) Generate(int? seed = null)
    {
        var rng = (seed == null) ? new Random() : new Random((int)seed);

        var solution = GenerateValidGrid(rng);

        var clues = solution.ToList();
        RemoveCells(clues, rng, backtrackThreshold: 100, clueThreshold: 35);

        return (clues, solution);
    }

    public static (List<int[]> solutions, bool isComplete) Solve(List<int> cells, SolveGoal? goal = SolveGoal.AllSolutions, int? backtrackThreshold = null)
    {
        var solutions = new List<int[]>();

        var steps = new Stack<Step>();
        var backtrackCount = 0;

        while (true)
        {
            if (backtrackThreshold.HasValue && (backtrackCount >= backtrackThreshold.Value))
            {
                // NOTE - Hit backtrack threshold, so return our solutions thus
                // far and a valud indicating we did not finish.

                return (solutions, isComplete: false);
            }

            var step = GetStep(cells);

            if (step == null)
            {
                // NOTE - Failed to generate a new step, which means we found a
                // solution.

                var solution = cells.ToArray();
                solutions.Add(solution);

                // NOTE - If we found a solution, then we have proved
                // solvability and can stop.

                if (goal == SolveGoal.ProveSolvable)
                {
                    break;
                }

                // NOTE - If we found multiple solutions, then we have
                // disproved uniqueness and can stop.

                if ((solutions.Count > 1) && (goal == SolveGoal.DisproveUniqueness))
                {
                    break;
                }

                // NOTE - Continue on.

                backtrackCount += 1;

                if (!TryBacktrack(cells, steps))
                {
                    break;
                }
            }
            else
            {
                if (step.Candidates.Count == 0)
                {
                    // NOTE - No candidates left to try for this step, so we 
                    // need to backtrack.

                    backtrackCount += 1;

                    if (!TryBacktrack(cells, steps))
                    {
                        break;
                    }
                }
                else
                {
                    // NOTE - Try next candidate for this step.

                    var candidate = step.Candidates.Pop();
                    cells[step.CellIndex] = candidate;

                    steps.Push(step);
                }
            }
        }

        return (solutions, isComplete: true);
    }

    private static List<int> GenerateValidGrid(Random rng)
    {
        var cells = Enumerable.Repeat(0, 81).ToList();

        var cellIndices = Enumerable.Range(0, 81).ToList();

        while (cellIndices.Count > 0)
        {
            // NOTE - Pick a random cell.

            var cellIndex = cellIndices[rng.Next(cellIndices.Count)];

            // NOTE - Pick a random candidate value.

            var candidates = GetCandidates(cells, cellIndex);
            cells[cellIndex] = candidates.ToList()[rng.Next(candidates.Count)];

            // NOTE - Verify that the grid is still solvable.

            var (solutions, _) = Solve(cells.ToList(), goal: SolveGoal.ProveSolvable, backtrackThreshold: 100);

            if (solutions.Count == 0)
            {
                // NOTE - Unable to prove the grid is solvable, so we must
                // remove our candidate and try the process again.

                cells[cellIndex] = 0;
            }
            else
            {
                // NOTE - We've locked down the value for this cell

                cellIndices.Remove(cellIndex);
            }
        }

        return cells;
    }

    private static void RemoveCells(List<int> cells, Random rng, int? backtrackThreshold, int? clueThreshold)
    {
        var cellIndices = Enumerable.Range(0, 81).ToList();
        Shuffle(cellIndices, rng);

        var counter = 0;

        foreach (var cellIndex in cellIndices)
        {
            var oldValue = cells[cellIndex];

            cells[cellIndex] = 0;

            var (solutions, isComplete) = Solve(cells.ToList(), goal: SolveGoal.DisproveUniqueness, backtrackThreshold);

            if (solutions.Count > 1 || !isComplete)
            {
                // NOTE - We can no longer prove that the grid is unique, so we
                // must revert our change and try the next cell.

                cells[cellIndex] = oldValue;
            }
            else
            {
                counter += 1;

                if (clueThreshold.HasValue && (counter >= (81 - clueThreshold.Value)))
                {
                    // NOTE - We hit our desired clue threshold, so we can stop.
                    break;
                }
            }
        }
    }

    private static Step? GetStep(List<int> cells)
    {
        var firstEmptyCellIndex = cells.IndexOf(0);

        if (firstEmptyCellIndex == -1)
        {
            return null;
        }

        // NOTE - We find the 'best' cell based on the number of possible
        // candidates. Lower is better.

        var bestCellIndex = firstEmptyCellIndex;
        var bestCellCandidates = GetCandidates(cells, bestCellIndex);

        for (var i = bestCellIndex + 1; i < cells.Count; i++)
        {
            if (cells[i] != 0)
            {
                continue;
            }

            var candidates = GetCandidates(cells, i);

            if (candidates.Count < bestCellCandidates.Count)
            {
                bestCellIndex = i;
                bestCellCandidates = candidates;
            }
        }

        var step = new Step(bestCellIndex, bestCellCandidates);

        return step;
    }

    private static bool TryBacktrack(List<int> cells, Stack<Step> steps)
    {
        while (true)
        {
            // NOTE - Cannot backtrack any further.

            if (steps.Count == 0)
            {
                return false;
            }

            // NOTE - Grab the previous step...

            var step = steps.Pop();

            if (step.Candidates.Count == 0)
            {
                // NOTE - And undo it if we have no more candidates to try.

                cells[step.CellIndex] = 0;
            }
            else
            {
                // NOTE - And try the next candidate.

                var candidate = step.Candidates.Pop();
                cells[step.CellIndex] = candidate;

                steps.Push(step);

                return true;
            }
        }
    }

    private static Stack<int> GetCandidates(List<int> cells, int cellIndex)
    {
        // NOTE - Check each number 1-9 for viability.

        var candidates = new Stack<int>();

        for (var i = 0; i < 9; i++)
        {
            var candidate = i + 1;

            if (ValidateCandidate(cells, cellIndex, candidate))
            {
                candidates.Push(candidate);
            }
        }

        return candidates;
    }

    private static bool ValidateCandidate(List<int> cells, int cellIndex, int candidate)
    {
        // NOTE - Check column for duplicate.

        var columnIndex = cellIndex % 9;

        for (var i = 0; i < 9; i++)
        {
            var scanCellIndex = 9 * i + columnIndex;

            if (cells[scanCellIndex] == candidate)
            {
                return false;
            }
        }

        // NOTE - Check row for duplicate.

        var rowIndex = cellIndex / 9;

        for (var j = 0; j < 9; j++)
        {
            var scanCellIndex = 9 * rowIndex + j;

            if (cells[scanCellIndex] == candidate)
            {
                return false;
            }
        }

        // NOTE - Check box for duplicate.

        var stackIndex = columnIndex / 3;
        var bandIndex = rowIndex / 3;

        for (var k = 0; k < 9; k++)
        {
            var scanCellIndex = 3 * (stackIndex + 9 * bandIndex) + (9 * (k / 3) + (k % 3));

            if (cells[scanCellIndex] == candidate)
            {
                return false;
            }
        }

        return true;
    }

    private static void Shuffle(List<int> values, Random rng)
    {
        for (var i = values.Count; i > 0; i--)
        {
            Swap(values, 0, rng.Next(0, i));
        }
    }

    private static void Swap(List<int> values, int i, int j)
    {
        var temp = values[i];
        values[i] = values[j];
        values[j] = temp;
    }

    public enum SolveGoal
    {
        ProveSolvable,
        DisproveUniqueness,
        AllSolutions
    }

    private record Step(int CellIndex, Stack<int> Candidates);
}

