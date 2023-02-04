using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSeason
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER,
    COUNT
}
public enum eSpawnerPrefabs
{
    PREFAB1,
    PREFAB2,
    COUNT
}

public class SeasonManager : MonoBehaviour
{
    /// <summary>
    /// List of particles depending on season (use the enum)
    /// </summary>
    [SerializeField] List<ParticleSystem> particles;
    /// <summary>
    /// Where the elementspawners will spawn
    /// </summary>
    [SerializeField] List<Transform> spawnPoints;

    /// <summary>
    /// the elementspawners prefabs
    /// </summary>
    [SerializeField] List<GameObject> prefabs;

    /// <summary>
    /// the elementspawners already spawned
    /// </summary>
    public List<GameObject> gameObjects;
    static public eSeason actualSeason { get; set; }

    private void Start()
    {
        spawnPoints = new List<Transform>();
        actualSeason = eSeason.SPRING;
    }

    /// <summary>
    /// Do things when season change
    /// </summary>
    private void SeasonChange()
    {
        // TODO :
        // clear les gameobjects déjà spawned si besoin
        // re spawn de nouvelles plantes si le spawner est vide
        // changer les particules (+1 sur la list)
    }
}
