using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{
    internal class CalculateMoves
    {

        public static bool countsAsMove = true;

        public int[,] calculateMoves()
        {
            throw new NotImplementedException();
        }

        public static bool moveIsValid(int x, int y)
        {
            return !(x < 0 || x > 7 || y < 0 || y > 7);
        }
        public static int[,] canTake(int[,] grid, string team, Point coords)
        {
            Piece[,] testPieceGrid = Form1.pieceGrid;

            // Check each square on the board. If marked with a 1, mark 2 if an enemy piece is there or mark 0 if a friendly piece is there
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


       
        //In future might have to make the move where it reaches a piece legal to allow taking


        // Takes in the location of a piece and the direction it wants to move, and returns an array with all possible moves marked 1

        public static int[,] calculateLongMoves(int[,] moveGrid, string team, int xDirection, int yDirection, Point coords)
        {
            int x = coords.X;
            int y = coords.Y;


            for (int i = 1; i < 8; i++)
            {
                if (CalculateMoves.moveIsValid(x + xDirection * i, y + yDirection * i))
                    // If a piece is in the way, no moves after this allowed
                    if (Form1.pieceGrid[x + xDirection * i, y + yDirection * i] != null)
                    {
                        moveGrid[x + xDirection * i, y + yDirection * i] = 1;
                        break;
                    }
                    else
                        moveGrid[x + xDirection * i, y + yDirection * i] = 1;
            }

            return moveGrid;
        }

        // For every square on the board, if the location of the opposing king is in the piece at that square's moveGrid, return true, else false 
        public static bool isCheck(string team, Piece[,] grid)
        {
            int[,] testGrid = new int[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Array.Clear(testGrid);
                    if (grid[i, j] != null)
                        if (grid[i, j].team == team)
                        {
                            Point coords = new Point(i, j);
                            testGrid = grid[i, j].moveRules(coords, false);
                            if (team == "black")
                            {
                                if (testGrid[Form1.whiteKingLocation.X, Form1.whiteKingLocation.Y] == 2)
                                    return true;
                            }
                            else if (team == "white")
                            {
                                if (testGrid[Form1.blackKingLocation.X, Form1.blackKingLocation.Y] == 2)
                                    return true;
                            }

                        }
                }
            return false;
        }

        // For every square on the board, if the square is a legal move in the given moveGrid, create a new board with the piece moved to that square
        // If in this position the moving team is in check, disallow the move.
        public static int[,] causesCheck(int[,] moveGrid, string team, Point coords, Piece[,] pieceGrid)
        {
            countsAsMove = false;
            
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (new[] { 1, 2 }.Contains(moveGrid[i, j]))
                    {
                        Point tempKingLocation = new Point(0, 0);
                        Piece? temp = pieceGrid[i, j];
                        pieceGrid[i, j] = pieceGrid[coords.X, coords.Y]; // Set the new square to the old square's piece
                        pieceGrid[coords.X, coords.Y] = null; // reset the old square
                        if (pieceGrid[i, j] != null)
                        {
                            if (pieceGrid[i, j].GetType() == typeof(King))
                            {
                                if (pieceGrid[i, j].team == "white")
                                {
                                    tempKingLocation = Form1.whiteKingLocation;
                                    Form1.whiteKingLocation = new Point(i, j);
                                }
                                else
                                {
                                    tempKingLocation = Form1.blackKingLocation;
                                    Form1.blackKingLocation = new Point(i, j);
                                }
                            }
                        }
                        if (isCheck(team, pieceGrid))
                        {
                            moveGrid[i, j] = 0; // If the move causes check disallow it
                            if (pieceGrid[i, j].GetType() == typeof(King))
                            {
                                if (i == coords.X + 1 && moveIsValid(i + 1, j)) 
                                {
                                    if (moveGrid[i + 1, j] == 1) // If moving the king one square to the right causes check, disallow the move to the right of that
                                    {
                                        moveGrid[i + 1, j] = 0;
                                    }
                                }
                                else if (i == coords.X - 1 && moveIsValid(i - 1, j)) // Repeat for left side
                                {
                                    if (moveGrid[i - 1, j] == 1)
                                    {
                                        moveGrid[i - 1, j] = 0;
                                    }
                                }
                            }
                        }
                        pieceGrid[coords.X, coords.Y] = pieceGrid[i, j];
                        pieceGrid[i, j] = temp;
                        if (pieceGrid[coords.X, coords.Y] != null)
                        {
                            if (pieceGrid[coords.X, coords.Y].GetType() == typeof(King)) // Reset the king's location
                            {
                                if (pieceGrid[coords.X, coords.Y].team == "white")
                                {
                                    Form1.whiteKingLocation = tempKingLocation;
                                }
                                else
                                {
                                    Form1.blackKingLocation = tempKingLocation;
                                }
                            }
                        }
                    
                    }
                }
            }
            countsAsMove = true;
            return moveGrid;
        }
        
        
        public static int[,] canCastle(Point coords, int[,] moveGrid, string team, string direction)
        {
            int directionMultiplier;
            int offset;

            if (direction == "left")
            {
                directionMultiplier = -1; // Multiplies values to reverse their direction
                offset = -1; // Moves left by one when added
            }
            else
            {
                directionMultiplier = 1; // Multiplies values to keep their direction the same
                offset = 0; // Has no effect when added
            }

            Rook rook = new Rook("white");
            King king = (King)Form1.pieceGrid[coords.X, coords.Y];

            if (moveIsValid(coords.X + (3 * directionMultiplier) + offset, coords.Y))
            {
                if (Form1.pieceGrid[coords.X + (3 * directionMultiplier) + offset, coords.Y] != null) // Check if there is a rook in the required location
                    if (Form1.pieceGrid[coords.X + (3 * directionMultiplier) + offset, coords.Y].GetType() == typeof(Rook))
                    {
                        rook = (Rook)Form1.pieceGrid[coords.X + (3 * directionMultiplier) + offset, coords.Y];
                    }
                    else
                        return moveGrid;
                else
                    return moveGrid;
            }
            
            if (king.hasMoved || rook.hasMoved || isCheck(team, Form1.pieceGrid)) // If the king or rook has moved or is in check, can't castle
                return moveGrid;

            for (int i = 1; i <= 2; i++) // Check each square to the right of the king for pieces
            {
                if (moveIsValid(coords.X + (i * directionMultiplier), coords.Y))
                    if (Form1.pieceGrid[coords.X + (i * directionMultiplier), coords.Y] != null)
                    {
                        return moveGrid; // Returns moveGrid if a piece is in that square
                    }
            }

            moveGrid[coords.X + (2 * directionMultiplier), coords.Y] = 1;
            return moveGrid;
            
        }
    }
}
