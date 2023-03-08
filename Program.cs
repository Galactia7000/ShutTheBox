using System;


namespace Shut_the_box
{
    internal class Program
    {
        const int players = 3;
        const int startLives = 3;

        static bool[,] playerBoards = new bool[players, 13]; // position one contains if they are out
        static int[] playerLives = new int[players];
        static void Main(string[] args)
        {
            bool gameWon = false;
            int activePlayer = 0;
            int playersLeft = players;
            SetupGame();

            while (!gameWon)
            {
                DisplayBox(activePlayer);

                Console.WriteLine("Player {0}'s turn, press any key to roll the dice.", activePlayer + 1); // Dice rolling
                Console.ReadKey();
                int total = RollDice();
                Console.WriteLine("You rolled a " + total);

                bool lose = CheckLoss(total, activePlayer); // Checking losing condition
                if (lose)
                {
                    playerLives[activePlayer]--;
                    Console.WriteLine("Player {0} has lost a life, they now have {1} lives left.", activePlayer + 1, playerLives[activePlayer]);
                    if (playerLives[activePlayer] == 0)
                    {
                        playersLeft--;
                        Console.WriteLine("Player {0} has no more lives. There is {1} player(s) left.", activePlayer + 1, playersLeft);
                        if (playersLeft == 1) // All but one player eliminated
                        {
                            CyclePlayer(ref activePlayer);
                            Console.WriteLine("Player {0} is the last player standing and therefore has one the game, congratulations!", activePlayer + 1);
                            gameWon = true;
                            break;
                        }
                        CyclePlayer(ref activePlayer);
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                } else
                {
                    playerBoards[activePlayer, total] = true;
                    Console.WriteLine("Player {0} was able to shut the lock on number {1}.", activePlayer + 1, total);
                }

                DisplayBox(activePlayer);

                bool win = CheckWin(activePlayer); // Checking winning condition
                if (win)
                {
                    Console.WriteLine("Player {0} has shut the final lock and has won the game, congratulations!", activePlayer + 1);
                    gameWon = true;
                } else // Setting up for next player
                {
                    CyclePlayer(ref activePlayer);
                }
                Console.ReadKey();
                Console.Clear();
            }
            Console.ReadKey();
        }

        static void SetupGame()
        {
            for (int i = 0; i < playerBoards.GetLength(0); i++)
            {
                for(int j = 0; j < playerBoards.GetLength(1); j++)
                {
                    playerBoards[i, j] = false;
                }
            }
            for (int i = 0; i < playerLives.Length; i++)
            {
                playerLives[i] = startLives;
            }
        }

        static int RollDice()
        {
            Random rng = new Random();
            return rng.Next(1, 7) + rng.Next(1, 7);
        }

        static void DisplayBox(int player)
        {
            Console.WriteLine("This is player {0}'s box:", player + 1);
            for(int i = 2; i < playerBoards.GetLength(1); i++)
            {
                Console.Write(i + ":    ");
                if (playerBoards[player, i]) Console.Write("SHUT");
                else Console.Write("OPEN");
                Console.WriteLine();
            }
        }

        static void CyclePlayer(ref int activePlayer)
        {
            bool valid = false;
            while (!valid)
            {
                activePlayer++;
                if (activePlayer == players) activePlayer = 0;
                if (playerLives[activePlayer] > 0) valid = true;
            }
        }

        static bool CheckWin(int player)
        {
            for (int i = 2; i < playerBoards.GetLength(1); i++) 
            {
                if (!playerBoards[player, i]) return false;
            }
            return true;
        }

        static bool CheckLoss(int index, int player)
        {
            if (playerBoards[player, index]) return true;
            else return false;
        }
    }
}
