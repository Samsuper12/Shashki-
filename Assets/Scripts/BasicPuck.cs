using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPuck : MonoBehaviour
{
    public Color color{get;set;}
    public System.UInt32 position{get; protected set;}
    
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void remove() {
        //this.gameObject.destroy();
    }

    public virtual void setPosition(System.UInt32 pos) {
        if (pos > 64) {
            return;
        }
        this.position = pos;
    }
}
