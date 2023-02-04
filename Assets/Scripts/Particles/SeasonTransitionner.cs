using UnityEngine;

public class SeasonTransitionner : MonoBehaviour
{
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
		var transitionners = GetComponentsInChildren<ParticleTransitionner>();
		foreach (var transitionner in transitionners)
		{
			transitionner.SpawnRate = spawnRate;
		}
	}
}