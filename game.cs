using System;
using System.Timers;

// i  -  toghery
// j  -  syunery

namespace Benivo
{
    class game
    {
        #region variables & functions

        static Random random = new Random();
        static Timer timer = new Timer();   

        static int boardWidth = 30;
        static int boardHeight = 20;
        static int startLeft = boardWidth / 2;
        static int startTop = boardHeight / 2;
        static int currentLeft;
        static int currentTop;
        static int startRemainingTime = 60;
        static int obstacleCount = 10;
        static int scoreToWin = 3;
        static int[,] board;
        static int score;
        static int remainingTime;    
        static bool isGameRunning = true;
        static bool isGameOver = false;

        static void Update()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }

            var input = Console.ReadKey();

            //board[currentTop, currentLeft] = 0;

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentTop > 0 && board[currentTop - 1, currentLeft] == 3)
                    {
                        PlayEatingSound();
                        score++;

                        board[random.Next(0, boardHeight), random.Next(0, boardWidth)] = 3;
                    }

                    if (currentTop > 0
                       && board[currentTop - 1, currentLeft] != 2)
                    {
                        board[currentTop, currentLeft] = 0;
                        currentTop--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (currentTop < boardHeight - 1
                         && board[currentTop + 1, currentLeft] == 3)
                    {
                        PlayEatingSound();
                        score++;

                        board[random.Next(0, boardHeight), random.Next(0, boardWidth)] = 3;
                    }
                    if (currentTop < boardHeight - 1
                         && board[currentTop + 1, currentLeft] != 2)
                    {
                        board[currentTop, currentLeft] = 0;
                        currentTop++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentLeft > 0
                         && board[currentTop, currentLeft - 1] == 3)
                    {
                        PlayEatingSound();
                        score++;

                        board[random.Next(0, boardHeight), random.Next(0, boardWidth)] = 3;
                    }
                    if (currentLeft > 0
                         && board[currentTop, currentLeft - 1] != 2)
                    {
                        board[currentTop, currentLeft] = 0;
                        currentLeft--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (currentLeft < boardWidth - 1
                         && board[currentTop, currentLeft + 1] == 3)
                    {
                        PlayEatingSound();
                        score++;

                        board[random.Next(0, boardHeight), random.Next(0, boardWidth)] = 3;
                    }
                    if (currentLeft < boardWidth - 1
                         && board[currentTop, currentLeft + 1] != 2)
                    {
                        board[currentTop, currentLeft] = 0;
                        currentLeft++;
                    }
                    break;

                default:
                    break;
            }

            board[currentTop, currentLeft] = 1;

            // draw()
        }

        static void Draw()
        {
            DrawScore();
            DrawBoard(board);
            DrawTime();
        }

        static void DrawXY(int left, int top, char c)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(c);

        }

        static void DrawXY(int left, int top, string s)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(s);
        }

        static void DrawBorder(int boardWidth, int boardHeight)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < boardWidth + 2; i++)
            {
                DrawXY(i, 0, '*');
                DrawXY(i, boardHeight + 1, '*');

            }
            for (int i = 0; i < boardHeight; i++)
            {
                DrawXY(0, i + 1, '*');
                DrawXY(boardWidth + 1, i + 1, '*');

            }
            Console.ResetColor();
        }

        static void DrawBoard(int[,] board)
        {
            int boardHeight = board.GetLength(0);
            int boardWidth = board.GetLength(1);

            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    char symbol = GetSymbol(board[i, j]);

                    if (symbol == 'P')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (symbol == 'B') 
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    } 
                    else if (symbol ==  'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                   
                    DrawXY(j + 1, i + 1, symbol);

                    Console.ResetColor();
                }
            }

            Console.SetCursorPosition(1, 1);

        }

        static void DrawScore()
        {
            DrawXY(boardWidth + 10, 0, $"   ");
            DrawXY(boardWidth + 3, 0, $"Score: {score}");
        }

        static void DrawTime()
        {
            DrawXY(boardWidth + 9, 2, $"   ");
            DrawXY(boardWidth + 3, 2, $"Time: {remainingTime}");
        }

        static char GetSymbol(int number)
        {
            switch (number)
            {
                case 0:
                    return ' ';
                case 1:
                    return 'P';
                case 2:
                    return 'O';
                case 3:
                    return 'B';
                case 4:
                    return 'E';
                default:
                    return ' ';

                    // 0 - empty
                    // 1 - player
                    // 2 - obstacle
                    // 3 - bonus
                    // 4 - enemy
            }
        }

        static void PrepareNewGame()
        {
            board = new int[boardHeight, boardWidth];
            score = 0;
            remainingTime = startRemainingTime;
            currentLeft = startLeft;
            currentTop = startTop;  


            Console.CursorVisible = false;
            DrawBorder(boardWidth, boardHeight);

            // obstacles
            for (int i = 0; i < obstacleCount; i++)
            {
                int left = random.Next(0, boardWidth);
                int top = random.Next(0, boardHeight);

                board[top, left] = 2;
            }

            //bonus
            board[random.Next(0, boardHeight), random.Next(0, boardWidth)] = 3;


            // player
            board[currentTop, currentLeft] = 1;

        }        

        static void PlayEatingSound()
        {           
            Console.Beep(300, 200);
            Console.Beep(600, 200);
        }

        static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isGameRunning)
            {
                return;
            }

            if (remainingTime > 0)
            {
                remainingTime--;
            }
            else
            {
                isGameRunning = false;
              
                if (score >= scoreToWin)
                {                    
                    Console.Beep(500, 300);
                    Console.Beep(700, 300);
                    Console.Beep(1000, 500);

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    DrawXY(0, 0, "You won!");
                    Console.ResetColor();
                }
                else
                {                   
                    Console.Beep(1000, 300);
                    Console.Beep(1000, 300);
                    Console.Beep(1000, 300);

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    DrawXY(0, 0, "You lost!");
                    Console.ResetColor();
                }

                DrawXY(0, 2, "Restart a new game? Y/N");

                var input = Console.ReadKey();

                if(input.Key == ConsoleKey.Y)
                {
                    // new game
                    PrepareNewGame();
                    isGameRunning = true;
                }
                else
                {
                    // end game
                    isGameOver = true;
                }
            }

        }
        
        #endregion

        static void Main()
        {
            PrepareNewGame();
           
            // timer
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();

            // game loop
            do
            {
                if (isGameRunning)
                {
                    Draw();
                    Update();
                }
            } while (!isGameOver);

            Console.Clear();
        }
               
    }
}

