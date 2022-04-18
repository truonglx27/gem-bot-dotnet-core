namespace bot 
{
    public class Gem {
    private const int HEIGHT = 8;
    private const int WIDTH = 8;
    private int index;
    private int x;
    private int y;
    private GemType type;

    public Gem(int index, GemType type) {
        this.index = index;
        this.type = type;
        updatePosition();
    }

    private void updatePosition() {
        y = index / HEIGHT;
        x = index - y * WIDTH;
    }

    public bool sameType(Gem other) {
        return this.type == other.type;
    }

    public bool sameType(GemType type) {
        return this.type == type;
    }

    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        Gem gem = (Gem) o;

        if (index != gem.index) return false;
        return type == gem.type;
    }

    public int hashCode() {
        int result = index;
        result = 31 * result + (type != null ? type.GetHashCode() : 0);
        return result;
    }
}
}