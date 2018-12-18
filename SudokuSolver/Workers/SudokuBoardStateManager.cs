using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Workers
{
    class SudokuBoardStateManager
    {
        public string GenerateState(int[,] sudokuBoard)
        {
            StringBuilder key = new StringBuilder();

            //getlength v tem primeru izbere multidimentional array katerega želimo (prvi element ali drugi)
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {             
                //getlength v tem primeru izbere multidimentional array katerega želimo (prvi element ali drugi) 
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    key.Append(sudokuBoard[row, col]);
                }             
            }

            return key.ToString();
        }

        public bool IsSolved(int[,] sudokuBoard)
        {
            //getlength v tem primeru izbere multidimentional array katerega želimo (prvi element ali drugi)
            for (int row = 0; row < sudokuBoard.GetLength(0); row++)
            {
                //getlength v tem primeru izbere multidimentional array katerega želimo (prvi element ali drugi) 
                for (int col = 0; col < sudokuBoard.GetLength(1); col++)
                {
                    // pregled če je slučajno vrednost znotraj enaka 0 ali pa, če je dolžina števila večja kot 1 potem nekaj ni prav
                    // sudoku ima lahko v enem elementu samo števila od 1-9 --- nesme biti npr 0 ali 1031 ali 96522542
                    if(sudokuBoard[row, col] == 0 || sudokuBoard[row, col].ToString().Length > 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
