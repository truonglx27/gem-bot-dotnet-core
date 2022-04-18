using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Timer = System.Threading.Timer;

namespace bot
{
    class Game
    {
        private SmartFox sfs;
        private const string username = "trung.hoangdinh";

        private Bot _bot = new Bot();
        private const int TIME_INTERVAL_IN_MILLISECONDS = 1000;
        private Timer _timer = null;
        private bool isJoinGameRoom = false;

        private Room room;

        public Game(){
            sfs = new SmartFox();
            sfs.ThreadSafeMode = false;
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.AddEventListener(SFSEvent.LOGIN, onLoginSuccess);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

            sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
            sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        }

        private void FindRoomAndJoin(){
            List<Room> rooms = sfs.RoomList;
            List<Room> joinedRooms = sfs.JoinedRooms;

            var room = rooms.First(room1 => { return room1.IsGame && room1.UserCount == 1;} );
            if (room != null) {
                sfs.Send(new JoinRoomRequest(room));
                isJoinGameRoom = true;
            }
        }

        private void OnRoomJoin(BaseEvent evt)
        {
            Console.WriteLine("Joined room " + this.sfs.LastJoinedRoom.Name);
            this.room = (Room) evt.Params["room"];
            if (room.IsGame) {
                return;
            }

            //taskScheduler.schedule(new FindRoomGame(), new Date(System.currentTimeMillis() + delayFindGame));
        }

        private void OnRoomJoinError(BaseEvent evt)
        {
            Console.WriteLine("Join-room ERROR");
        }

        private void OnExtensionResponse(BaseEvent evt)
        {            
            var evtParam = (ISFSObject)evt.Params["params"];
            var cmd = (string)evt.Params["cmd"];
            Console.WriteLine("OnExtensionResponse " + cmd);

            switch (cmd){
                case ConstantCommand.START_GAME:
                    ISFSObject gameSession = evtParam.GetSFSObject("gameSession");
                    //startGame(gameSession, room);
                    break;
                case ConstantCommand.END_GAME:
                    //endGame();
                    break;
                case ConstantCommand.START_TURN:
                    //startTurn(evtParam);
                    break;
                case ConstantCommand.ON_SWAP_GEM:
                    //swapGem(evtParam);
                    break;
                case ConstantCommand.ON_PLAYER_USE_SKILL:
                    //handleGems(evtParam);
                    break;
                case ConstantCommand.PLAYER_JOINED_GAME:
                    this.sfs.Send(new ExtensionRequest(ConstantCommand.I_AM_READY, new SFSObject(), room));                    
                    break;
            }
        }

        public void Start(string ip)
        {
            Console.WriteLine("Connecting to Game-Server at " + ip);

            _bot.Load();
            _timer = new Timer(Tick, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);

            // Connect to SFS
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
            var user = evt.Params["user"] as User;
            Console.WriteLine("Login Success! You are: " + user?.Name);
        }

        private void OnLoginError(BaseEvent evt)
        {
            Console.WriteLine("Login error");
        }

        private void Tick(Object state)
        {
            if (sfs != null) sfs.ProcessEvents();

            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);

            if (!isJoinGameRoom){
                FindRoomAndJoin();
            }
        }

        private void OnConnection(BaseEvent evt)
        {
            Console.WriteLine("Smartfox connection state: " + (bool)evt.Params["success"]);
            SFSObject parameters = new SFSObject();
            parameters.PutUtfString("BATTLE_MODE", "NORMAL");
            parameters.PutUtfString("ID_TOKEN", "bot");
            sfs.Send(new LoginRequest(username, "", "gmm", parameters));
        }

        private void OnConnectionLost(BaseEvent evt)
        {
            Console.WriteLine("Smartfox OnConnectionLost");
        }

        public void Stop()
        {
            _bot?.Unload();
        }
    }
}