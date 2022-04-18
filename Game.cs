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

        public void Start(string ip)
        {
            _bot.Load();
            _timer = new Timer(Tick, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);

            // Smartfox
            sfs = new SmartFox();
            sfs.ThreadSafeMode = false;
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.AddEventListener(SFSEvent.LOGIN, onLoginSuccess);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

            // Set connection parameters
            ConfigData cfg = new ConfigData();
            cfg.Host = ip;
            cfg.Port = 9933;
            cfg.Zone = "gmm";
            cfg.Debug = false;
            cfg.BlueBox.IsActive = true;
            
            sfs.Connect(cfg);            
        }

        private void onLoginSuccess(BaseEvent evt)
        {
            //var username = evt.Params["user"] as User;
            Console.WriteLine("LoginSuccess");
        }

        private void OnLoginError(BaseEvent evt)
        {
            Console.WriteLine("Login error");
        }

        private void Tick(Object state)
        {
            Console.WriteLine("Tick");
            if (sfs != null) sfs.ProcessEvents();

            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private static void OnConnection(BaseEvent evt)
        {
            Console.WriteLine("Smartfox connection state: " + (bool)evt.Params["success"]);
            SFSObject parameters = new SFSObject();
            parameters.PutUtfString("BATTLE_MODE", "NORMAL");
            parameters.PutUtfString("ID_TOKEN", "bot");
            sfs.Send(new LoginRequest("tung.tranhuy", "", "gmm", parameters));
        }

        public void Stop()
        {
            _bot?.Unload();
        }
    }
}