namespace bot
{
    public class GemSwapInfo 
    {
        private int index1;
        private int index2;
        private int sizeMatch;
        private GemType type;

        public Pair<int> getIndexSwapGem() {
            return new Pair<int>(index1, index2);
        }
    }
}