using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTutorials
{

    public class Position
    {
        public int P1position { get; set; }
        public int p2position { get; set; }

        public Position(int a, int b)
        {
            P1position = a;
            p2position = b;
        }
    }

    public class Player
    {
        public string Name { get; }
        public Position Position { get; set; }
        public int CountGem { get; set; }
        private Game game;

        public Player(string name, Position position, Game game)
        {
            Name = name;
            Position = position;
            CountGem = 0;
            this.game = game;
        }

        public void Movement(char direction)
        {
            int newX = Position.P1position;
            int newY = Position.p2position;

            switch (direction)
            {
                case 'U':
                    newX--;
                    break;
                case 'D':
                    newX++;
                    break;
                case 'L':
                    newY--;
                    break;
                case 'R':
                    newY++;
                    break;
                default:
                    Console.WriteLine("Enter valid direction.");
                    return; // Exit the method if the direction is invalid
            }

            // Check if the new position is within the bounds of the board
            if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
            {
                Console.WriteLine("Cannot move outside the board.");
                return; // Exit the method if the move is outside the board
            }

            // Check if the new position contains an obstacle
            if (game.board.Grid[newX, newY].cellHolder == "O")
            {
                Console.WriteLine("Cannot move into an obstacle.");
                return; // Exit the method if the move is into an obstacle
            }

            // Update the player's position
            Position.P1position = newX;
            Position.p2position = newY;

            // Update the grid with the new player position
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            game.board.Grid[Position.P1position, Position.p2position].cellHolder = Name;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (game.board.Grid[i, j].cellHolder == Name && (i != Position.P1position || j != Position.p2position))
                    {
                        game.board.Grid[i, j].cellHolder = "-";
                        return;
                    }
                }
            }
        }
    }

    public class Cell
    {
        public string cellHolder { get; set; }
    }

    public class Board
    {
        public Cell[,] Grid { get; }

        public Board()
        {
            Grid = new Cell[6, 6];
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize the game with all empty cells
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Grid[i, j] = new Cell { cellHolder = "-" };
                }
            }

            // Place players
            Grid[0, 0].cellHolder = "P1";
            Grid[5, 5].cellHolder = "P2";

            // Place gems (randomly for demonstration)
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                int a = random.Next(6);
                int b = random.Next(6);
                if (Grid[a, b].cellHolder == "-")
                {
                    Grid[a, b].cellHolder = "G";
                }
            }

            // Place obstacles (randomly for demonstration)
            for (int i = 0; i < 6; i++)
            {
                int a = random.Next(6);
                int b = random.Next(6);
                if (Grid[a, b].cellHolder == "-")
                {
                    Grid[a, b].cellHolder = "O";
                }
            }
        }

        public void DisplayBoard()
        {
            Console.WriteLine("\n");
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(Grid[i, j].cellHolder + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }

        public bool ValidMove(Player player, char direction)
        {
            int newX = player.Position.P1position;
            int newY = player.Position.p2position;

            switch (direction)
            {
                case 'U':
                    newX--;
                    break;
                case 'D':
                    newX++;
                    break;
                case 'L':
                    newY--;
                    break;
                case 'R':
                    newY++;
                    break;
                default:
                    return false;
            }

            if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
                return false;

            if (Grid[newX, newY].cellHolder == "O")
            {
                return false;
            }

            return true;
        }

        public void GetGem(Player player, char direction)
        {
            int newX = player.Position.P1position;
            int newY = player.Position.p2position;

            switch (direction)
            {
                case 'U':
                    newX--;
                    break;
                case 'D':
                    newX++;
                    break;
                case 'L':
                    newY--;
                    break;
                case 'R':
                    newY++;
                    break;
                default:
                    Console.WriteLine("Enter valid direction");
                    return; // Exit the method if the direction is invalid
            }

            // Check if the new position is within the bounds of the board
            if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
            {
                Console.WriteLine("player cannot move outside the board.");
                return; // Exit the method if the move is outside the board
            }

            Cell playercurrentCell = Grid[newX, newY];

            if (playercurrentCell.cellHolder == "G")
            {
                player.CountGem++;
                playercurrentCell.cellHolder = "-"; // Remove the collected gem from the board
                Console.WriteLine($"collected gem by {player.Name} is: {player.CountGem}" );
            }
            else if (playercurrentCell.cellHolder == "O")
            {
                // If the current cell contains an obstacle, inform the player
                Console.WriteLine("Player cannot collect gem. Obstacle in the way.");
            }
            
        }
    }

    public class Game
    {
        public Board board; // Change to public
        public Player p1;
        public Player p2;
        public Player currentTurn;
        private int totalTurns;

        public Game()
        {
            board = new Board();
            p1 = new Player("P1", new Position(0, 0), this); // Pass reference to the game
            p2 = new Player("P2", new Position(5, 5), this); // Pass reference to the game
            currentTurn = p1;
            totalTurns = 0;
        }

        public void Begin()
        {
            while (!checkGameOver())
            {
                Console.WriteLine($"Turn : {totalTurns + 1}");
                board.DisplayBoard();
                Console.WriteLine($"Current Player is : {currentTurn.Name}");
                Console.Write("Enter direction (U/D/L/R): ");
                char direction = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine("\n");

                if (board.ValidMove(currentTurn, direction))
                {
                    board.GetGem(currentTurn, direction);
                    currentTurn.Movement(direction);
                    totalTurns++;
                    ShiftTurn(); // Switch the turn only after a valid move
                }
                else
                {
                    Console.WriteLine("Invalid move!");
                }
            }

            Winner();
        }
        private void ShiftTurn()
        {
            if (currentTurn == p1)
                currentTurn = p2;
            else
                currentTurn = p1;
        }

        private bool checkGameOver()
        {
            // Count the remaining gems on the board
            int remainingGems = 0;
            foreach (Cell cell in board.Grid)
            {
                if (cell.cellHolder == "G")
                {
                    remainingGems++;
                }
            }

            // Game ends if either player has collected all the gems or there are no more gems left
            return totalTurns >= 30 || remainingGems == 0;
        }

        private void Winner()
        {
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Gems collected by Player 1 is : {p1.CountGem}");
            Console.WriteLine($"Gems collected by Player 2 is : {p2.CountGem}");

            if (p1.CountGem > p2.CountGem)
            {
                Console.WriteLine("Player 1 wins!");
            }
            else if (p2.CountGem > p1.CountGem)
            {
                Console.WriteLine("Player 2 wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }
    }

    public class Program
    {
        public static Game game;

        static void Main(string[] args)
        {
            game = new Game();
            Player player1 = new Player("P1", new Position(0, 0), game);
            Player player2 = new Player("P2", new Position(5, 5), game);
            game.Begin();
        }
    }
}