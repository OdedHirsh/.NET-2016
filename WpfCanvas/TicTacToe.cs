using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCanvas
{
    public enum CellValue
    {
        X, O, EMPTY
    }

    public enum MoveResult
    {
        VALID_MOVE, INVALID_MOVE, VICTORY, DRAW
    }

    class TicTacToe
    {
        private CellValue[] board;
        public bool isXTurn;
        private int moveCount;

        public TicTacToe()
        {
            board = new CellValue[9];
            reset();
        }

        public void reset()
        {
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = CellValue.EMPTY;
            }
            isXTurn = true;
            moveCount = 0;
        }

        public MoveResult makeMove(int cell)
        {
            if (cell < 1 || cell > 9)
                throw new Exception("cell must be 1-9");
            cell--;
            if (board[cell] == CellValue.EMPTY)
            {
                moveCount++;
                board[cell] = isXTurn ? CellValue.X : CellValue.O;
                isXTurn = !isXTurn;
                if (moveCount >= 5 && checkVictory(cell))
                {
                    return MoveResult.VICTORY;
                }
                if (moveCount == 9)
                    return MoveResult.DRAW;
                return MoveResult.VALID_MOVE;
            }
            return MoveResult.INVALID_MOVE;
        }

        private bool checkVictory(int cell)
        {
            int row = cell / 3;
            int column = cell % 3;
            if (board[column] == board[column + 3] && board[column] == board[column + 6])
                return true;
            row *= 3;
            if (board[row] == board[row + 1] && board[row] == board[row + 2])
                return true;
            if (cell%2 ==0)
            {
                bool diagonal1 = board[0] != CellValue.EMPTY && board[0] == board[4] && board[0] == board[8];
                if (diagonal1)
                {
                    return true;
                }
                bool diagonal2 = board[2] != CellValue.EMPTY && board[2] == board[4] && board[2] == board[6];
                if (diagonal2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
