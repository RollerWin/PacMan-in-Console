using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PacMan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            char[,] map;
            Random randomDirection = new Random();
            bool isPlay = true;
            char wall = '#';

            bool isAlive = true;
            int playerX, playerY;
            int playerDirectX = 0, playerDirectY = 1;
            char player = '@';

            int enemyX, enemyY;
            int enemyDirectX = 0, enemyDirectY = 1;
            char enemy = '0';

            int collectedDots = 0, allDots;

            map = ReadMap("map1", out playerX, out playerY, out enemyX, out enemyY, out allDots);

            DisplayMap(map);

            Console.SetCursorPosition(playerX, playerY);
            Console.Write(player);

            Console.SetCursorPosition(0, map.GetLength(0) + 3);
            Console.WriteLine("Управление:\nrightArrow - направо\nleftArrow - налево\nupArrow - вверх\ndownArrow - вниз\n\nВы - @. Чудовище - 0.\nВаша цель - собрать все монетки и не быть съеденными чудовищем! Удачи!");

            while (isPlay)
            {

                if (Console.KeyAvailable)
                {
                    PlayerControl(ref playerDirectX, ref playerDirectY);
                }

                if (map[playerY + playerDirectY, playerX + playerDirectX] != wall)
                {
                    CollectDots(playerX, playerY, ref collectedDots, map);
                    Move(ref playerX, ref playerY, playerDirectX, playerDirectY, player, map);
                }

                if (map[enemyY + enemyDirectY, enemyX + enemyDirectX] != wall)
                {
                    Move(ref enemyX, ref enemyY, enemyDirectX, enemyDirectY, enemy, map);
                }
                else
                {
                    EnemyControl(randomDirection, ref enemyDirectX, ref enemyDirectY);
                }

                if (enemyX == playerX && enemyY == playerY)
                {
                    isAlive = false;
                }

                System.Threading.Thread.Sleep(200);

                if (collectedDots == allDots || !isAlive)
                {
                    isPlay = false;
                }

                Console.SetCursorPosition(0, map.GetLength(0));
                Console.Write($"Всего собрано{collectedDots}/{allDots}");
            }

            if (isAlive)
            {
                Console.SetCursorPosition(0, map.GetLength(0) + 2);
                Console.WriteLine("Поздравляем! Вы выжили и собрали все монетки! Вы - молодец!");
            }
            else if(!isAlive)
            {
                Console.SetCursorPosition(0, map.GetLength(0) + 2);
                Console.WriteLine("Ой-ой. Кажется, вас съели :( Попробуйте ещё раз!");
            }
        }

        static char[,] ReadMap(string fileName, out int playerX, out int playerY, out int enemyX, out int enemyY, out int allDots)
        {
            playerX = 0;
            playerY = 0;
            enemyX = 0;
            enemyY = 0;
            allDots = 0;

            string[] newFile = File.ReadAllLines($"Maps/{fileName}.txt");
            char[,]map = new char[newFile.Length, newFile[0].Length];

            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j  < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        playerX = j;
                        playerY = i;
                        map[i, j] = '.';
                    }
                    else if (map[i,j] == '0')
                    {
                        enemyX = j;
                        enemyY = i;
                        map[i, j] = '.';
                    }
                    else if (map[i,j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }
            return map;
        }

        static void DisplayMap(char[,]map)
        {
            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void PlayerControl(ref int playerDirectX, ref int playerDirectY)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    playerDirectX = 1;
                    playerDirectY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    playerDirectX = -1;
                    playerDirectY = 0;
                    break;
                case ConsoleKey.UpArrow:
                    playerDirectX = 0;
                    playerDirectY = -1;
                    break;
                case ConsoleKey.DownArrow:
                    playerDirectX = 0;
                    playerDirectY = 1;
                    break;
            }
        }

        static void EnemyControl(Random randomDirection, ref int enemyDirectX, ref int enemyDirectY)
        {
            int enemyDirection = randomDirection.Next(1, 5);

            switch (enemyDirection)
            {
                case 1:
                    enemyDirectX = 1;
                    enemyDirectY = 0;
                    break;
                case 2:
                    enemyDirectX = -1;
                    enemyDirectY = 0;
                    break;
                case 3:
                    enemyDirectX = 0;
                    enemyDirectY = -1;
                    break;
                case 4:
                    enemyDirectX = 0;
                    enemyDirectY = 1;
                    break;
            }
        }

        static void Move(ref int x, ref int y, int directX, int directY, char symbol, char[,]map)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(map[y,x]);

            x += directX;
            y += directY;

            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
        }

        static void CollectDots(int playerX, int playerY, ref int collectedDots, char[,] map)
        {
            if (map[playerY, playerX] == '.')
            {
                collectedDots++;
                map[playerY, playerX] = ' ';
            }
        }
    }
}