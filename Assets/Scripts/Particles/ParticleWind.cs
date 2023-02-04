using UnityEngine;

public class ParticleWind : MonoBehaviour
{
	[SerializeField] private float windDirection;
	[SerializeField] private float windForceChangeScale = 2f;

	public float WindDirection
	{
		set
		{
			windDirection = value;
			UpdateSystem();
		}
		get => windDirection;
	}

	void Start()
	{
		UpdateSystem();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector2.zero, new Vector2(windDirection, -Mathf.Abs(windDirection)));

		UpdateSystem();
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
	}
}