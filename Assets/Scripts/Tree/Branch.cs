using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BranchType
{
    Large,
    MediumLarge,
    Medium,
    LittleMedium,
    Little
}

public class Branch : MonoBehaviour
{
    public BranchType branchType;

    [SerializeField] private List<GameObject> branchChlidrenPossibilityList;

    public Vector3 startPos;
    public Transform endPosTransform;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        
        //Debug.Log(FindPath()[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewBranch()
    {
        
    }
    
    public List<Vector3> FindPath()
    {
        List<Vector3> pathResult = new List<Vector3>();
        GameObject nextObject = gameObject;
        pathResult.Add(nextObject.GetComponent<Branch>().startPos);
        
        while (nextObject.transform.parent != null)
        {
            nextObject = nextObject.transform.parent.gameObject;
            pathResult.Add(nextObject.GetComponent<Branch>().startPos);
        }
        
        return pathResult;
    }
}
