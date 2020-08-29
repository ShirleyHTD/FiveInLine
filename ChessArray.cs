using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInLine
{
    enum CHESSMAN {
        WHITE = -1,
        NOTHING = 0,
        BLACK = 1,
    }

    struct TurnManager
    {
        public CHESSMAN color;
        public int currentColumn;
        public int currentRow;
    };

    class ChessArray
    {
        private int[,] chessArray;
        public int rows;
        public int columns;
        private TurnManager turnManager = new TurnManager();
        public int[] victoryLine;
        //private Stack<TurnManager> stack = new Stack<TurnManager>();

        public ChessArray(int row, int col)
        {
            rows = row*3;
            columns = col*3;
            chessArray = new int[row*3,col*3];
            victoryLine = new int[4];

            for(int i = 0; i < row*3; i++)
            {
                for(int j = 0; j < col*3; j++)
                {
                    chessArray[i, j] = (int)CHESSMAN.NOTHING;
                }
            }

            turnManager.color = CHESSMAN.NOTHING;
            //stack.Push(turnManager);
        }

        public void addChessMan(int row, int col, CHESSMAN chessman)
        {
            if(chessArray[row,col] != (int)CHESSMAN.NOTHING || chessman==CHESSMAN.NOTHING)
            {
                return;
            }

            if(turnManager.color == CHESSMAN.NOTHING && chessman != CHESSMAN.BLACK)
            {
                return;
            }

            if (chessman == turnManager.color)
            {
                deletChessMan(turnManager.currentRow, turnManager.currentColumn);
                chessArray[row, col] = (int)chessman;
                turnManager.color = chessman;
                turnManager.currentColumn = col;
                turnManager.currentRow = row;
            }
            else
            {
                
                turnManager.color = chessman;
                turnManager.currentColumn = col;
                turnManager.currentRow = row;
                chessArray[row, col] = (int)chessman;
            }

            //checkStatus(row,col, chessArray[row, col]);
        }

        public void deletChessMan(int row, int col)
        {
            chessArray[row, col] = (int)CHESSMAN.NOTHING;
        }

        private bool verify(int row, int col, CHESSMAN current)
        {
            //TurnManager tmp = stack.Pop();
            if (turnManager.color != CHESSMAN.NOTHING)
            {
                if (turnManager.color == current)
                {
                    return false;
                }else
                {
                    if(current == CHESSMAN.BLACK)
                    {
                        turnManager.color = CHESSMAN.WHITE;
                        turnManager.currentColumn = col;
                        turnManager.currentRow = row;
                        return true;
                    }
                    turnManager.currentColumn = col;
                    turnManager.currentRow = row;
                    turnManager.color = CHESSMAN.BLACK;
                    return true;
                }
            }
            return true;
        }

        public bool checkStatus(int curRow, int curCol, int curColor)
        {
            int row = 0;
            int col = 0;

            int hCount = -1;
            row = curRow;
            col = curCol;
            while(chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                hCount++;
                col--;
            }
            victoryLine[0] = row;
            victoryLine[1] = col+1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                hCount++;
                col++;
            }
            if (hCount >= 5)
            {
                victoryLine[2] = row;
                victoryLine[3] = col-1;
                return true;
            }

            int vCount = -1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                vCount++;
                row--;
            }
            victoryLine[0] = row+1;
            victoryLine[1] = col;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                vCount++;
                row++;
            }
            if (vCount >= 5)
            {
                victoryLine[2] = row-1;
                victoryLine[3] = col;
                return true;
            }

            int d1Count = -1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                d1Count++;
                col--;
                row--;
            }
            victoryLine[0] = row+1;
            victoryLine[1] = col+1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                d1Count++;
                col++;
                row++;
            }
            if (d1Count >= 5)
            {
                victoryLine[2] = row-1;
                victoryLine[3] = col-1;
                return true;
            }

            int d2Count = -1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                d2Count++;
                col--;
                row++;
            }
            victoryLine[0] = row-1;
            victoryLine[1] = col+1;
            row = curRow;
            col = curCol;
            while (chessArray[row, col] == curColor && col >= 0 && row >= 0)
            {
                d2Count++;
                col++;
                row--;
            }
            if (d2Count >= 5)
            {
                victoryLine[2] = row+1;
                victoryLine[3] = col-1;
                return true;
            }

            return false;

        }

        public int[,] getArray()
        {
            return chessArray;
        }
    }
}
