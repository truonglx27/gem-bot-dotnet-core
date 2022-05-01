namespace bot
{
    class Program
    {      
        static void Main(string[] args)
        {
            Console.WriteLine("=== PRESS `q` AT ANY TIME TO STOP THIS ===");
            
            var game = new Game();
            game.Start();

            while (true){
                var input = Console.ReadKey();

                if (input != null && input.KeyChar == 'q'){
                    break;
                }
            }
        }        
    }
}