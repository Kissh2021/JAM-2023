using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Branch : MonoBehaviour
{
    public void GenerateChildBranch(Vector3 startPosition, float length, float angle, float width, float widthDecreaseFactor,
        float lengthDecreaseFactor, int generations)
    {
        if (generations <= 0)
        {
            return;
        }

        var branch = new GameObject("Branch")
        {
            transform =
            {
                position = startPosition,
                parent = transform
            }
        };
        
        //transform angle to radians
        var newAngle = angle * Mathf.Deg2Rad;
        
        var endPosition = startPosition + (Vector3.up * Mathf.Sin(newAngle / 2) + Vector3.right * Mathf.Cos(newAngle / 2)) * length;
        
        var lineRenderer = branch.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width * widthDecreaseFactor;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        branch.transform.Rotate(Vector3.forward, angle);

        var childBranch = branch.AddComponent<Branch>();
        childBranch.GenerateChildBranch(lineRenderer.GetPosition(1), length * lengthDecreaseFactor, angle - angle * 9.5f / 10, width * widthDecreaseFactor,
            widthDecreaseFactor, lengthDecreaseFactor, generations - 1);
        
        branch = new GameObject("Branch")
        {
            transform =
            {
                position = startPosition,
                parent = transform
            }
        };
        
        endPosition = startPosition + (Vector3.up * Mathf.Sin(newAngle / 2) - Vector3.right * Mathf.Cos(newAngle / 2)) * length;
        
        lineRenderer = branch.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width * widthDecreaseFactor;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        branch.transform.Rotate(Vector3.forward, angle);

        childBranch = branch.AddComponent<Branch>();
        childBranch.GenerateChildBranch(lineRenderer.GetPosition(1), length * lengthDecreaseFactor, angle - angle * 9.5f / 10, width * widthDecreaseFactor,
            widthDecreaseFactor, lengthDecreaseFactor, generations - 1);
    }
}