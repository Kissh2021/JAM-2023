using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SplineFollower : MonoBehaviour
{
	public bool test;
	public PositionTester tester;
	public float timeBetweenNodes = 0.5f;
	public int precision = 60;
	public float cosRatio = 15f;
	public float cosLength = 0.25f;

	private void Update()
	{
		if (!test)
			return;
		test = false;

		StartCoroutine(LerpOnPos());
	}

	private IEnumerator LerpOnPos()
	{
		List<Vector3> positions = tester.GetPositions();
		Vector3[] allPos = Generics.CurveTools.SplineCatmullRom(positions.ToArray(), precision);

		var lerpValue = 0f;
		float lerpTime = positions.Count / (1f / timeBetweenNodes);

		while (lerpValue <= lerpTime)
		{
			int currentIndex = (int) (lerpValue / lerpTime * (allPos.Length - 1));
			var direction = (currentIndex == 0
				? allPos[0] - allPos[currentIndex + 1]
				: allPos[currentIndex - 1] - allPos[currentIndex]).normalized;


			var newPos = allPos[currentIndex]
			             + Vector3.Cross(direction, Vector3.forward)
			             * (Mathf.Sin(lerpValue * cosRatio) * cosLength);

			transform.position = newPos;
			lerpValue += Time.deltaTime;

			yield return 0;
		}
	}
}