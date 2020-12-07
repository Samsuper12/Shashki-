
public class Step
{
    public CellScript current {get;}
    public bool impact {get;}
    public PawnScript enemy {get;}

    public Step(CellScript current, PawnScript enemy = null) {
        this.impact = (enemy != null) ? true : false;
        this.enemy = enemy;
        this.current = current;
    }
}
