using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Official_Chess_Actual
{
    internal class Promotion
    {
        static string[] promotionPiecesChars = { "n", "b", "r", "q" }; // List of characters representing the possible promotion pieces

        public static bool moveIsPromotion(Point coords) => coords.Y == 0 || coords.Y == 7; // Checks if the move is a promotion (only works when done on pawns since it only checks the y coordinate)
        public static Panel createPromotionPanel(string selectedPieceTeamChar, Point coords)
        {
            int directionMultiplier;

            if (selectedPieceTeamChar == "w") // Determines which side to display the promotion panel on depending on team
            {
                directionMultiplier = 1;
            }
            else
            {
                directionMultiplier = -1;
            }
            Panel promotionPanel = new Panel();
            promotionPanel.Size = new Size(320, 80);
            // For Location - X found by starting at leftmost point on board, adding the location within the board, then taking half the width of the panel off to centre it. Panel now centred around the left side of the button so add half button width (40)
            // For Location - Y found by starting at bottom and taking off location within board, then adding half the height to centre, and adding either -80 or 80 to display either above or below
            promotionPanel.Location = new Point(220 + coords.X * 80 - promotionPanel.Width / 2 + 40, 560 - (coords.Y * 80) + (80 * directionMultiplier) + promotionPanel.Height / 2);
            promotionPanel.BackColor = Color.Navy;

            promotionPanel = addPromotionButtons(promotionPanel, selectedPieceTeamChar);

            return promotionPanel;
        }
        private static Panel addPromotionButtons(Panel promotionPanel, string selectedPieceTeamChar)
        {
            for (int i = 0; i < 4; i++)
            {
                Button button = new Button();
                button.Size = new Size(80, 80);
                button.Location = new Point(i * 80, 0); // Places each button at increments of 80 

                string pieceCode = selectedPieceTeamChar + promotionPiecesChars[i]; // Code containing the team of the piece and the type of piece
                button.Image = Form1.images[pieceCode]; // Find the correct image using the code
                button.Click += (sender, EventArgs) => { promotionPieceClick(sender, EventArgs, promotionPanel, selectedPieceTeamChar); };
                promotionPanel.Controls.Add(button);
            }
            return promotionPanel;

        }

        public static void promotionPieceClick(object sender, EventArgs e, Panel promotionPanel, string selectedPieceTeamChar)
        {
            string selectedPieceTeam;

            if (selectedPieceTeamChar == "w")
            {
                selectedPieceTeam = "white";
            }
            else
            {
                selectedPieceTeam = "black";
            }

            Button button = (Button)sender;
            Point coords = new Point(button.Location.X, button.Location.Y);
            string promotionPieceChar = promotionPiecesChars[coords.X / 80]; // Find the position of the selected piece within the array

            Piece promotionPiece = new Piece(selectedPieceTeam);

            switch (promotionPieceChar) // Selects the clicked piece
            {
                case "n":
                    Knight knight = new Knight(selectedPieceTeam);
                    promotionPiece = knight;
                    break;
                case "b":
                    Bishop bishop = new Bishop(selectedPieceTeam);
                    promotionPiece = bishop;
                    break;
                case "r":
                    Rook rook = new Rook(selectedPieceTeam);
                    promotionPiece = rook;
                    break;
                case "q":
                    Queen queen = new Queen(selectedPieceTeam);
                    promotionPiece = queen;
                    break;
            }

            convertPromotionPiece(promotionPanel, selectedPieceTeamChar, promotionPiece, promotionPieceChar);
            
            promotionPanel.Hide(); // Hides the panel after a piece has been selected
        }

        static void convertPromotionPiece(Panel promotionPanel, string selectedPieceTeamChar, Piece promotionPiece, string promotionPieceChar)
        {
            int directionMultiplier;

            if (selectedPieceTeamChar == "w") // Determines which side to display the promotion panel on depending on team
            {
                directionMultiplier = 1;
            }
            else
            {
                directionMultiplier = -1;
            }

            //Finds the coordinates of the promotion square by reversing the calculations done to place the panel

            Point promotionSquareCoords = new Point((promotionPanel.Location.X + promotionPanel.Width / 2 - 260) / 80, (560 + 80 * directionMultiplier + promotionPanel.Height/2 - promotionPanel.Location.Y)/80);

            Form1.pieceGrid[promotionSquareCoords.X, promotionSquareCoords.Y] = promotionPiece; // Sets the promoted pawn to the selected piece

            string promotionPieceImageCode = selectedPieceTeamChar + promotionPieceChar;

            Form1.grid[promotionSquareCoords.X, promotionSquareCoords.Y].Image = Form1.images[promotionPieceImageCode]; // Sets the background image of the square to the correct piece

        }

        /*
         * 
         * 
         * ADD STALEMATE, RESIGNATION
         * 
         * 
         * 
         * 
         */

    }

}
