using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public bool IsDrag = false;
    public virtual void BeginDrag()
    {
        Debug.Log("BeginDrag" + gameObject.name);
        DragAndDropEvents.OnBeginDrag.Invoke(this);
        IsDrag = true;
    }
    public void Drag(Vector2 pos)
    {
        transform.position = pos;
    }
    public void Drop()
    {
        Debug.Log("Drop" + gameObject.name);
        DragAndDropEvents.OnEndDrag.Invoke();
        IsDrag = false;
    }
}
