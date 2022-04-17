using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Timer = System.Threading.Timer;

namespace bot
{
    class Program
    {
        private const int TIME_INTERVAL_IN_MILLISECONDS = 1000;
        static SmartFox sfs;
        static Timer _timer;
        static GameLoop gameLoop;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            gameLoop = new GameLoop();            
            gameLoop.Start();

            // Start Graphics Timer
            _timer = new Timer( Callback, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite );

            sfs = new SmartFox();
            sfs.ThreadSafeMode = false;
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);

            // Set connection parameters
            ConfigData cfg = new ConfigData();
            cfg.Host = "172.16.15.54";
            cfg.Port = 9933;
            cfg.Zone = "gmm";
            
            cfg.Debug = false;

            // Connect to SFS2X
            sfs.Connect(cfg);

            var name = Console.ReadLine();

            var account = new Account();
            account.Name = name;

            Console.WriteLine(account.Name);
        }

        private static void Callback( Object state )
        {
            Console.WriteLine("Tick");
            // Long running operation
            _timer.Change( TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite );
        }

        private static void OnConnection(BaseEvent evt)
        {
            Console.WriteLine((bool)evt.Params["success"]);
            SFSObject parameters = new SFSObject();
            parameters.PutUtfString("BATTLE_MODE", "BATTLE_MODE");
            parameters.PutUtfString("ID_TOKEN", "bot");
            sfs.Send(new LoginRequest("username", "", "gmm", parameters));
        }
    }
}