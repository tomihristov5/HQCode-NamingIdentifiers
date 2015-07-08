namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Minesweeper
    {
        private const int MAX_FIELDS_WITHOUT_MINES = 35;

        public static void Main()
        {
            int pointsCount = 0;
            int row = 0;
            int col = 0;
            string command = string.Empty;
            char[,] gameField = CreateGameField();
            char[,] mines = PositionMines();
            bool steppedOnAMine = false;
            bool startedGame = true;
            bool endedGame = false;
            List<ScoreInfo> highscore = new List<ScoreInfo>(6);

            do
            {
                if (startedGame)
                {
                    Console.WriteLine("Let's play \"Minesweeper\". Try your luck not to step on fields with a mine." +
                    " Command 'top' shows the highscores, 'restart' starts a new game, 'exit' quits the game!");
                    PrintGameBoard(gameField);
                    startedGame = false;
                }
                Console.Write("Enter row and column : ");
                command = Console.ReadLine().Trim();
                if (command.Length >= 3)
                {
                    if (int.TryParse(command[0].ToString(), out row) &&
                    int.TryParse(command[2].ToString(), out col) &&
                        row <= gameField.GetLength(0) && col <= gameField.GetLength(1))
                    {
                        command = "turn";
                    }
                }
                switch (command)
                {
                    case "top":
                        Highscore(highscore);
                        break;
                    case "restart":
                        gameField = CreateGameField();
                        mines = PositionMines();
                        PrintGameBoard(gameField);
                        steppedOnAMine = false;
                        startedGame = false;
                        break;
                    case "exit":
                        Console.WriteLine("Bye bye!");
                        break;
                    case "turn":
                        if (mines[row, col] != '*')
                        {
                            if (mines[row, col] == '-')
                            {
                                GetFieldValue(gameField, mines, row, col);
                                pointsCount++;
                            }
                            if (MAX_FIELDS_WITHOUT_MINES == pointsCount)
                            {
                                endedGame = true;
                            }
                            else
                            {
                                PrintGameBoard(gameField);
                            }
                        }
                        else
                        {
                            steppedOnAMine = true;
                        }
                        break;
                    default:
                        Console.WriteLine("\nIvalid command!\n");
                        break;
                }
                if (steppedOnAMine)
                {
                    PrintGameBoard(mines);
                    Console.Write("\nSorry! You died with {0} points. " +
                        "Enter your nickname: ", pointsCount);
                    string nickName = Console.ReadLine();
                    ScoreInfo playerScore = new ScoreInfo(nickName, pointsCount);
                    if (highscore.Count < 5)
                    {
                        highscore.Add(playerScore);
                    }
                    else
                    {
                        for (int i = 0; i < highscore.Count; i++)
                        {
                            if (highscore[i].PlayerPoints < playerScore.PlayerPoints)
                            {
                                highscore.Insert(i, playerScore);
                                highscore.RemoveAt(highscore.Count - 1);
                                break;
                            }
                        }
                    }
                    highscore.Sort((ScoreInfo r1, ScoreInfo r2) => r2.PlayerName.CompareTo(r1.PlayerName));
                    highscore.Sort((ScoreInfo r1, ScoreInfo r2) => r2.PlayerPoints.CompareTo(r1.PlayerPoints));
                    Highscore(highscore);

                    gameField = CreateGameField();
                    mines = PositionMines();
                    pointsCount = 0;
                    steppedOnAMine = false;
                    startedGame = true;
                }
                if (endedGame)
                {
                    Console.WriteLine("\nCongrats! You opened all 35 fields unharmed.");
                    PrintGameBoard(mines);
                    Console.WriteLine("Please, enter your nickname: ");
                    string nickName = Console.ReadLine();
                    ScoreInfo playerScore = new ScoreInfo(nickName, pointsCount);
                    highscore.Add(playerScore);
                    Highscore(highscore);
                    gameField = CreateGameField();
                    mines = PositionMines();
                    pointsCount = 0;
                    endedGame = false;
                    startedGame = true;
                }
            }
            while (command != "exit");
            Console.WriteLine("Made in Bulgaria");
            Console.WriteLine("Bye bye. Press any key to exit.");
            Console.Read();
        }

        private static void Highscore(List<ScoreInfo> highscores)
        {
            Console.WriteLine("\nPoints:");
            if (highscores.Count > 0)
            {
                for (int i = 0; i < highscores.Count; i++)
                {
                    Console.WriteLine("{0}. {1} --> {2} fields open",
                        i + 1, highscores[i].PlayerName, highscores[i].PlayerPoints);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Empty highscores!\n");
            }
        }

        private static void GetFieldValue(char[,] gameField, char[,] mines, int row, int col)
        {
            char minesCount = CountMines(mines, row, col);
            mines[row, col] = minesCount;
            gameField[row, col] = minesCount;
        }

        private static void PrintGameBoard(char[,] gameBoard)
        {
            int rows = gameBoard.GetLength(0);
            int cols = gameBoard.GetLength(1);
            Console.WriteLine("\n    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");
            for (int row = 0; row < rows; row++)
            {
                Console.Write("{0} | ", row);
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(string.Format("{0} ", gameBoard[row, col]));
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine("   ---------------------\n");
        }

        private static char[,] CreateGameField()
        {
            int rows = 5;
            int cols = 10;
            char[,] board = new char[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    board[row, col] = '?';
                }
            }

            return board;
        }

        private static char[,] PositionMines()
        {
            int rows = 5;
            int cols = 10;
            char[,] gameField = new char[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    gameField[row, col] = '-';
                }
            }

            List<int> mines = new List<int>();
            while (mines.Count < 15)
            {
                Random random = new Random();
                int randomNumber = random.Next(50);
                if (!mines.Contains(randomNumber))
                {
                    mines.Add(randomNumber);
                }
            }

            foreach (int mine in mines)
            {
                int row = (mine / cols);
                int col = (mine % cols);
                if (col == 0 && mine != 0)
                {
                    row--;
                    col = cols;
                }
                else
                {
                    col++;
                }
                gameField[row, col - 1] = '*';
            }

            return gameField;
        }

        private static void CalculateGameFieldValues(char[,] gameField)
        {
            int rows = gameField.GetLength(0);
            int cols = gameField.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (gameField[row, col] != '*')
                    {
                        char minesCount = CountMines(gameField, row, col);
                        gameField[row, col] = minesCount;
                    }
                }
            }
        }

        private static char CountMines(char[,] gameField, int cols, int col)
        {
            int count = 0;
            int reds = gameField.GetLength(0);
            int rowss = gameField.GetLength(1);

            if (cols - 1 >= 0)
            {
                if (gameField[cols - 1, col] == '*')
                {
                    count++;
                }
            }
            if (cols + 1 < reds)
            {
                if (gameField[cols + 1, col] == '*')
                {
                    count++;
                }
            }
            if (col - 1 >= 0)
            {
                if (gameField[cols, col - 1] == '*')
                {
                    count++;
                }
            }
            if (col + 1 < rowss)
            {
                if (gameField[cols, col + 1] == '*')
                {
                    count++;
                }
            }
            if ((cols - 1 >= 0) && (col - 1 >= 0))
            {
                if (gameField[cols - 1, col - 1] == '*')
                {
                    count++;
                }
            }
            if ((cols - 1 >= 0) && (col + 1 < rowss))
            {
                if (gameField[cols - 1, col + 1] == '*')
                {
                    count++;
                }
            }
            if ((cols + 1 < reds) && (col - 1 >= 0))
            {
                if (gameField[cols + 1, col - 1] == '*')
                {
                    count++;
                }
            }
            if ((cols + 1 < reds) && (col + 1 < rowss))
            {
                if (gameField[cols + 1, col + 1] == '*')
                {
                    count++;
                }
            }

            return char.Parse(count.ToString());
        }
    }
}
