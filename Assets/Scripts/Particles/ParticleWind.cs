using UnityEngine;

public class ParticleWind : MonoBehaviour
{
	[SerializeField] private float windDirection;
	[SerializeField] private Vector2 windForceChangeScale = new(2f, 2f);

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
		x.constantMin = windDirection - windForceChangeScale.x;
		x.constantMax = windDirection + windForceChangeScale.x;
		vol.x = x;
		var z = vol.z;
		z.constantMin = Mathf.Abs(windDirection) - windForceChangeScale.y;
		z.constantMax = Mathf.Abs(windDirection) + windForceChangeScale.y;
		vol.z = z;

		var pos = transform.position;
		pos.x = -windDirection / 2f;
		transform.position = pos;
	}
}