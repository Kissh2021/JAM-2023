using System;
using UnityEngine;

public class ParticleWind : MonoBehaviour
{
	[SerializeField, Range(-10f, 10f)] private float windDirection;
	[SerializeField] private float windForceChangeScale = 2f;
	[SerializeField] private bool rotateParticleToFaceDirection = false;

	public float WindDirection
	{
		set
		{
			windDirection = value;
			UpdateSystem();
		}
		get => windDirection;
	}

	private void Awake()
	{
		WindManager.OnWindValueChanged += UpdateWind;
	}

	void Start()
	{
		UpdateSystem();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector2.zero, new Vector2(WindDirection, -Mathf.Abs(windDirection)));

		UpdateSystem();
	}

	private void OnDestroy()
	{
		WindManager.OnWindValueChanged -= UpdateWind;
	}

	void UpdateWind(float windValue)
	{
		WindDirection = windValue;
	}

	void UpdateSystem()
	{
		var vol = GetComponent<ParticleSystem>().velocityOverLifetime;
		var x = vol.x;
		x.constantMin = windDirection - windForceChangeScale;
		x.constantMax = windDirection + windForceChangeScale;
		vol.x = x;
		var z = vol.z;
		z.constantMin = Mathf.Abs(windDirection) - windForceChangeScale;
		z.constantMax = Mathf.Abs(windDirection) + windForceChangeScale;
		vol.z = z;

		var pos = transform.position;
		pos.x = -windDirection / 2f;
		transform.position = pos;

		if (!rotateParticleToFaceDirection) 
			return;
		
		var main = GetComponent<ParticleSystem>().main;
		var rot = main.startRotationZ;
		rot.constant = -windDirection * Mathf.Deg2Rad;
		main.startRotationZ = rot;
	}
}