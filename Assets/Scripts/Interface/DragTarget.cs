using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTarget : MonoBehaviour
{
    void Start()
    {
        DragAndDropEvents.OnBeginDrag += OutLine;
        DragAndDropEvents.OnEndDrag += StopOutline;
    }
    void OutLine(Dragable dragObj)
    {
        Debug.Log("StartOutline");
    }
    void StopOutline()
    {
        Debug.Log("StopOutline");
    }
    public void DragReceive(Dragable dragObj)
    {
        Debug.Log("DragReceive" +  dragObj.name);
        dragObj.IsDrag = false;
    }
}
