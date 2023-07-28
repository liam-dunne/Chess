using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{

    public class Piece
    {
        public string team;
        public int[,] moveGrid = new int[8, 8];
        private bool _isBlocked = false;
        public bool isBlocked { get { return _isBlocked; } set { _isBlocked = value; } }

        private bool _hasMoved = false;
        public bool hasMoved { get { return _hasMoved; } set { _hasMoved = value; } }

        private bool _canMove = false;
        public bool canMove { get { return _canMove; } set { _canMove = value; } }

        public Piece(string Team)
        {
            team = Team;
        }
        public virtual int[,] moveRules(Point coords, bool check)
        {
            return moveGrid;
        }
    }
    class Pawn : Piece
    {
        
        public Pawn(string Team) : base(Team)
        {
        }

        // Checks the diagonal squares to see if there is an enemy piece there, and if there is makes it a legal move
        private int[,] canTake(int[,] moveGrid, Point coords, int xDirection, int yDirection, string team)
        {
            if (CalculateMoves.moveIsValid(coords.X + xDirection, coords.Y + yDirection))
            {
                if (Form1.pieceGrid[coords.X + xDirection, coords.Y + yDirection] != null)
                {
                    if (Form1.pieceGrid[coords.X + xDirection, coords.Y + yDirection].team != team)
                    {
                        moveGrid[coords.X + xDirection, coords.Y + yDirection] = 2;
                    }
                }
            }
            return moveGrid;
        }


        private int[,] moveForward(int steps, Point coords)
        {
            if (team == "white") //Allows the pawn to move 1 square forwards 
            {
                if (CalculateMoves.moveIsValid(coords.X, coords.Y + steps))
                {
                    if (Form1.pieceGrid[coords.X, coords.Y + steps] != null)
                    {
                        this.isBlocked = true;
                        return moveGrid;
                    }
                    else
                    {
                        moveGrid[coords.X, coords.Y + steps] = 1;
                    }
                }
            }
            else if (team == "black")
            {
                if (CalculateMoves.moveIsValid(coords.X, coords.Y - steps))
                {
                    if (Form1.pieceGrid[coords.X, coords.Y - steps] != null)
                    {
                        this.isBlocked = true;
                        return moveGrid;
                    }
                    else
                    {
                        moveGrid[coords.X, coords.Y - steps] = 1;
                    }
                }
            }

            return moveGrid;
        }

        public override int[,] moveRules(Point coords, bool check) // Returns an array with '1' in the squares where a move can be made
        {
            Array.Clear(moveGrid);

                if (hasMoved == false)
                {
                    for (int i = 1; i < 3; i++) // Can move twice if the pawn hasn't moved yet
                    {
                        if (isBlocked == false)
                        {
                            moveGrid = moveForward(i, coords);
                        }
                        else
                        {
                            moveGrid = moveForward(1, coords);
                        }
                    }
                }
                else
                {
                    moveGrid = moveForward(1, coords);
                }

                // Check for pieces diagonally
                for (int i = -1; i < 2; i += 2)
                {
                    if (team == "white")
                    {
                        moveGrid = canTake(moveGrid, coords, i, 1, this.team);
                    }
                    else
                    {
                        moveGrid = canTake(moveGrid, coords, i, -1, this.team);
                    }
                }
            
                // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {             
                if (team == "white")
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                else
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
            }

            this.isBlocked = false;

            return moveGrid;
        }
    }
    class Knight : Piece
    {
        public Knight(string Team) : base(Team)
        {
        }
        
        // Goes through each possible move for the knight and labels it with a 1 if it is in the range of the array
        public override int[,] moveRules(Point coords, bool check)
        {
            Array.Clear(moveGrid);

            int x = coords.X;
            int y = coords.Y;

            if (CalculateMoves.moveIsValid(x + 1, y + 2))
                moveGrid[x + 1, y + 2] = 1;
            if (CalculateMoves.moveIsValid(x + 1, y - 2))
                moveGrid[x + 1, y - 2] = 1;
            if (CalculateMoves.moveIsValid(x + 2, y + 1))
                moveGrid[x + 2, y + 1] = 1;
            if (CalculateMoves.moveIsValid(x + 2, y - 1))
                moveGrid[x + 2, y - 1] = 1;
            if (CalculateMoves.moveIsValid(x - 1, y + 2))
                moveGrid[x - 1, y + 2] = 1;
            if (CalculateMoves.moveIsValid(x - 1, y - 2))
                moveGrid[x - 1, y - 2] = 1;
            if (CalculateMoves.moveIsValid(x - 2, y + 1))
                moveGrid[x - 2, y + 1] = 1;
            if (CalculateMoves.moveIsValid(x - 2, y - 1))
                moveGrid[x - 2, y - 1] = 1;

            moveGrid = CalculateMoves.canTake(moveGrid, this.team, coords);
            // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {
                if (team == "white")
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                else
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
            }

            return moveGrid;
        }
    }
    class Bishop : Piece
    {
        public Bishop(string Team) : base(Team)
        {
        }
        
        public override int[,] moveRules(Point coords, bool check)
        {
            Array.Clear(moveGrid);

            int x = coords.X;
            int y = coords.Y;

            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, -1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, -1, coords);

            moveGrid = CalculateMoves.canTake(moveGrid, this.team, coords);
            // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {
                if (team == "white")
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                else
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
            }
            return moveGrid;
        }
    }
    class Rook : Piece
    {
        public bool canCastle = true;
        public Rook(string Team) : base(Team)
        {
        }

        public override int[,] moveRules(Point coords, bool check)
        {
            Array.Clear(moveGrid);

            int x = coords.X;
            int y = coords.Y;

            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, 0, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, 0, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 0, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 0, -1, coords);

            moveGrid = CalculateMoves.canTake(moveGrid, this.team, coords);
            // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {
                if (team == "white")
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                else
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
            }
            return moveGrid;
        }
    }
    class Queen : Piece
    {
        public Queen(string Team) : base(Team)
        {
        }

        public override int[,] moveRules(Point coords, bool check)
        {
            Array.Clear(moveGrid);

            int x = coords.X;
            int y = coords.Y;

            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, 0, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, 0, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 0, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 0, -1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, 1, -1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, 1, coords);
            moveGrid = CalculateMoves.calculateLongMoves(moveGrid, this.team, -1, -1, coords);

            moveGrid = CalculateMoves.canTake(moveGrid, this.team, coords);
            // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {
                if (team == "white")
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                else
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
            }
            return moveGrid;

        }

    }
    class King : Piece
    {
        public bool canCastle = true;
        public King(string Team) : base(Team)
        {
        }

        
        
        
        // If a possible move contains an enemy piece, set to 2 but if it contains an ally piece, set to 0

        // Sets all possible moves to 1
        public override int[,] moveRules(Point coords, bool check)
        {
            this.canMove = false;
            Array.Clear(moveGrid);

            int x = coords.X;
            int y = coords.Y;

            for (int i = -1; i < 2; i+=2)
            {
                if (CalculateMoves.moveIsValid(x + i, y))
                    moveGrid[x + i, y] = 1;
                if (CalculateMoves.moveIsValid(x, y + i))
                    moveGrid[x, y + i] = 1;
                if (CalculateMoves.moveIsValid(x + i, y + i))
                    moveGrid[x + i, y + i] = 1;
                if (CalculateMoves.moveIsValid(x + i, y - i))
                    moveGrid[x + i, y - i] = 1;
            }

            moveGrid = CalculateMoves.canTake(moveGrid, this.team, coords);

            
            // If in check, test each move to see if the king is still in check after it. If so, disallow the move
            if (check)
            {
                if (team == "white")
                {
                    moveGrid = CalculateMoves.canCastle(coords, moveGrid, "black", "left");
                    moveGrid = CalculateMoves.canCastle(coords, moveGrid, "black", "right");
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "black", coords, Form1.pieceGrid);
                }
                else
                {
                    moveGrid = CalculateMoves.canCastle(coords, moveGrid, "white", "left");
                    moveGrid = CalculateMoves.canCastle(coords, moveGrid, "white", "right");
                    moveGrid = CalculateMoves.causesCheck(moveGrid, "white", coords, Form1.pieceGrid);
                }
            }
            return moveGrid;
        }

    }
}
