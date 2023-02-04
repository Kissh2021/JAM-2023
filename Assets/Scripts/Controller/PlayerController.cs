using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Dragable currentlyDraggedObject = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyDraggedObject != null)
        {
            currentlyDraggedObject.Drag(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Clickable clickable;
            foreach (var hit in hits)
            {
                if (currentlyDraggedObject == null)
                {
                    if (hit.collider.gameObject.TryGetComponent<Clickable>(out clickable))
                    {
                        clickable.Click();
                    }
                    if (hit.collider.gameObject.TryGetComponent<Dragable>(out currentlyDraggedObject))
                    {
                        currentlyDraggedObject.BeginDrag();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            if(currentlyDraggedObject != null)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                foreach (var hit in hits)
                {
                    DragTarget target;
                    if(hit.collider.gameObject.TryGetComponent<DragTarget>(out target))
                    {
                        target.DragReceive(currentlyDraggedObject);
                        currentlyDraggedObject = null;
                    }
                }
                if (currentlyDraggedObject != null)
                {
                    currentlyDraggedObject.Drop();
                    currentlyDraggedObject = null;
                }

            }
        }
    }
}
