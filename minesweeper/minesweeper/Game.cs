using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    /// <summary>
    /// This class contains all logic of the game
    /// </summary>
    
    public class Game
    {
        int Rows;
        int Columns;
        int Mines;
        int correctlyMarkedMines; // Flags that they are marked correctly
        int markedFields; //Flags entered by user
        bool gameEnded = false;
        Field[,] Board;
        public List<Point> MinesCoordinates; // Coordinates where they are all mines; 

        /// <summary>
        /// Show the menu to the user and validate the input
        /// </summary>
        public void StartGame()
        {
            string selectedOption = "";
            while (selectedOption != "2")
            {
                showMenu();
                selectedOption = Console.ReadLine();

                switch (selectedOption)
                {
                    case "1":
                        newGame();
                        break;
                    case "2":
                        Console.WriteLine("See you later. Key enter to exit...");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("selected option invalid");
                        break;
                }
            }
        }

        /// <summary>
        /// Receive and validate the size of the board
        /// </summary>
        public void newGame()
        {
            bool isValid = false;
            gameEnded = false;
            MinesCoordinates= new List<Point>();

            while (!isValid)
            {

                Console.WriteLine("Please enter the dimensions: rows, columns and mines separated by a space Eg. 10 10 15");
                string dimentions = Console.ReadLine();
                string[] dimentionsValues = dimentions.Split(' ');

                // Validate if they are three elements and if they are int
                if (dimentionsValues.Length == 3)
                {
                    if (int.TryParse(dimentionsValues[0], out Rows) && Rows > 0)
                    {
                        if (int.TryParse(dimentionsValues[1], out Columns) && Columns > 0)
                        {
                            if (int.TryParse(dimentionsValues[2], out Mines) && Mines > 0 && (Rows * Columns) > Mines)
                            {
                                isValid = true;
                                correctlyMarkedMines = 0;
                                markedFields = 0;
                            }
                        }
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine("Incorrect Input, try again");
                }
            }

            Console.Clear();
            buildBoard();
            printBoard();
            readInput();

        }

        /// <summary>
        /// Menu content
        /// </summary>
        public void showMenu()
        {
            Console.SetWindowSize(120, 30);
            Console.WriteLine("::::::::::::Welcome to Minesweeper::::::::::::");
            Console.WriteLine();
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. New game");
            Console.WriteLine("2. exit");
        }

        /// <summary>
        /// Building the board with mines and other elements
        /// </summary>
        public void buildBoard()
        {
            // List that contains all possible cordinates
            List<Point> pointsGenerated = new List<Point>();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    pointsGenerated.Add(new Point(i, j));
                }
            }

            //Building of the board
            Board = new Field[Rows, Columns];
            Random r = new Random();

            //Entering the mines to the board
            for (int i = 0; i < Mines; i++)
            {
                Point p = GetRandomPoint(r, pointsGenerated);
                Board[p.X, p.Y] = new Field();
                Board[p.X, p.Y].type = Type.mine;
                Board[p.X, p.Y].visible = false;
                Board[p.X, p.Y].marked = false;
            }

            //Entering the rest of fields to the board
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Board[i, j] is null)
                    {
                        initializeField(i, j);
                    }
                }
            }
        }

        /// <summary>
        /// This method evaluates what can go in each field depending if there is a mine or not
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        public void initializeField(int x, int y)
        {
            int mines = 0;

            if (x > 0)
            {
                if (y > 0)
                {
                    if (Board[x - 1, y - 1] != null) // Top left
                    {
                        if (Board[x - 1, y - 1].type.Equals(minesweeper.Type.mine))
                        {
                            mines++;
                        }
                    }
                }

                if (Board[x - 1, y] != null) // Top
                {
                    if (Board[x - 1, y].type.Equals(minesweeper.Type.mine))
                    {
                        mines++;
                    }
                }

                if (y < Columns - 1)
                {
                    if (Board[x - 1, y + 1] != null) // Top Right
                    {
                        if (Board[x - 1, y + 1].type.Equals(minesweeper.Type.mine))
                        {
                            mines++;
                        }
                    }
                }
            }

            if (y > 0)
            {
                if (Board[x, y - 1] != null) //Left
                {
                    if (Board[x, y - 1].type.Equals(minesweeper.Type.mine))
                    {
                        mines++;
                    }
                }
            }

            if (y < Columns - 1)
            {
                if (Board[x, y + 1] != null) // Right
                {
                    if (Board[x, y + 1].type.Equals(minesweeper.Type.mine))
                    {
                        mines++;
                    }
                }
            }

            if (x < Rows - 1)
            {
                if (y > 0)
                {
                    if (Board[x + 1, y - 1] != null) //Down left
                    {
                        if (Board[x + 1, y - 1].type.Equals(minesweeper.Type.mine))
                        {
                            mines++;
                        }
                    }
                }

                if (Board[x + 1, y] != null) // Down
                {
                    if (Board[x + 1, y].type.Equals(minesweeper.Type.mine))
                    {
                        mines++;
                    }
                }

                if (y < Columns - 1)
                {
                    if (Board[x + 1, y + 1] != null)// Down Right
                    {
                        if (Board[x + 1, y + 1].type.Equals(minesweeper.Type.mine))
                        {
                            mines++;
                        }
                    }
                }
            }

            Board[x, y] = new Field();
            Board[x, y].visible = false;
            Board[x, y].marked = false;

            if (mines > 0) // if there is one or more adjacent mines it shows the number
            {
                Board[x, y].type = Type.number;
                Board[x, y].value = mines;
            }
            else
            {
                Board[x, y].type = Type.empty;
            }
        }

        /// <summary>
        /// Receive and validate the user's movement
        /// </summary>
        public void readInput()
        {

            bool isValid = false;
            int selectedRow = 0;
            int selectedColumn = 0;
            string selectedAction = "";

            while (!isValid)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the index of a row, column and action for the specific cell.  U: uncover, M: mark  Eg. 5 5 U");
                string action = Console.ReadLine();
                string[] actionValues = action.Split(' ');

                // Validate if they are three elements
                if (actionValues.Length == 3)
                {
                    selectedAction = actionValues[2].ToUpper();

                    if (int.TryParse(actionValues[0], out selectedRow) && selectedRow > 0 && selectedRow <= Rows)
                    {
                        if (int.TryParse(actionValues[1], out selectedColumn) && selectedColumn > 0 && selectedColumn <= Columns)
                        {
                            if (selectedAction.Equals("U") || selectedAction.Equals("M")) // Validate the action
                            {
                                isValid = true;
                                executeInput(selectedRow, selectedColumn, selectedAction);
                            }
                        }
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine("Incorrect Input, try again");
                }
            }
        }

        /// <summary>
        /// Execute the user's movement and it validates if the game has finished
        /// </summary>
        /// <param name="row">Coordinate X</param>
        /// <param name="column">Coordinate Y</param>
        /// <param name="action">Action</param>
        public void executeInput(int row, int column, string action)
        {
            // validate that the field is not visible
            if (action.Equals("U") && !Board[row - 1, column - 1].visible) 
            {
                // if it was marked, it is unmarked
                if (Board[row - 1, column - 1].marked) 
                {
                    Board[row - 1, column - 1].marked = false;
                    markedFields--;
                }

                Board[row - 1, column - 1].visible = true;
            }
            // validate that the field is not visible
            else if (action.Equals("M") && !Board[row - 1, column - 1].visible) 
            {
                Board[row - 1, column - 1].marked = true;
                markedFields++;
            }

            //if the action is to uncover and there is a mine
            if (Board[row - 1, column - 1].type.Equals(minesweeper.Type.mine)
                        && Board[row - 1, column - 1].visible)
            {
                gameEnded = true;
                Console.WriteLine(":::::::::::: Game over ::::::::::::");
                showHiddenMines();
            }
            
            //validate if a field with a mine was marked
            if (Board[row - 1, column - 1].type.Equals(minesweeper.Type.mine)
                        && Board[row - 1, column - 1].marked)
            {
                correctlyMarkedMines++;
            }

            //If a empty field is uncovered it will show its adjacent fields
            if (Board[row - 1, column - 1].type.Equals(minesweeper.Type.empty)
                        && Board[row - 1, column - 1].visible)
            {
                showAdjacent(row - 1, column - 1);
            }

            while (!gameEnded)
            {
                Console.Clear();
                printBoard();

                if (correctlyMarkedMines == Mines && markedFields == correctlyMarkedMines) {
                    Console.WriteLine(" Congratulations, You won " );
                    gameEnded = true;
                    break;
                }
                
                readInput();
            }


        }

        /// <summary>
        /// Show all hidden mines when the player loses
        /// </summary>
        public void showHiddenMines()
        {
            foreach (Point p in MinesCoordinates)
            {

                if (Board[p.X, p.Y].marked)
                {
                    Board[p.X, p.Y].marked = false;
                }

                Board[p.X, p.Y].visible = true;
            }

            printBoard();
        }

        /// <summary>
        /// Show adjacent fields when a empty field is uncovered
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        public void showAdjacent(int x, int y)
        {

            if (x > 0)
            {
                if (y > 0)
                {
                    if (!Board[x - 1, y - 1].type.Equals(minesweeper.Type.mine)) // Top left
                    {
                        Board[x - 1, y - 1].visible = true;
                    }
                }

                if (!Board[x - 1, y].type.Equals(minesweeper.Type.mine)) // Top
                {
                    Board[x - 1, y - 1].visible = true;
                }

                if (y < Columns - 1)
                {
                    if (!Board[x - 1, y + 1].type.Equals(minesweeper.Type.mine)) // Top Right
                    {
                        Board[x - 1, y + 1].visible = true;
                    }
                }
            }

            if (y > 0)
            {
                if (!Board[x, y - 1].type.Equals(minesweeper.Type.mine)) //Left
                {
                    Board[x, y - 1].visible = true;
                }
            }

            if (y < Columns - 1)
            {
                if (!Board[x, y + 1].type.Equals(minesweeper.Type.mine)) // Right
                {
                    Board[x, y + 1].visible = true;
                }
            }

            if (x < Rows - 1)
            {
                if (y > 0)
                {
                    if (!Board[x + 1, y - 1].type.Equals(minesweeper.Type.mine)) //Down left
                    {
                        Board[x + 1, y - 1].visible = true;
                    }
                }

                if (!Board[x + 1, y].type.Equals(minesweeper.Type.mine)) // Down
                {
                    Board[x + 1, y].visible = true;
                }

                if (y < Columns - 1)
                {
                    if (!Board[x + 1, y + 1].type.Equals(minesweeper.Type.mine))// Down Right
                    {
                        Board[x + 1, y + 1].visible = true;
                    }
                }
            }
        }

        /// <summary>
        ///  Show the board to the user
        /// </summary>
        public void printBoard()
        {
            Console.WriteLine();

            for (int i = 0; i < Rows; i++)
            {
                string row = "";
                for (int j = 0; j < Columns; j++)
                {
                    string view = "";
                    if (!Board[i, j].visible && !Board[i, j].marked)
                    {
                        view = ".";
                    }
                    else if (Board[i, j].marked)
                    {
                        view = "P";
                    }
                    else
                    {
                        switch (Board[i, j].type)
                        {
                            case Type.empty:
                                view = "-";
                                break;
                            case Type.number:
                                view = Board[i, j].value.ToString();
                                break;
                            case Type.mine:
                                view = "*";
                                break;
                        }
                    }
                    if (row.Equals(string.Empty))
                    {
                        row = view;
                    }
                    else
                    {
                        row = string.Format("{0} {1}", row, view); ;
                    }
                }
                Console.WriteLine(row);
            }
        }
        
        /// <summary>
        /// Generate a coordinate to save a mine
        /// </summary>
        /// <param name="r">Random element</param>
        /// <param name="pointsGenerated">List of all possible coordinates</param>
        /// <returns></returns>
        public Point GetRandomPoint(Random r, List<Point> pointsGenerated)
        {

            int pos = r.Next(0, pointsGenerated.Count - 1);
            Point p = pointsGenerated[pos];
            MinesCoordinates.Add(p);
            pointsGenerated.RemoveAt(pos);

            return p;
        }

    }

}
