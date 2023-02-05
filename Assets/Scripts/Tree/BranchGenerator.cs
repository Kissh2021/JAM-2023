using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BranchGenerator : MonoBehaviour
{
    public Vector3 startPos;
    [SerializeField] private Transform endPosTransform;
    public Vector3 scaleDecrease;

    public void GenerateBranch(float angle, int generations)
    {
        Debug.Log("Yo");
        if (generations <= 0)
            return;

        Vector3 rotation = Vector3.zero;
        rotation.z = angle;

        Vector3 endPos = endPosTransform.position;

        GameObject branch = GetComponentInParent<TreeInit>().branchPrefab;

        var nextBranchRight = Instantiate(branch, endPos, Quaternion.identity);
        nextBranchRight.transform.parent = transform;
        nextBranchRight.GetComponent<BranchGenerator>().startPos = endPos;

        nextBranchRight.transform.localRotation = Quaternion.Euler(0,0,angle);

        nextBranchRight.transform.localScale = scaleDecrease;
        nextBranchRight.GetComponent<BranchGenerator>().scaleDecrease = scaleDecrease;

        var nextBranchLeft = Instantiate(branch, endPos, Quaternion.identity);
        nextBranchLeft.transform.parent = transform;
        nextBranchLeft.GetComponent<BranchGenerator>().startPos = endPos;

        nextBranchLeft.transform.localRotation = Quaternion.Euler(0,0,-angle);

        nextBranchLeft.transform.localScale = scaleDecrease;
        nextBranchLeft.GetComponent<BranchGenerator>().scaleDecrease = scaleDecrease;
        
        nextBranchRight.GetComponent<BranchGenerator>().GenerateBranch(angle, generations - 1);
        nextBranchLeft.GetComponent<BranchGenerator>().GenerateBranch(angle, generations - 1);
    }
}
