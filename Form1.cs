namespace Official_Chess_Actual
{

    
    public partial class Form1 : Form
    {
        bool pieceSelected = false; // Whether or not a piece is selected
        Piece? selectedPiece; // The currently selected piece
        Point selectedCoords;

        public int[,] moveGrid = new int[8, 8]; // Create an empty 8x8 grid for the possible moves

        public static Piece[,] pieceGrid = new Piece[8, 8];
        public Button[,] grid = new Button[8, 8]; // Create an empty 8x8 grid of buttons

        string selectedPieceTeam; //Colour of the selected piece
        string selectedPieceType; //Type of selected piece
        string selectedPieceTypeChar; //First character of the selected piece's name (e.g k for king)
        string selectedPieceTeamChar; //First character of the selected piece's team (e.g w for white
        string selectedPieceImageCode; //The 2 letter combination of the name character and team character which forms a key in the image dictionary
        
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
            //board.Location = new Point(220, 20);
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
                    newButton.Location = new Point(i * 80, j * 80);
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.Click += new EventHandler(squareClick);
                    // Set background colour
                    if ((i + j) % 2 == 1)
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
                pieceGrid[i, 6] = new Pawn("white");
                pieceGrid[i, 1] = new Pawn("black");
            }
            pieceGrid[0, 0] = new Rook("black");
            pieceGrid[7, 0] = new Rook("black");
            pieceGrid[1, 0] = new Knight("black");
            pieceGrid[6, 0] = new Knight("black");
            pieceGrid[2, 0] = new Bishop("black");
            pieceGrid[5, 0] = new Bishop("black");
            pieceGrid[3, 0] = new Queen("black");
            pieceGrid[4, 0] = new King("black");
            pieceGrid[0, 7] = new Rook("white");
            pieceGrid[7, 7] = new Rook("white");
            pieceGrid[1, 7] = new Knight("white");
            pieceGrid[6, 7] = new Knight("white");
            pieceGrid[2, 7] = new Bishop("white");
            pieceGrid[5, 7] = new Bishop("white");
            pieceGrid[3, 7] = new Queen("white");
            pieceGrid[4, 7] = new King("white");

        }

        public void displayImages()
        {
            for (int i = 0; i < 8; i++)
            {
                grid[i, 6].BackgroundImage = images["wp"];
                grid[i, 1].BackgroundImage = images["bp"];
            }
            grid[0, 0].BackgroundImage = images["br"]; //Loads all images accessed from a dictionary
            grid[7, 0].BackgroundImage = images["br"];
            grid[1, 0].BackgroundImage = images["bn"];
            grid[6, 0].BackgroundImage = images["bn"];
            grid[2, 0].BackgroundImage = images["bb"];
            grid[5, 0].BackgroundImage = images["bb"];
            grid[3, 0].BackgroundImage = images["bq"];
            grid[4, 0].BackgroundImage = images["bk"];
            grid[0, 7].BackgroundImage = images["wr"];
            grid[7, 7].BackgroundImage = images["wr"];
            grid[1, 7].BackgroundImage = images["wn"];
            grid[6, 7].BackgroundImage = images["wn"];
            grid[2, 7].BackgroundImage = images["wb"];
            grid[5, 7].BackgroundImage = images["wb"];
            grid[3, 7].BackgroundImage = images["wq"];
            grid[4, 7].BackgroundImage = images["wk"];

        }
            
            public void squareClick(object sender, EventArgs e) // Click event for all squares
            {
                Button button = (Button)sender; // Cast sender to button type
                Point coords = getCoords(button); // Get the coordinates of the clicked button

                
                // If no piece selected, sets the selected piece to the one in the clicked square and finds possible moves
                if (!pieceSelected & pieceGrid[coords.X, coords.Y] != null) 
                {
                    selectedPiece = pieceGrid[coords.X, coords.Y]; // Selects the piece at the clicked square
                    selectedCoords = coords; // Stores the location of the selected piece for later
                    pieceSelected = true;
                    selectedPieceTeam = selectedPiece.team;
                    selectedPieceTeamChar = selectedPieceTeam[0].ToString(); // Gives the first letter of the colour of the piece
                    selectedPieceType = selectedPiece.GetType().ToString();

                    // Add a border around the selected piece
                    button.FlatAppearance.BorderColor = Color.Black;
                    button.FlatAppearance.BorderSize = 1;

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

                    moveGrid = pieceGrid[selectedCoords.X, selectedCoords.Y].moveRules(selectedCoords);

                }
                // If the selected piece is clicked again, deselect it
                else if (coords == selectedCoords)
                {
                    selectedPiece.hasMoved = false;
                    pieceSelected = false;
                    selectedPiece = null;
                    
                    button.FlatAppearance.BorderSize = 0;
                }

            // If a move is legal, moves the piece to the new square and clears the old square
            else if (pieceSelected & moveGrid[coords.X, coords.Y] == 1)  
                {
                    grid[selectedCoords.X, selectedCoords.Y].BackgroundImage = null; // Set old location's image to null
                    grid[selectedCoords.X, selectedCoords.Y].FlatAppearance.BorderSize = 0;

                    if (selectedPieceTeam == "white") //Places the correct colour piece based on selected piece colour
                        grid[coords.X, coords.Y].BackgroundImage = images[selectedPieceImageCode];
                    else
                        grid[coords.X, coords.Y].BackgroundImage = images[selectedPieceImageCode];

                    // Set new location's image to correct image and reset original location's image to null
                    pieceGrid[coords.X, coords.Y] = selectedPiece; 
                    pieceGrid[selectedCoords.X, selectedCoords.Y] = null; 
                    pieceSelected = false;
                }

                
            }

            // Divides the x and y coordinate by 80 to find the location within the board
            public Point getCoords(Button button)
            {
                int x = button.Location.X / 80; 
                int y = button.Location.Y / 80;
                return new Point(x, y);
            }
        }
}