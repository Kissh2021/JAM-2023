using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTester : MonoBehaviour
{
	public List<Vector3> GetPositions()
	{
		List<Vector3> result = new List<Vector3>();

		for (int i = 0; i < transform.childCount; i++)
		{
			result.Add(transform.GetChild(i).transform.position);
		}

		return result;
	}
}