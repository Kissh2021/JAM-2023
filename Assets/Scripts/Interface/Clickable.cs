using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public void Click()
    {
        Debug.Log("ClickOn" + gameObject.name);
    }
}
