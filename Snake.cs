namespace SnakeGame
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

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

        public ConsoleKeyInfo kierunek { get; set; }

        int[,] map;

        readonly LinkedList<Point> SnakeBodyPoints = new LinkedList<Point>();

        public Snake(int sizeX, int sizeY, int sizeSnake)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            this.MapX = sizeX;
            this.MapY = sizeY;
            this.SnakeLenght = sizeSnake;
        }

        public void PlayGame()
        {
            Task t = new Task(
                () => 
                    {
                        while (true)
                            kierunek = Console.ReadKey();
                    }
                );
            t.Start();
            
            kierunek = new ConsoleKeyInfo('s',ConsoleKey.LeftArrow,false,false,false) ; // Console.ReadKey();
            
            while (true)
            Play(); 
        }

        private void Play()
        {
            int velocity = 100;

            map = new int[MapX, MapY];

            CreateBounds();

            var endGame = false;

            var random = new Random();

            map[random.Next(1, MapX - 1), random.Next(1, MapY - 1)] = 8;  

            var actualPosition = new Point(random.Next(2, MapX - 2), random.Next(2, MapY - 2));

            SnakeBodyPoints.Clear();

            CreateSnake(actualPosition.X, actualPosition.Y);

            DrawMap();

            while (!endGame)
            {
                System.Threading.Thread.Sleep(velocity);

                var key = kierunek;//new ConsoleKeyInfo('s',ConsoleKey.LeftArrow,false,false,false) ; // Console.ReadKey();


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
                        SnakeBodyPoints.AddFirst(actualPosition);
                        SnakeBodyPoints.RemoveLast();
                        break;
                    case 8:
                        SnakeBodyPoints.AddFirst(actualPosition);
                        CreateSnakeDiner();
                        velocity -= 10;
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

        private void CreateSnakeDiner()
        {
            var random = new Random();

            var goodPoint = false;

            Point p = new Point();
            
            while (!goodPoint)
            {
                p.X = random.Next(1, MapX - 2);
                p.Y = random.Next(1, MapY - 2);

                foreach (var snakeBodyPoint in SnakeBodyPoints)
                {
                    if (snakeBodyPoint.X != p.X && snakeBodyPoint.Y != p.Y)
                    {
                        goodPoint = true;
                    }
                }
            }

            map[p.X, p.Y] = 8;

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

                while (!CheckPosition(x,y))  // Jak będzie w rogu to się zawiesi : )
                {
                    if (x > MapX - 2)
                    {
                        x--;
                        y++;
                    }

                    if (x < 2)
                    {
                        x++;
                    }


                    if (y > MapY - 2)
                    {
                        x++;
                        y--;
                    }
                    
                    if (y < 2)
                    {
                        y++;
                    }



                }

                SnakeBodyPoints.AddLast(new Point(x++, y));
            }

            AddSnakeToMap();
        }

        private bool CheckPosition(int x, int y)
        {
            if (x > MapX - 2 || x < 2)
            {
                return false;
            }

            if (y > MapY - 2 || x < 2)
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
            foreach (var point in SnakeBodyPoints)
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
