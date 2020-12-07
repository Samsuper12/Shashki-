using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour
{   
    public delegate void OnCellClicked(CellScript point);

    private OnCellClicked onCellClicked;
    private Image img;
    private Color originColor;
    private Transform pos;
    private EventTrigger events;
    private Color lightColor;
    private bool isLight = false;

    private bool isPlaylable;

    public bool IsPlaylable {
        get{ return this.isPlaylable; }
        set{ this.isPlaylable = value;}
    }

    [SerializeField]
    private float colorSpeed = 1.0f; 

    public Vector2Int coordinate {get; set;} = new Vector2Int(0, 0);

    public Color CellColor { 
    get {
        return this.GetComponent<Image>().color;
    }}

    public void SetColor(Color value) {
        if (img == null) {
            img = GetComponent<Image>();
        }

        if (value != originColor) {
            originColor = value;
        }
        GetComponent<Image>().color = value;
    }

    public void EnableLight(Color light) {
        this.isLight = true;
        this.lightColor = light;
    }

    public void DisableLight() {
        EnableLight(originColor);
    }

    public void SetClickDelegate(OnCellClicked del) {
        this.onCellClicked = del;
    }

    private IEnumerator test() {
        yield return new WaitForSeconds(500);
    }


    void Start()
    {
        img = GetComponent<Image>();
        pos = GetComponent<Transform>();
        events = GetComponent<EventTrigger>();

        EventTrigger.Entry pEnter = new EventTrigger.Entry();
        pEnter.eventID = EventTriggerType.PointerEnter;
        pEnter.callback.AddListener((data) => {EnableLight(Color.red);});
        
        EventTrigger.Entry pExit = new EventTrigger.Entry();
        pExit.eventID = EventTriggerType.PointerExit;
        pExit.callback.AddListener((data) => {DisableLight();});

        EventTrigger.Entry pClicked = new EventTrigger.Entry();
        pClicked.eventID = EventTriggerType.PointerClick;
        pClicked.callback.AddListener((data) => {
            Debug.Log("clicked on cell");
            if(onCellClicked == null) return;
            onCellClicked(this);
        });

        events.triggers.Add(pEnter);
        events.triggers.Add(pExit);
        events.triggers.Add(pClicked);
    }

    void Update()
    {
        if(isLight) {
            StartCoroutine(changeColor());
        }
    }

    private IEnumerator changeColor() {
        float tick = 0.0f;
        Color origin = CellColor;
        while(CellColor != lightColor) {
            tick += Time.deltaTime * colorSpeed;
            img.color = Color.Lerp(origin, lightColor, tick);
            yield return null;
        }
        isLight = false;
    }
}
