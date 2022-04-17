namespace bot 
{
    public class Bot
    {
        internal void Load()
        {
            Console.WriteLine("Bot.Load()");
        }

        internal void Update(TimeSpan gameTime)
        {
            Console.WriteLine("Bot.Update()");
        }

        internal void Unload()
        {
            Console.WriteLine("Bot.Unload()");
        }
    }
}