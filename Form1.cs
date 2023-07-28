namespace Official_Chess_Actual
{


    public partial class Form1 : Form
    {
        bool pieceSelected = false; // Whether or not a piece is selected
        Piece? selectedPiece; // The currently selected piece
        Point selectedCoords;       

        string turn = "white";

        public int[,] moveGrid = new int[8, 8]; // Create an empty 8x8 grid for the possible moves

        public static Piece[,] pieceGrid = new Piece[8, 8];
        public Button[,] grid = new Button[8, 8]; // Create an empty 8x8 grid of buttons

        string selectedPieceTeam; //Colour of the selected piece
        string selectedPieceType; //Type of selected piece
        string selectedPieceTypeChar; //First character of the selected piece's name (e.g k for king)
        string selectedPieceTeamChar; //First character of the selected piece's team (e.g w for white
        string selectedPieceImageCode; //The 2 letter combination of the name character and team character which forms a key in the image dictionary

        Color selectedColor;

        public static Point whiteKingLocation = new Point(4, 0);
        public static Point blackKingLocation = new Point(4, 7);

        //Dictionary to give a short code for each image, the first letter representing the colour and the second representing the piece
        Dictionary<string, Image> images = new Dictionary<string, Image>()
        {
            { "bp", Resource1.dark_pawn_image },
            { "wp", Resource1.light_pawn_image },
            { "bn", Resource1.dark_knight_image },
            { "wn", Resource1.light_knight_image },
            { "bb", Resource1.dark_bishop_image },
            { "wb", Resource1.light_bishop_image }, // :)
            { "br", Resource1.dark_rook_image },
            { "wr", Resource1.light_rook_image },
            { "bq", Resource1.dark_queen_image },
            { "wq", Resource1.light_queen_image },
            { "bk", Resource1.dark_king_image },
            { "wk", Resource1.light_king_image },
        };


        public Form1()
        {
            InitializeComponent();
            createBoard();
            displayImages();
            createPieceBoard();
        }

        public void createBoard()
        {
            Size = new Size(1080, 760); // Set window size
            BackColor = Color.Gray;

            Panel board = new Panel(); // Create a panel within the window for the board
            Panel borderPanel = new Panel();

            borderPanel.Size = new Size(680, 680);
            borderPanel.Location = new Point(200, 20);
            borderPanel.BackColor = Color.FromArgb(41, 44, 51);


            // Set size and location of panel within form
            board.Location = new Point(220, 20);
            board.Size = new Size(640, 640);
            board.Location = new Point(20, 20);

            Controls.Add(borderPanel);
            borderPanel.Controls.Add(board); // Add the board to the form



            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Create new button and set size and location
                    Button newButton = new Button();
                    newButton.Size = new Size(80, 80);
                    newButton.Location = new Point(i * 80, 560 - j * 80);
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.Click += new EventHandler(squareClick);
                    // Set background colour
                    if ((i + j) % 2 == 0)
                    {
                        newButton.BackColor = Color.DarkOliveGreen;
                    }
                    else
                    {
                        newButton.BackColor = Color.PapayaWhip;
                    }
                    newButton.FlatAppearance.BorderSize = 0;

                    // Add button to board and array
                    board.Controls.Add(newButton);
                    grid[i, j] = newButton;
                }
            }
        }

        public void createPieceBoard() // Creates a variable for each type of piece and places them in the correct locations within the board
        {
            for (int i = 0; i < 8; i++)
            {
                pieceGrid[i, 1] = new Pawn("white");
                pieceGrid[i, 6] = new Pawn("black");
            }
            pieceGrid[0, 7] = new Rook("black");
            pieceGrid[7, 7] = new Rook("black");
            pieceGrid[1, 7] = new Knight("black");
            pieceGrid[6, 7] = new Knight("black");
            pieceGrid[2, 7] = new Bishop("black");
            pieceGrid[5, 7] = new Bishop("black");
            pieceGrid[3, 7] = new Queen("black");
            pieceGrid[4, 7] = new King("black");
            pieceGrid[0, 0] = new Rook("white");
            pieceGrid[7, 0] = new Rook("white");
            pieceGrid[1, 0] = new Knight("white");
            pieceGrid[6, 0] = new Knight("white");
            pieceGrid[2, 0] = new Bishop("white");
            pieceGrid[5, 0] = new Bishop("white");
            pieceGrid[3, 0] = new Queen("white");
            pieceGrid[4, 0] = new King("white");

        }

        public void displayImages()
        {
            for (int i = 0; i < 8; i++) //Loads pawns from dictionary
            {
                grid[i, 6].Image = images["bp"];
                grid[i, 1].Image = images["wp"];
            }
            grid[0, 0].Image = images["wr"]; //Loads all images accessed from a dictionary
            grid[7, 0].Image = images["wr"];
            grid[1, 0].Image = images["wn"];
            grid[6, 0].Image = images["wn"];
            grid[2, 0].Image = images["wb"];
            grid[5, 0].Image = images["wb"];
            grid[3, 0].Image = images["wq"];
            grid[4, 0].Image = images["wk"];
            grid[0, 7].Image = images["br"];
            grid[7, 7].Image = images["br"];
            grid[1, 7].Image = images["bn"];
            grid[6, 7].Image = images["bn"];
            grid[2, 7].Image = images["bb"];
            grid[5, 7].Image = images["bb"];
            grid[3, 7].Image = images["bq"];
            grid[4, 7].Image = images["bk"];

        }

        public void squareClick(object sender, EventArgs e) // Click event for all squares
        {
            Button button = (Button)sender; // Cast sender to button type
            Point coords = getCoords(button); // Get the coordinates of the clicked button


            // If no piece selected, sets the selected piece to the one in the clicked square and finds possible moves
            if (!pieceSelected & pieceGrid[coords.X, coords.Y] != null)
            {
                if (pieceGrid[coords.X, coords.Y].team == turn)
                {
                    selectedPiece = pieceGrid[coords.X, coords.Y]; // Selects the piece at the clicked square
                    selectedCoords = coords; // Stores the location of the selected piece for later
                    pieceSelected = true;
                    selectedPieceTeam = selectedPiece.team;
                    selectedPieceTeamChar = selectedPieceTeam[0].ToString(); // Gives the first letter of the colour of the piece
                    selectedPieceType = selectedPiece.GetType().ToString();

                    selectedColor = button.BackColor;
                    button.BackColor = Color.PaleGoldenrod;


                    // Gives the first letter of the type of piece unless it is a knight which is represented by 'n'
                    if (selectedPieceType == "Official_Chess_Actual.Knight")
                    {
                        selectedPieceTypeChar = "n";
                    }
                    else
                    {
                        selectedPieceTypeChar = selectedPieceType.ToLower()[22].ToString();
                    }
                    selectedPieceImageCode = selectedPieceTeamChar + selectedPieceTypeChar; // Gives the 2 letter code corresponding to the key for the selected piece in the image dictionary

                    moveGrid = pieceGrid[selectedCoords.X, selectedCoords.Y].moveRules(selectedCoords, true);

                    // For each possible move, if it is a capture circle the piece else display a smaller circle
                    for (int i = 0; i < moveGrid.GetLength(0); i++)
                    {
                        for (int j = 0; j < moveGrid.GetLength(1); j++)
                        {
                            if (moveGrid[i, j] == 1)
                            {
                                grid[i, j].BackgroundImage = Resource1.grey_move_circle;
                                grid[i, j].BackgroundImageLayout = ImageLayout.Center;
                            }
                            else if (moveGrid[i, j] == 2)
                            {
                                grid[i, j].BackgroundImage = Resource1.grey_capture_circle;
                                grid[i, j].BackgroundImageLayout = ImageLayout.Center;
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(selectedPiece.hasMoved);
                    if (pieceGrid[3, 4] != null)
                        System.Diagnostics.Debug.WriteLine(pieceGrid[3, 4].GetType().ToString());
                }
            }



            // If the selected piece is clicked again, deselect it
            else if (coords == selectedCoords)
            {

                pieceSelected = false;
                selectedPiece = null;
                button.BackColor = selectedColor;

                grid = resetImages(grid);
            }

            // If a move is legal, moves the piece to the new square and clears the old square and changes the turn
            else if (pieceSelected & new[] { 1, 2 }.Contains(moveGrid[coords.X, coords.Y]))
            {
                if (CalculateMoves.countsAsMove)
                {
                    selectedPiece.hasMoved = true;
                    
                    
                    if (selectedPiece.GetType() == typeof(King))
                    {
                        if (coords.X == selectedCoords.X + 2 && coords.Y == selectedCoords.Y) // Move the rook left of the king when castling kingside
                        {
                            Piece rookToCastle = pieceGrid[selectedCoords.X + 3, selectedCoords.Y];
                            pieceGrid[coords.X - 1, coords.Y] = rookToCastle;
                            pieceGrid[selectedCoords.X + 3, selectedCoords.Y] = null;
                            grid[coords.X + 1, coords.Y].Image = null;
                            string rookImageCode = selectedPieceTeamChar + "r";
                            grid[coords.X - 1, coords.Y].Image = images[rookImageCode];
                            rookToCastle.hasMoved = true;
                        }
                        else if (coords.X == selectedCoords.X - 2 && coords.Y == selectedCoords.Y) // Move the rook right of the king when castling queenside
                        {
                            Piece rookToCastle = pieceGrid[selectedCoords.X - 4, selectedCoords.Y];
                            pieceGrid[coords.X + 1, coords.Y] = rookToCastle;
                            pieceGrid[selectedCoords.X - 4, selectedCoords.Y] = null;
                            grid[coords.X - 2, coords.Y].Image = null;
                            string rookImageCode = selectedPieceTeamChar + "r";
                            grid[coords.X + 1, coords.Y].Image = images[rookImageCode];
                            rookToCastle.hasMoved = true;
                        }
                    }
                }
                

                grid[selectedCoords.X, selectedCoords.Y].Image = null; // Set old location's image to null
                grid = resetImages(grid); // Removes all movement circles

                if (selectedPieceTeam == "white") //Places the correct colour piece based on selected piece colour
                    grid[coords.X, coords.Y].Image = images[selectedPieceImageCode];
                else
                    grid[coords.X, coords.Y].Image = images[selectedPieceImageCode];

                // Set new location's image to correct image and reset original location's image to null
                pieceGrid[coords.X, coords.Y] = selectedPiece;
                pieceGrid[selectedCoords.X, selectedCoords.Y] = null;
                pieceSelected = false;
                grid[selectedCoords.X, selectedCoords.Y].BackColor = selectedColor;

                // Set background colour of king to red if in check
                // Also if it is check, check if it is checkmate
                if (CalculateMoves.isCheck(selectedPieceTeam, pieceGrid))
                {                    
                    if (Checkmate.isCheckmate(selectedPiece, selectedPieceTeam, grid, moveGrid, pieceGrid))
                    {
                        Checkmate.CheckMate();
                    }
                }

                // If not in check reset king's colour to its default
                else
                {
                    if (selectedPieceTeam == "white")
                    {
                        if ((whiteKingLocation.X + whiteKingLocation.Y) % 2 == 0)
                            grid[whiteKingLocation.X, whiteKingLocation.Y].BackColor = Color.DarkOliveGreen;
                        else
                            grid[whiteKingLocation.X, whiteKingLocation.Y].BackColor = Color.PapayaWhip;
                    }
                    if (selectedPieceTeam == "black")
                    {
                        if ((blackKingLocation.X + blackKingLocation.Y) % 2 == 0)
                            grid[blackKingLocation.X, blackKingLocation.Y].BackColor = Color.DarkOliveGreen;
                        else
                            grid[blackKingLocation.X, blackKingLocation.Y].BackColor = Color.PapayaWhip;
                    }
                }

                // If moved piece was a king, update the variable storing its location
                if (selectedPieceTypeChar == "k")
                {
                    if (selectedPieceTeam == "white")
                        whiteKingLocation = new Point(coords.X, coords.Y);
                    else
                        blackKingLocation = new Point(coords.X, coords.Y);
                }
                
                // Change turn
                if (turn == "white")
                    turn = "black";
                else if (turn == "black")
                    turn = "white";




            }

        }
        // Divides the x and y coordinate by 80 to find the location within the board
        public Point getCoords(Button button)
        {
            int x = button.Location.X / 80;
            int y = 7 - button.Location.Y / 80;
            return new Point(x, y);
        }

        public Button[,] resetImages(Button[,] grid)
        {
            foreach (Button button in grid)
            {
                button.BackgroundImage = null;
            }
            return grid;
        }

        
    }
}