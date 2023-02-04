using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
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
    private static SeasonManager instance;
    public static SeasonManager seasonManager { get => instance; }

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
    [SerializeField] List<GameObject> springSpawners;
    [SerializeField] List<GameObject> summerSpawners;
    [SerializeField] List<GameObject> autumnSpawners;
    [SerializeField] List<GameObject> winterSpawners;

    [SerializeField] float seasonDuration;

    [Category("Rain")]
    [SerializeField] GameObject rainSpawnerPrefab;
    [SerializeField] List<Transform> rainSpawnPoints;
    [SerializeField] bool isRaining;
    bool isRainingLAST;
    public Action<bool> OnRainChange;

    /// <summary>
    /// the elementspawners already spawned
    /// </summary>
    public List<GameObject> ActualSpawners;
    static public eSeason actualSeason { get; set; }
    private bool IsRaining { get => isRaining; 
        set {
            if (isRaining != isRainingLAST)
            {
                Debug.Log(value);
                isRaining = value;
                OnRainChange.Invoke(IsRaining);
            }
            isRainingLAST = isRaining;
        } }

    float timerSeason;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        spawnPoints = new List<Transform>();
        actualSeason = eSeason.SPRING;
        timerSeason = 0f;

        OnRainChange += SpawnsRainSpawners;
    }
    private void Update()
    {
        timerSeason += Time.deltaTime;
        if (timerSeason >= seasonDuration)
        {
            SeasonChange();
            timerSeason = 0f;
        }
        IsRaining = isRaining;
    }
    private void AddSpawnerToList(GameObject _spawner)
    {
        ActualSpawners.Add(_spawner);
    }
    /// <summary>
    /// Do things when season change
    /// </summary>
    private void SeasonChange()
    {
        // Update actualSeason
        actualSeason = actualSeason + 1;
        if (actualSeason == eSeason.COUNT)
        {
            actualSeason = eSeason.SPRING;
        }
        Debug.Log(actualSeason.ToString());



        // TODO :
        // clear les gameobjects d�j� spawned si besoin
        // re spawn de nouvelles plantes si le spawner est vide
        // changer les particules (+1 sur la list)
    }
    private void SpawnsRainSpawners(bool _rainStarts)
    {
        if (_rainStarts)
        {
            //spawn spawners
            foreach(Transform spawnPoint in rainSpawnPoints)
            {
                GameObject newSpawner = Instantiate<GameObject>(rainSpawnerPrefab, spawnPoint);
                newSpawner.transform.parent = spawnPoint;
            }
        }
    }
}
