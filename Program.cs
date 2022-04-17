namespace bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Game!");
            
            var game = new Game();
            game.Start();

            Console.ReadLine();
        }        
    }
}