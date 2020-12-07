using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class PawnScript : MonoBehaviour
{
    protected bool isWhite = false;
    private Vector2Int pos = new Vector2Int(0,0);

    public Vector2Int coordinate {
        get { return this.pos;}
        set {
            if(IsValidPosition(value)) {
                pos = value;
            }
    }}

    public bool IsWhite{
    get { return this.isWhite; } 
    set {
        var img = GetComponent<Image>();
        this.isWhite = value;
        GetComponent<Image>().color = value ? Color.white : Color.black;
    }}

    public void SetPositionByCell(CellScript cell) {

        this.transform.position = cell.transform.position;
        this.coordinate = cell.coordinate;
    }
    public virtual List<Step> PossibleSteps() {
        return new List<Step>();
    }

    protected PawnScript HavePawnInPoint(Vector2Int point) {
        return BasicTable.Instance.GetPawn(point);
    }

    protected void Step (int x, int y, ref List<Step> moves) {
        if(!IsValidPosition(new Vector2Int(x,y))) return;

        var anotherPawn = HavePawnInPoint(new Vector2Int(x, y));
        var currentCell = BasicTable.Instance.GetCell(new Vector2Int(x, y));

        if(anotherPawn != null){
            if(this.isWhite == anotherPawn.isWhite)
                return;
        }

        moves.Add(new Step(currentCell, anotherPawn));
    }

    private bool IsValidPosition(Vector2Int point) {
        return (point.x >= 0 && point.x <= 7 && point.y >= 0 && point.y <= 7);
    }
}
