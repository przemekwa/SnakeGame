namespace SnakeGame
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public class Snake
    {
        private int MapX { get; set; }
        private int MapY { get; set; }
        private int SnakeLenght { get; set; }

        int[,] map;

        readonly LinkedList<Point> SnakeBodyPoint = new LinkedList<Point>();

        public Snake(int sizeX, int sizeY, int sizeSnake)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            this.MapX = sizeX;
            this.MapY = sizeY;
            this.SnakeLenght = sizeSnake;
        }

        public void PlayGame()
        {
            while (true)
                Play(); 
        }

        private void Play()
        {
            map = new int[MapX, MapY];

            CreateBounds();

            var endGame = false;

            var random = new Random();

            map[random.Next(1, MapX - 1), random.Next(1, MapY - 1)] = 8;  

            var actualPosition = new Point(random.Next(2, MapX - 2), random.Next(2, MapY - 2));

            SnakeBodyPoint.Clear();

            CreateSnake(actualPosition.X, actualPosition.Y);

            DrawMap();

            while (!endGame)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        actualPosition.Y--;
                        break;
                    case ConsoleKey.DownArrow:
                        actualPosition.Y++;
                        break;
                    case ConsoleKey.LeftArrow:
                        actualPosition.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        actualPosition.X++;
                        break;
                }

                switch (map[actualPosition.X, actualPosition.Y])
                {
                    case 0:
                        SnakeBodyPoint.AddFirst(actualPosition);
                        SnakeBodyPoint.RemoveLast();
                        break;
                    case 8:
                        SnakeBodyPoint.AddFirst(actualPosition);
                        map[random.Next(1, MapX - 1), random.Next(1, MapY - 1)] = 8;
                        break;
                    case 9:
                        endGame = true;
                        break;
                    case 2:
                        endGame = true;
                        break;
                }
                ClearSnakePosition();
                AddSnakeToMap();
                RefreshScrean();
            }
        }

        private void RefreshScrean()
        {
            Console.Clear();
            DrawMap();
        }

        private void ClearSnakePosition()
        {
            for (var i = 0; i < MapX; i++)
            {
                for (var j = 0; j < MapY; j++)
                {
                    if (map[i, j] == 2)
                        map[i, j] = 0;
                }
            }
        }

        private void CreateSnake(int x, int y)
        {
            for (var i = 0; i < SnakeLenght; i++)
            {
                SnakeBodyPoint.AddLast(new Point(x++, y));
            }

            AddSnakeToMap();
        }

        private bool CheckPosition(int x, int y)
        {
            if (x > MapX - 1 || x < 2)
            {
                return false;
            }

            if (y > MapY - 1 || x < 2)
            {
                return false;
            }

            return true;
        }

        private void DrawMap()
        {
            Console.Clear();

            for (int y = 0; y < MapY; y++)
            {
                string linia = "";

                for (int x = 0; x < MapX; x++)
                {
                    switch (map[x, y])
                    {
                        case 1:
                            linia = linia + "*";
                            break;
                        case 2:
                            linia = linia + (char)1;
                            break;
                        case 7:
                            linia = linia + "B";
                            break;
                        case 8:
                            linia = linia + (char)15;
                            break;
                        case 9:
                            linia = linia + (char)176;
                            break;
                        default:
                            linia = linia + " ";
                            break;
                    }
                }
                Console.Write(linia);
                Console.WriteLine();
            }
        }

        private void AddSnakeToMap()
        {
            foreach (var point in SnakeBodyPoint)
            {
                map[point.X, point.Y] = 2;
            }
        }

        private void CreateBounds()
        {
            for (int y = 0; y < MapY; y++)
            {
                map[0, y] = 9;

                map[MapX - 1, y] = 9;

                if (y == 0 || y == MapY - 1)
                {
                    for (int i = 0; i < MapX; i++)
                    {
                        map[i, y] = 9;
                    }
                }
            }
        }
    }
}
