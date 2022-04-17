using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Timer = System.Threading.Timer;

namespace bot
{
    class Game
    {
        private static SmartFox sfs;

        private Bot _bot = new Bot();
        private const int TIME_INTERVAL_IN_MILLISECONDS = 1000;
        private Timer _timer = null;


        public bool Running { get; private set; }

        public void Start()
        {
            _bot.Load();
            _timer = new Timer(Tick, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);

            // Smartfox
            sfs = new SmartFox();
            sfs.ThreadSafeMode = false;
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);

            // Set connection parameters
            ConfigData cfg = new ConfigData();
            cfg.Host = "172.16.15.54";
            cfg.Port = 9933;
            cfg.Zone = "gmm";
            cfg.Debug = false;

            sfs.Connect(cfg);

            Running = true;
        }

        private void Tick( Object state )
        {
            Console.WriteLine("Tick");
            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private static void OnConnection(BaseEvent evt)
        {
            Console.WriteLine("Smartfox connection state: " + (bool)evt.Params["success"]);
            SFSObject parameters = new SFSObject();
            parameters.PutUtfString("BATTLE_MODE", "BATTLE_MODE");
            parameters.PutUtfString("ID_TOKEN", "bot");
            sfs.Send(new LoginRequest("username", "", "gmm", parameters));
        }

        public void Stop()
        {
            Running = false;
            _bot?.Unload();
        }
    }
}