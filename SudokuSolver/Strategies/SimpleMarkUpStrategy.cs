using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    class SimpleMarkUpStrategy : ISudokuStrategy
    {
        private readonly SudokuMapper _sudokuMapper;

        public SimpleMarkUpStrategy(SudokuMapper sudokuMapper)
        {
            _sudokuMapper = sudokuMapper;
        }

        public int[,] Solve(int[,] sudokuBoard)
        {
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                //getlength v tem primeru izbere multidimentional array katerega želimo (prvi element ali drugi) 
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    if (sudokuBoard[row, col] == 0 || sudokuBoard[row, col].ToString().Length > 1)
                    {
                        var possibilitiesInRowAndCol = GetPossibilitesInRowAndCol(sudokuBoard, row, col);
                        var possibilitiesInBlock = GetPossibilitiesInBlock(sudokuBoard, row, col);
                        sudokuBoard[row, col] = getPossibilityIntersection(possibilitiesInRowAndCol, possibilitiesInBlock);

                    }
                }
            }

            return sudokuBoard;
        }

        private int GetPossibilitesInRowAndCol(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            int[] possibilites = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //for all the columns
            for (int col = 0; col < 9; col++)
            {
                // the given row (lets say 0) we go throught all the columns
                // is valid single will check if its okay to 
                if (IsValidSingle(sudokuBoard[givenRow, col]))
                {
                    // if we find a number we have to remove that number from possibilites that it's a number we can insert into an empty slot
                    possibilites[sudokuBoard[givenRow, col] - 1] = 0;
                }
            }

            for (int row = 0; row < 9; row++)
            {
                if (IsValidSingle(sudokuBoard[row, givenCol]))
                {
                    possibilites[sudokuBoard[row, givenCol] - 1] = 0;
                }
            }

            // convert the array that we select (we use Join as a binder that get the array together) with LINQ query 
            // the LINQ query selects all the possibilites but we do not want 0's because we don't want them (no use in sudoku)
            // we start with all 1-9 numbers we switch some numbers to 0's and we remove those 0's out so we're left with the numbers that are possible
            return Convert.ToInt32(String.Join(string.Empty, possibilites.Select(p => p).Where(p => p != 0)));
        }

        private int GetPossibilitiesInBlock(int[,] sudokuBoard, int givenRow, int givenCol)
        {
            int[] possibilites = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var sudokuMap = _sudokuMapper.Find(givenRow, givenCol);

            for (int row = sudokuMap.StartRow; row <= sudokuMap.StartRow + 2 ; row++)
            {
                for (int col = sudokuMap.StartCol; col <= sudokuMap.StartCol + 2 ; col++)
                {
                    if (IsValidSingle(sudokuBoard[row, col]))
                    {
                        possibilites[sudokuBoard[row, col] - 1] = 0;
                    }
                }
            }

            return Convert.ToInt32(String.Join(string.Empty, possibilites.Select(p => p).Where(p => p != 0)));
        }

        // this checks if the digit is equal to 0 or if it's too big of a number (more than 9)
        private bool IsValidSingle(int cellDigit)
        {
            return cellDigit != 0 && cellDigit.ToString().Length < 2;
        }

        // we're checking if one of the possibilites (getRowAndCol or Block) are intersecting and we can deduct some possibilites away - can reduce the options in the end
        private int getPossibilityIntersection(int possibilitiesInRowAndCol, int possibilitiesInBlock)
        {
            var getPossibilityInRowAndColCharArray = possibilitiesInRowAndCol.ToString().ToCharArray();
            var possibilitiesInBlockCharArray = possibilitiesInBlock.ToString().ToCharArray();
            var possibilitesSubset = getPossibilityInRowAndColCharArray.Intersect(possibilitiesInBlockCharArray);

            return Convert.ToInt32(String.Join(string.Empty, possibilitesSubset));
        }
    }
}
