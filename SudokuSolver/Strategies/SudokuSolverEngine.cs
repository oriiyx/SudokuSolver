using SudokuSolver.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Strategies
{
    class SudokuSolverEngine
    {
        private readonly SudokuBoardStateManager _sudokuBoardStateManager;
        private readonly SudokuMapper _sudokuMapper;

        public SudokuSolverEngine(SudokuBoardStateManager sudokuBoardStateManager, SudokuMapper sudokuMapper)
        {
            _sudokuBoardStateManager = sudokuBoardStateManager;
            _sudokuMapper = sudokuMapper;
        }

        public bool Solve(int[,] sudokuBoard)
        {
            // initialize all of our strategies
            List<ISudokuStrategy> strategies = new List<ISudokuStrategy>()
            {
                new SimpleMarkUpStrategy(_sudokuMapper),
                new NakedPairStrategy(_sudokuMapper)
            };

            //generate the state for the initial state
            var currentState = _sudokuBoardStateManager.GenerateState(sudokuBoard);

            //nextstate is follow after we run the first strategie and things change
            var nextState = _sudokuBoardStateManager.GenerateState(strategies.First().Solve(sudokuBoard));

            // if the first strategie already solved the sudoku we dont need a while loop
            // and if the strategies are not changing anything - are not working anymore because its a really hard sudoku or something
            // stop the while loop
            while(!_sudokuBoardStateManager.IsSolved(sudokuBoard) && currentState != nextState)
            {
                // we change the states from the worse one to the newer one
                currentState = nextState;

                // we loop throught all strategies that we have prepaired and check the while loop again if we solved it
                foreach(var strategy in strategies)
                {
                    nextState = _sudokuBoardStateManager.GenerateState(strategy.Solve(sudokuBoard));
                }
            }

            return _sudokuBoardStateManager.IsSolved(sudokuBoard);
        }
    }
}
