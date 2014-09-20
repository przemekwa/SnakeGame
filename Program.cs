namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var snakeGame = new Snake(40, 20, 5);
            snakeGame.PlayGame();
        }
    }
}
