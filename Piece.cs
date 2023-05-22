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
        public Piece(string Team)
        {
            team = Team;
        }
        public virtual int[,] moveRules(Point coords)
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
            if (Form1.pieceGrid[coords.X+xDirection, coords.Y+yDirection] != null)
            {
                if (Form1.pieceGrid[coords.X + xDirection, coords.Y + yDirection].team != team)
                {
                    moveGrid[coords.X + xDirection, coords.Y + yDirection] = 2;
                }
            }
            return moveGrid;
        }


        private int[,] moveForward(int steps, Point coords)
        {
            if (team == "white") //Allows the pawn to move 1 square forwards 
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
            else if (team == "black")
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
            
            return moveGrid;
        }

        public override int[,] moveRules(Point coords) // Returns an array with '1' in the squares where a move can be made
        {
            Array.Clear(moveGrid);
            try 
            {
                if (hasMoved == false)
                {
                    for (int i = 1; i < 3; i++) // Can move twice if the pawn hasn't moved yet
                    {
                        if (isBlocked == false)
                        {
                            moveGrid = moveForward(i, coords);
                        }
                    }
                    this.hasMoved = true;
                }
                else
                {
                    moveGrid = moveForward(1, coords);
                }

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
            }
            catch (IndexOutOfRangeException e)
            {
                return moveGrid;
            }
            

            return moveGrid;
        }
    }
    class Knight : Piece
    {
        public Knight(string Team) : base(Team)
        {
        }
    }
    class Bishop : Piece
    {
        public Bishop(string Team) : base(Team)
        {
        }
    }
    class Rook : Piece
    {
        public Rook(string Team) : base(Team)
        {
        }
    }
    class Queen : Piece
    {
        public Queen(string Team) : base(Team)
        {
        }
    }
    class King : Piece
    {
        public King(string Team) : base(Team)
        {
        }
    }
}
