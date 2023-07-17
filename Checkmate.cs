using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Official_Chess_Actual
{
    internal class Checkmate
    {
        public static void CheckMate()
        {           
            Form1.ActiveForm.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }

        public static bool isCheckmate(Piece selectedPiece, string selectedPieceTeam, Button[,] grid, int[,] moveGrid, Piece[,] pieceGrid)
        {
            Point blackKingLocation = Form1.blackKingLocation;
            Point whiteKingLocation = Form1.whiteKingLocation;
            if (selectedPieceTeam == "white") // If white has just moved
            {
                string opposingTeam = "black"; // Check black pieces
                grid[blackKingLocation.X, blackKingLocation.Y].BackColor = Color.IndianRed;
                selectedPiece = pieceGrid[blackKingLocation.X, blackKingLocation.Y];
                moveGrid = selectedPiece.moveRules(blackKingLocation, true); // Find possible moves for black's king

                return possibleMovesExist(selectedPiece, moveGrid, pieceGrid, opposingTeam);
            }
            else
            {
                string opposingTeam = "white"; // Check black pieces
                grid[whiteKingLocation.X, whiteKingLocation.Y].BackColor = Color.IndianRed;
                selectedPiece = pieceGrid[whiteKingLocation.X, whiteKingLocation.Y];
                moveGrid = selectedPiece.moveRules(whiteKingLocation, true); // Find possible moves for black's king

                return possibleMovesExist(selectedPiece, moveGrid, pieceGrid, opposingTeam);
            }
            
        }

        static bool possibleMovesExist(Piece selectedPiece, int[,] moveGrid, Piece[,] pieceGrid, string opposingTeam)
        {
            for (int i = 0; i < 8; i++) // For each square on the board, if that square is a legal move for the king set canMove to true
                for (int j = 0; j < 8; j++)
                    if (new[] { 1, 2 }.Contains(moveGrid[i, j]))
                    {
                        selectedPiece.canMove = true;
                    }
            if (selectedPiece.canMove == false) // If the king can't move, check all other piece's moves (efficiency)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Point coords = new Point(i, j);
                        if (pieceGrid[i, j] != null) // If there is a piece on the opposing team, check if it can move
                            if (pieceGrid[i, j].team == opposingTeam)
                            {
                                selectedPiece = pieceGrid[i, j];
                                moveGrid = selectedPiece.moveRules(coords, true);
                                for (int k = 0; k < 8; k++)
                                {
                                    for (int l = 0; l < 8; l++)
                                    {
                                        if (new[] { 1, 2 }.Contains(moveGrid[k, l])) // If there is a legal move, return false
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }

                    }
                }

                return true;
            }
            return false;
        }
    }
}
