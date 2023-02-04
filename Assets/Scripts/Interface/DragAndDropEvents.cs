using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class DragAndDropEvents
{
    public static Action<Dragable> OnBeginDrag;
    public static Action OnEndDrag;
}
