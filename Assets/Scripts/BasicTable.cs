using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTable : MonoBehaviour
{
     public static BasicTable Instance {get; private set;}

     // USER 1
     // USER 2

    [SerializeField]
    protected Object cellPrefab;

    
    [SerializeField]
    private Object pawnPrefab;

    [SerializeField]
    private Object superPrefab;

    [SerializeField]
    private Color playlableColor;

    [SerializeField]
    private Color groundColor;

    [SerializeField]
    private Color helpColor;

    private const int columns = 8;
    private const int rows = 8;
    private const int pawnsCount = 24;

    protected GameObject[,] cellsArray;
    private List<GameObject> pawnsList;


    // вынести в логику
    private bool cellSelected = false;
    private CellScript cellPrevious = null;
    private PawnScript pawnPrevious = null;
    private bool whiteTurn = false;
    private List<Step> tempMoves = null;

    void Start() {
        Instance = this;
        cellsArray = new GameObject[columns, rows];
        pawnsList = new List<GameObject>();
        PopulateArea(ref cellsArray, columns, rows);
        PaintArea(ref cellsArray, columns, rows, playlableColor, groundColor);
        PopulatePawns();
    }

    void Update() {
    }

    public PawnScript GetPawn(Vector2Int point) {
        foreach(var pawn in pawnsList ) {
            var script = pawn.GetComponent<PawnScript>() as PawnScript;
            if(script.coordinate == point){
                return script;
            }
        }
        Debug.Log("GetPawn return null");
        return null;
    }

    public CellScript GetCell(Vector2Int point) {

        foreach(var cell in cellsArray) {
            var script =  cell.GetComponent<CellScript>();
            if(script.coordinate == point) return script;
        }
        Debug.Log("GetCell return null");
        return null;
    }

    private void PopulateArea(ref GameObject[,] array, int columns, int rows) {
        for(int i = 0; i < columns; ++i) {
            for(int j = 0; j < rows; ++j) {
                GameObject obj = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                CellScript obj_script = obj.GetComponent<CellScript>() as CellScript;
                var rect = obj.transform.GetComponent<RectTransform>();
                obj.transform.SetParent(transform);
                
                float cellWidth = rect.rect.width;
                float cellHeight = rect.rect.width;
                
                // магия. Не смотреть
                obj.transform.position = transform.position - new Vector3(cellWidth * rows / 2, -(cellHeight * columns / 2), 0);
                rect.localPosition += new Vector3(j * (cellWidth / 2), -((cellHeight /2 ) * i), 0);

                obj_script.coordinate = new Vector2Int(j, i);
                obj_script.SetClickDelegate(this.onCellClicked);
                array[i,j] = obj;
            }
        }
    }//Аня

    private void PaintArea(ref GameObject[,] array, int columns, int rows, Color colorOne, Color colorTwo) {
        for(int i = 0; i < columns; ++i) {
            for(int j = 0; j < rows; ++j) {
                var script = array[i, j].GetComponent<CellScript>();
                bool isPlaylable = isPlaylableCell(i + 1, j + 1);
                script.SetColor(isPlaylable ? colorOne : colorTwo);
                script.IsPlaylable = isPlaylable;
            }
        }
    }

    private bool isPlaylableCell(int x, int y) {
        bool xOdd = x % 2 == 0;
        bool yOdd = y % 2 == 0;

        if (xOdd) {
            if (!yOdd)
                return true;
        }
        else{
            if (yOdd)
                return true;
        }
        return false;
    }

    // Лютые алгоритмы в 4 ночи
    private void PopulatePawns() {
        var positions = new List<Vector2Int>();

        // Black
        for(int i = 1; i < columns + 1; ++i) {
            // Если ряд черных заполнен, то прыгаем к белым.
            if (i == columns / 2) i = 6;
            
            for(int j = 1; j < rows + 1; ++j) {
                if(isPlaylableCell(j , i))
                    positions.Add(new Vector2Int(j - 1, i - 1)); // TODO
            }
        }

        // Instantiate
        for(int i = 0; i < pawnsCount; ++i) {
            bool isWhite = (i + 1 > (pawnsCount / 2)) ? true : false;
            var temp = CreatePawn(pawnPrefab, positions[i], isWhite);

            pawnsList.Add(temp);
        }
    }

    private bool InMoveRange(Vector2Int point, ref List<Step> moves) {
        foreach(var move in moves) {
            if(move.current.coordinate == point) {
                return true;
            }
        }
        return false;
    }

    // do not try to understand that code.
    private void RemoveByPoint(Vector2Int point, ref List<Step> steps) {
        for(int i = 0; i < steps.Count; ++i) {
            if(steps[i].current.coordinate == point && (i - 1) >= 0) {
                if(steps[i - 1].impact) {
                    var obj = steps[i - 1].enemy.gameObject;
                    Destroy(obj);
                    pawnsList.Remove(obj);
                }
            }
        }
    }

    private void onCellClicked(CellScript point) {
        if(!point.IsPlaylable) return;

        CheckLooser();

        if(cellSelected) {
            Debug.Log("is old celected cell");
            if(InMoveRange(point.coordinate, ref tempMoves)) {
                pawnPrevious.SetPositionByCell(point);
                RemoveByPoint(point.coordinate, ref tempMoves);
            }
            MaybeSuperPawn(point.coordinate);

            whiteTurn = !whiteTurn;

            cellSelected = false;
            cellPrevious = null;
            pawnPrevious = null;
            HighlightMoves(ref tempMoves, false);
            tempMoves = null;
        }
        else {
            
            var pwn = this.GetPawn(point.coordinate);

            if(pwn == null) return;
            if(pwn.IsWhite != whiteTurn) return;

            cellPrevious = point;
            pawnPrevious = pwn;
            
            tempMoves = pwn.PossibleSteps();
            HighlightMoves(ref tempMoves, true);
            cellSelected = true;
        }
    }

    private void HighlightMoves(ref List<Step> moves, bool state) {

        foreach(var move in moves) {
            if(!move.impact) {
               if(state) move.current.EnableLight(helpColor);
               else move.current.DisableLight();
            }
        }
    }

    private void CheckLooser() {
        int whiteCount = 0, whiteCountSteps = 0;
        int blackCount = 0, blackCountSteps = 0;
        foreach(var pawn in pawnsList) {
            var objScript = pawn.GetComponent<PawnScript>();
            if(objScript.IsWhite) {
                whiteCountSteps += objScript.PossibleSteps().Count;
                ++whiteCount;
            }
            else {
                blackCountSteps += objScript.PossibleSteps().Count;
                ++blackCount;
            }
        }

        if(whiteCount == 0 || whiteCountSteps == 0) {
            // white is looser
        }

        if(blackCount == 0 || blackCountSteps == 0) {
            // black is looser
        }
    }

    private GameObject CreatePawn(Object pawnObject, Vector2Int point, bool isWhite) {
        if(GetPawn(point)!= null) return null;

        var objPawn = Instantiate(pawnObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        var objScript = objPawn.GetComponent<PawnScript>() as PawnScript;
        var objRect = objPawn.GetComponent<RectTransform>() as RectTransform;

        objPawn.transform.SetParent(transform);

            // maybe use setPosition be cell?
        objRect.transform.position = this.GetCell(point).transform.position;
        objScript.IsWhite = isWhite;
        objScript.coordinate = point;

        return objPawn;
    }

    private void MaybeSuperPawn(Vector2Int point) {
        var pawnOld = GetPawn(point);
        if(pawnOld == null) return;
        bool canMutate = false;

        if(pawnOld.IsWhite) {
            // white
            if(pawnOld.coordinate.y == 0) 
                canMutate = true;
        }
        else {
            // black
            if(pawnOld.coordinate.y == 7) 
                canMutate = true;
        }

        if(!canMutate)  return;


        bool color = pawnOld.IsWhite;
        Destroy(pawnOld.gameObject);
        pawnsList.Remove(pawnOld.gameObject);

        var pawnNew = CreatePawn(superPrefab, point, color);
        pawnsList.Add(pawnNew); 
    }
}
