using Sfs2X.Entities;
using Sfs2X.Entities.Data;
namespace bot
{
    public class GemBot : BaseBot
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

        protected override void StartGame(ISFSObject gameSession, Room room){         
            // Assign Bot player & enemy player
            AssignPlayers(room);

            // Player & Heroes
            ISFSObject objBotPlayer = gameSession.GetSFSObject(botPlayer.displayName);
            ISFSObject objEnemyPlayer = gameSession.GetSFSObject(enemyPlayer.displayName);

            ISFSArray botPlayerHero = objBotPlayer.GetSFSArray("heroes");
            ISFSArray enemyPlayerHero = objEnemyPlayer.GetSFSArray("heroes");

            for (int i = 0; i < botPlayerHero.Size(); i++) {
                var hero = new Hero(botPlayerHero.GetSFSObject(i));
                botPlayer.heroes.Add(hero);
            }

            for (int i = 0; i < enemyPlayerHero.Size(); i++) {
                enemyPlayer.heroes.Add(new Hero(enemyPlayerHero.GetSFSObject(i)));
            }

            // Gems
            grid = new Grid(gameSession.GetSFSArray("gems"), botPlayer.getRecommendGemType());
            currentPlayerId = gameSession.GetInt("currentPlayerId");
            log("StartGame ");
            
            SendFinishTurn(true);
            //taskScheduler.schedule(new FinishTurn(true), new Date(System.currentTimeMillis() + delaySwapGem));
        }

        protected override void SwapGem(ISFSObject paramz)
        {

        }

        protected override void HandleGems(ISFSObject paramz)
        {

        }

        protected override void StartTurn(ISFSObject paramz)
        {
            currentPlayerId = paramz.GetInt("currentPlayerId");
            if (!isBotTurn()) {
                return;
            }

            Hero heroFullMana = botPlayer.anyHeroFullMana();
            if (heroFullMana != null) {
                //taskScheduler.schedule(new SendReQuestSkill(heroFullMana.get()), new Date(System.currentTimeMillis() + delaySwapGem));
                SendCastSkill(heroFullMana);
                return;
            }

            //taskScheduler.schedule(new SendRequestSwapGem(), new Date(System.currentTimeMillis() + delaySwapGem));
            SendSwapGem();
        }

        protected bool isBotTurn() {
            return botPlayer.playerId == currentPlayerId;
        }
    }
}