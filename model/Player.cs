
namespace bot
{
    public class Player
    {
        public int playerId;
        public string displayName;
        public List<Hero> heroes;
        public HashSet<GemType> heroGemType;

        public Player(int playerId, string name)
        {
            this.playerId = playerId;
            this.displayName = name;

            heroes = new List<Hero>();
            heroGemType = new HashSet<GemType>();
        }

        public Hero anyHeroFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana()) return hero;
            }

            return null;
        }

        public Hero firstHeroAlive()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive()) return hero;
            }

            return null;
        }

        public Hero GetHeroSEA_SPIRIT()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.id == HeroIdEnum.AIR_SPIRIT) return hero;
            }

            return firstHeroAlive();
        }

        public bool IsHeroLive(HeroIdEnum id)
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.id == id) return true;
            }
            return false;
        }



        public Hero AttackedByFIRE_SPIRIT()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() &&
                ((hero.id == HeroIdEnum.AIR_SPIRIT) ||
                 (hero.id == HeroIdEnum.FIRE_SPIRIT) ||
                 (hero.id == HeroIdEnum.CERBERUS) ||
                 (hero.id == HeroIdEnum.SEA_GOD) ||
                 (hero.id == HeroIdEnum.MONK))
                )
                {
                    return hero;
                }
            }

            return firstHeroAlive();
        }

        public HashSet<GemType> getRecommendGemType()
        {
            heroGemType.Clear();
            foreach (var hero in heroes)
            {
                if (!hero.isAlive()) continue;

                foreach (var gt in hero.gemTypes)
                {
                    heroGemType.Add((GemType)gt);
                }
            }

            return heroGemType;
        }
    }
}