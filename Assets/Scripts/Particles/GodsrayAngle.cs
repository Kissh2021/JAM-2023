using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodsrayAngle : MonoBehaviour
{
	[SerializeField, Range(-45f, 45f)] private float angle = 30f;

	public float Angle
	{
		set
		{
			angle = value;
			UpdateSystem();
		}
		get => angle;
	}

	void Start()
	{
		UpdateSystem();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector2.zero, Quaternion.Euler(0f, 0f, -angle) * Vector2.down);

		UpdateSystem();
	}

	void UpdateSystem()
	{
		var main = GetComponent<ParticleSystem>().main;
		var rot = main.startRotationZ;
		rot.constant = angle * Mathf.Deg2Rad;
		main.startRotationZ = rot;
	}
}