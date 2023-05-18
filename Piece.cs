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
        private bool _hasMoved = false;
        public bool hasMoved { get { return _hasMoved; } set { _hasMoved = value; } }
        
        public Pawn(string Team) : base(Team)
        {
        }
        private int[,] moveForward(int steps, Point coords)
        {
            if (team == "white") //Allows the pawn to move 1 square forwards 
            {
                moveGrid[coords.X, coords.Y - steps] = 1;
            }
            else if (team == "black")
            {
                moveGrid[coords.X, coords.Y + steps] = 1;
            }
            return moveGrid;
        }

        public override int[,] moveRules(Point coords) // Returns an array with '1' in the squares where a move can be made
        {
            Array.Clear(moveGrid);

            if (hasMoved == false)
            { 
                for (int i = 1; i < 3; i++) // Can move twice if the pawn hasn't moved yet
                {
                    moveGrid = moveForward(i, coords);
                }
                this.hasMoved = true;
            }
            else
            {
                moveGrid = moveForward(1, coords);
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
