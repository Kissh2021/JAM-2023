using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[
    RequireComponent(typeof(Branch))
]
public class Trunk : MonoBehaviour
{
    [SerializeField] private int m_firstGenerationLoops = 3;

    [Space]
    [SerializeField] private float m_branchLength = 1f;
    [SerializeField] private float m_branchWidth = 1f;
    [SerializeField] private float m_branchAngle = 0f;
    [SerializeField] private float m_branchWidthDecreaseFactor = 0.5f;
    [SerializeField] private float m_branchLengthDecreaseFactor = 0.5f;
     
    private Branch m_branch;
    private LineRenderer m_lineRenderer;

    void GenerateRoot(Vector3 origin, float rootLength, float rootWidth, float rootAngle, Vector3 offset,
        float widthDecreaseFactor, int generations)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        m_branch = GetComponent<Branch>();
        m_lineRenderer = GetComponent<LineRenderer>();
        
        //delay the first generation of branches
        //Invoke(nameof(GenerateFirstGeneration), 1f);
        m_branch.GenerateChildBranch(m_lineRenderer.GetPosition(1), m_branchLength, m_branchAngle, m_lineRenderer.endWidth, m_branchWidthDecreaseFactor, m_branchLengthDecreaseFactor, m_firstGenerationLoops);
    }

    // Update is called once per frame
    void Update()
    {
    }
}