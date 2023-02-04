using UnityEngine;

public class ParticleTransitionner : MonoBehaviour
{
	[SerializeField] private Vector2 rateOverTimeMinMax;
	[SerializeField, Range(0f, 1f)] private float spawnRate = 1f;

	public float SpawnRate
	{
		set
		{
			spawnRate = value;
			UpdateSystem();
		}
		get => spawnRate;
	}

	private void Awake()
	{
		var emission = GetComponent<ParticleSystem>().emission;
		rateOverTimeMinMax.x = emission.rateOverTime.constantMin;
		rateOverTimeMinMax.y = emission.rateOverTime.constantMax;
		
	}

	void Start()
	{
		UpdateSystem();
	}

	private void OnDrawGizmos()
	{
		UpdateSystem();
	}

	void UpdateSystem()
	{
		var emission = GetComponent<ParticleSystem>().emission;
		var rateOverTime = emission.rateOverTime;
		rateOverTime.constantMin = rateOverTimeMinMax.x * spawnRate;
		rateOverTime.constantMax = rateOverTimeMinMax.y * spawnRate;
		emission.rateOverTime = rateOverTime;
	}
}