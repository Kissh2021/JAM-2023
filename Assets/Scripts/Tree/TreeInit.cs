using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BranchGenerator))]
public class TreeInit : MonoBehaviour
{
    private BranchGenerator m_branch;
    [SerializeField] public GameObject branchPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        m_branch = GetComponent<BranchGenerator>();
        
        m_branch.GenerateBranch(30f, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
