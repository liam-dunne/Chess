using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{
    internal class EnPassant
    {
        public static bool enPassantAvailable = false; // Stores whether en passant is a legal move

        // Determines whether a pawn is moving 2 forward or not
        public static bool isDoubleMove(Point selectedCoords, Point moveCoords, Piece selectedPiece)
        {
            if (selectedPiece.GetType() == typeof(Pawn))
            {
                if (selectedPiece.team == "white") // If a white pawn moves 2 squares up en passant becomes legal
                {
                    if (moveCoords.X == selectedCoords.X && moveCoords.Y == selectedCoords.Y + 2)
                    {
                        return true;
                    }
                }
                else if (selectedPiece.team == "black") // If a black pawn moves 2 squares down en passant becomes legal
                {
                    if (moveCoords.X == selectedCoords.X && moveCoords.Y == selectedCoords.Y - 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
