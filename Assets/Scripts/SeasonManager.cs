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

    [SerializeField] float seasonDuration;

    /// <summary>
    /// the elementspawners already spawned
    /// </summary>
    public List<GameObject> ActualSpawners;
    static public eSeason actualSeason { get; set; }
    float timerSeason;
    private void Start()
    {
        spawnPoints = new List<Transform>();
        actualSeason = eSeason.SPRING;
        timerSeason = 0f;
    }
    private void Update()
    {
        timerSeason += Time.deltaTime;
        if (timerSeason >= seasonDuration)
        {
            SeasonChange();
            timerSeason = 0f;
        }
    }

    /// <summary>
    /// Do things when season change
    /// </summary>
    private void SeasonChange()
    {
        Debug.Log(actualSeason.ToString());
        // TODO :
        // clear les gameobjects déjà spawned si besoin
        // re spawn de nouvelles plantes si le spawner est vide
        // changer les particules (+1 sur la list)
    }
}
