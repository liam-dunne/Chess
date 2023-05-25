using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{
    internal class CalculateMoves
    {
        public int[,] calculateMoves()
        {
            throw new NotImplementedException();
        }

        public static bool moveIsValid(int x, int y)
        {
            return !(x < 0 || x > 7 || y < 0 || y > 7);
        }
        public static int[,] canTake(int[,] grid, string team)
        {
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (grid[i, j] == 1 && Form1.pieceGrid[i, j] != null)
                    {
                        if (Form1.pieceGrid[i, j].team != team)
                        {
                            grid[i, j] = 2;
                        }
                        else if (Form1.pieceGrid[i, j].team == team)
                        {
                            grid[i, j] = 0;
                        }
                    }
                }
            }

            return grid;
        }
    }
}
