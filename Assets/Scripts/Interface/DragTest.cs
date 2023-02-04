using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour
{
    bool isDrag;
    private void Update()
    {
       
    }
    public void Drag(Vector2 pos)
    {
        transform.position = pos;
    }

    public void Drop()
    {
        isDrag = false;
        Debug.Log("Drop");
    }

    public void BeginDrag()
    {
        Debug.Log("BeginDrag");
        isDrag = true;
    }
}
