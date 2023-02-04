using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTargetTest : MonoBehaviour
{
    [SerializeField] List<GameObject> elementsReceivable;
    public void DragReceive(Dragable dragObj)
    {
        Debug.Log("DragTarget" + dragObj.GetType().Name);
    }

    public void EventRegister()
    {
        DragAndDropEvents.OnBeginDrag += OutLine;
    }

    public void OutLine(Dragable dragable)
    {
        var element = dragable.GetType();
        Debug.Log(element);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventRegister();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        DragAndDropEvents.OnBeginDrag -= OutLine;
    }

    public void StopOutline()
    {
        
    }
}
