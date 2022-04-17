using bot;

class GameLoop
{
    private Bot _bot = new Bot();

    public bool Running { get; private set; }

    public async void Start()
    {
        if (_bot == null)
            throw new ArgumentException("Bot not loaded!");

        _bot.Load();

        Running = true;

        DateTime _previousGameTime = DateTime.Now;

        while (Running)
        {
            // Calculate the time elapsed since the last game loop cycle
            TimeSpan GameTime = DateTime.Now - _previousGameTime;
            // Update the current previous game time
            _previousGameTime = _previousGameTime + GameTime;
            // Update the game
            _bot.Update(GameTime);
            // Update Game at 60fps
            await Task.Delay(8);
        }
    }

    public void Stop()
    {
        Running = false;
        _bot?.Unload();
    }
}