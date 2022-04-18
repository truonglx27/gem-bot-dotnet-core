namespace bot
{
    class Program
    {
        private const string IP = "172.16.15.54";
        static void Main(string[] args)
        {
            Console.WriteLine("=== PRESS `q` AT ANY TIME TO STOP THIS ===");
            
            var game = new Game();
            game.Start(IP);

            while (true){
                var input = Console.ReadKey();

                if (input != null && input.KeyChar == 'q'){
                    break;
                }
            }
        }        
    }
}