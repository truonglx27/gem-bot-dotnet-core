
using Sfs2X.Entities.Data;

namespace bot
{
    public class Grid
    {
        private List<Gem> gems = new List<Gem>();
        private ISFSArray gemsCode;
        public HashSet<GemType> gemTypes = new HashSet<GemType>();
        private HashSet<GemType> myHeroGemType;

        public Player myPlayer;

        public Grid(ISFSArray gemsCode, ISFSArray gemModifiers, HashSet<GemType> gemTypes, Player myPlayer)
        {
            updateGems(gemsCode, gemModifiers);
            this.myHeroGemType = gemTypes;

            this.myPlayer = myPlayer;
        }

        public void updateGems(ISFSArray gemsCode, ISFSArray gemModifiers)
        {
            gems.Clear();
            gemTypes.Clear();
            for (int i = 0; i < gemsCode.Size(); i++)
            {
                Gem gem = new Gem(i, (GemType)gemsCode.GetByte(i), gemModifiers != null ? (GemModifier)gemModifiers.GetByte(i) : GemModifier.NONE);
                gems.Add(gem);
                gemTypes.Add(gem.type);
            }
        }

        public Pair<int> recommendSwapGem()
        {
            List<GemSwapInfo> listMatchGem = suggestMatch();

            Console.WriteLine("recommendSwapGem " + listMatchGem.Count);
            if (listMatchGem.Count == 0)
            {
                return new Pair<int>(-1, -1);
            }

            GemSwapInfo matchGemSizeThanFour = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4).FirstOrDefault();
            if (matchGemSizeThanFour != null)
            {
                return matchGemSizeThanFour.getIndexSwapGem();
            }

            GemSwapInfo matchGemSword4 = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3 || gemMatch.type == GemType.SWORD).FirstOrDefault();
            if (matchGemSword4 != null)
            {
                return matchGemSword4.getIndexSwapGem();
            }

            GemSwapInfo matchGemSword = listMatchGem.Where(gemMatch => gemMatch.type == GemType.SWORD).FirstOrDefault();
            if (matchGemSword != null)
            {
                return matchGemSword.getIndexSwapGem();
            }

            // GemSwapInfo matchGemSizeThanThree = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3).FirstOrDefault();
            // if (matchGemSizeThanThree != null)
            // {
            //     return matchGemSizeThanThree.getIndexSwapGem();
            // }

            // foreach (GemType type in myHeroGemType)
            // {
            //     GemSwapInfo matchGem;

            //     if (this.myPlayer.IsHeroLive(HeroIdEnum.MONK))
            //     {
            //         matchGem = listMatchGem.Where(gemMatch => gemMatch.type == GemType.YELLOW || gemMatch.type == GemType.GREEN).FirstOrDefault();

            //         if (matchGem != null)
            //         {
            //             return matchGem.getIndexSwapGem();
            //         }
            //     }


            //     matchGem = listMatchGem.Where(gemMatch => gemMatch.type == type).FirstOrDefault();

            //     if (matchGem != null)
            //     {
            //         return matchGem.getIndexSwapGem();
            //     }
            // }


            // if (this.myPlayer.IsHeroLive(HeroIdEnum.MONK))
            // {
            //     GemSwapInfo matchGem = listMatchGem.Where(gemMatch => gemMatch.type == GemType.YELLOW || gemMatch.type == GemType.GREEN).FirstOrDefault();

            //     if (matchGem != null)
            //     {
            //         return matchGem.getIndexSwapGem();
            //     }
            // }

            foreach (Hero hero in this.myPlayer.heroes)
            {
                GemSwapInfo matchGem;

                if (hero.isAlive())
                {
                    if (hero.gemTypes[1] != null)
                    {
                        matchGem = listMatchGem.Where(gemMatch => gemMatch.type == (GemType)hero.gemTypes[0] || gemMatch.type == (GemType)hero.gemTypes[1]).FirstOrDefault();
                    }
                    else
                    {
                        matchGem = listMatchGem.Where(gemMatch => gemMatch.type == (GemType)hero.gemTypes[0]).FirstOrDefault();
                    }

                    if (matchGem != null)
                    {
                        return matchGem.getIndexSwapGem();
                    }
                }
            }

            return listMatchGem[0].getIndexSwapGem();
        }

        private List<GemSwapInfo> suggestMatch()
        {
            var listMatchGem = new List<GemSwapInfo>();

            var tempGems = new List<Gem>(gems);
            foreach (Gem currentGem in tempGems)
            {
                Gem swapGem = null;
                // If x > 0 => swap left & check
                if (currentGem.x > 0)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x - 1, currentGem.y)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If x < 7 => swap right & check
                if (currentGem.x < 7)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x + 1, currentGem.y)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If y < 7 => swap up & check
                if (currentGem.y < 7)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x, currentGem.y + 1)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If y > 0 => swap down & check
                if (currentGem.y > 0)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x, currentGem.y - 1)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
            }
            return listMatchGem;
        }

        private void checkMatchSwapGem(List<GemSwapInfo> listMatchGem, Gem currentGem, Gem swapGem)
        {
            swap(currentGem, swapGem);
            HashSet<Gem> matchGems = matchesAt(currentGem.x, currentGem.y);

            swap(currentGem, swapGem);
            if (matchGems.Count > 0)
            {
                listMatchGem.Add(new GemSwapInfo(currentGem.index, swapGem.index, matchGems.Count, currentGem.type));
            }
        }

        private int getGemIndexAt(int x, int y)
        {
            return x + y * 8;
        }

        private void swap(Gem a, Gem b)
        {
            int tempIndex = a.index;
            int tempX = a.x;
            int tempY = a.y;

            // update reference
            gems[a.index] = b;
            gems[b.index] = a;

            // update data of element
            a.index = b.index;
            a.x = b.x;
            a.y = b.y;

            b.index = tempIndex;
            b.x = tempX;
            b.y = tempY;
        }

        private HashSet<Gem> matchesAt(int x, int y)
        {
            HashSet<Gem> res = new HashSet<Gem>();
            Gem center = gemAt(x, y);
            if (center == null)
            {
                return res;
            }

            // check horizontally
            List<Gem> hor = new List<Gem>();
            hor.Add(center);
            int xLeft = x - 1, xRight = x + 1;
            while (xLeft >= 0)
            {
                Gem gemLeft = gemAt(xLeft, y);
                if (gemLeft != null)
                {
                    if (!gemLeft.sameType(center))
                    {
                        break;
                    }
                    hor.Add(gemLeft);
                }
                xLeft--;
            }
            while (xRight < 8)
            {
                Gem gemRight = gemAt(xRight, y);
                if (gemRight != null)
                {
                    if (!gemRight.sameType(center))
                    {
                        break;
                    }
                    hor.Add(gemRight);
                }
                xRight++;
            }
            if (hor.Count >= 3) res.UnionWith(hor);

            // check vertically
            List<Gem> ver = new List<Gem>();
            ver.Add(center);
            int yBelow = y - 1, yAbove = y + 1;
            while (yBelow >= 0)
            {
                Gem gemBelow = gemAt(x, yBelow);
                if (gemBelow != null)
                {
                    if (!gemBelow.sameType(center))
                    {
                        break;
                    }
                    ver.Add(gemBelow);
                }
                yBelow--;
            }
            while (yAbove < 8)
            {
                Gem gemAbove = gemAt(x, yAbove);
                if (gemAbove != null)
                {
                    if (!gemAbove.sameType(center))
                    {
                        break;
                    }
                    ver.Add(gemAbove);
                }
                yAbove++;
            }
            if (ver.Count >= 3) res.UnionWith(ver);

            return res;
        }

        // Find Gem at Position (x, y)
        private Gem gemAt(int x, int y)
        {
            foreach (Gem g in gems)
            {
                if (g != null && g.x == x && g.y == y)
                {
                    return g;
                }
            }
            return null;
        }
    }
}