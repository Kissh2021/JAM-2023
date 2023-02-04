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

    [Space(20), Header("Particles")]
    /// <summary>
    /// List of particles depending on season (use the enum)
    /// </summary>
    [SerializeField] List<GameObject> particles;
    public GameObject actualParticle;


    [Space(20), Header("Nutrients")]
    [SerializeField] List<GameObject> NutrientsSpawners;
    public GameObject actualNutrientSpawner;
    [SerializeField] Transform nutrientTransform;


    [Space(20), Header("Rain")]
    [SerializeField] GameObject rainSpawnerPrefab;
    [SerializeField] List<Transform> rainSpawnPoints;
    [SerializeField] bool isRaining;
    bool isRainingLAST;
    public Action<bool> OnRainChange;

    [Space(20), Header("Others")]
    /// <summary>
    /// the elementspawners already spawned
    /// </summary>
    public List<GameObject> ActualSpawners;
    [SerializeField] BackgroundManager backgroundManager;
    [SerializeField] float seasonDuration;

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
    static public eSeason actualSeason { get; set; }
    private bool IsRaining { get => isRaining; 
        set {
            if (isRaining != isRainingLAST)
            {
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

        // Particles
        actualParticle = Instantiate(particles[(int)actualSeason], transform);

        // Nutrients
        actualNutrientSpawner = Instantiate(NutrientsSpawners[(int)actualSeason], nutrientTransform);

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
        backgroundManager.ProcFade();
        Destroy(actualParticle.gameObject);
        actualParticle = Instantiate(particles[(int)actualSeason], transform);
        Destroy(actualNutrientSpawner.gameObject);
        actualNutrientSpawner = Instantiate(NutrientsSpawners[(int)actualSeason], nutrientTransform);

        // TODO :
        // clear les gameobjects déjà spawned si besoin
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
