using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    /// <summary>
    /// the elementspawners prefabs
    /// </summary>
    [SerializeField] List<GameObject> springSpawners;
    [SerializeField] List<GameObject> summerSpawners;
    [SerializeField] List<GameObject> autumnSpawners;
    [SerializeField] List<GameObject> winterSpawners;
    static public eSeason actualSeason { get; set; }
    private bool IsRaining
    {
        get => isRaining;
        set
        {
            if (isRaining != isRainingLAST)
            {
                isRaining = value;
                OnRainChange.Invoke(IsRaining);
            }
            isRainingLAST = isRaining;
        }
    }

    float timerSeason;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        actualSeason = eSeason.SPRING;

        // Particles
        // Set all the particles to 0
        foreach (GameObject particle in particles)
        {
            particle.GetComponent<SeasonTransitionner>().SpawnRate = 0;
        }

        // set the actual particle to 1
        actualParticle = particles[(int)actualSeason];
        actualParticle.GetComponent<SeasonTransitionner>().SpawnRate = 1;

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
            switch (actualSeason)
            {
                case eSeason.SPRING:
                    SeasonChange(summerSpawners);
                    break;
                case eSeason.SUMMER:
                    SeasonChange(autumnSpawners);
                    break;
                case eSeason.AUTUMN:
                    SeasonChange(winterSpawners);
                    break;
                case eSeason.WINTER:
                    SeasonChange(springSpawners);
                    break;
            }

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
    private void SeasonChange(List<GameObject> _spawners)
    {
        // Update actualSeason
        actualSeason = actualSeason + 1;
        if (actualSeason == eSeason.COUNT)
        {
            actualSeason = eSeason.SPRING;
        }
        Debug.Log(actualSeason.ToString());
        backgroundManager.ProcFade();

        // Particles 
        actualParticle.GetComponent<SeasonTransitionner>().SpawnRate = 0;
        actualParticle = particles[(int)actualSeason];
        actualParticle.GetComponent<SeasonTransitionner>().SpawnRate = 1;

        // Nutrients
        Destroy(actualNutrientSpawner.gameObject);
        actualNutrientSpawner = Instantiate(NutrientsSpawners[(int)actualSeason], nutrientTransform);

        // TODO :
        // clear les gameobjects déjà spawned
        foreach (GameObject spawner in ActualSpawners)
        {
            Destroy(spawner);
        }
        ActualSpawners.Clear();
        // re spawn de nouvelles plantes 
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (_spawners.Count != 0)
            {
                GameObject prefab = _spawners[UnityEngine.Random.Range(0, _spawners.Count)];
                if (prefab != null)
                {
                    GameObject newSpawner = Instantiate(prefab, spawnPoint);
                    AddSpawnerToList(newSpawner);
                }
            }
        }
    }
    private void SpawnsRainSpawners(bool _rainStarts)
    {
        if (_rainStarts)
        {
            //spawn spawners
            foreach (Transform spawnPoint in rainSpawnPoints)
            {
                GameObject newSpawner = Instantiate<GameObject>(rainSpawnerPrefab, spawnPoint);
                newSpawner.transform.parent = spawnPoint;
            }
        }
    }
}
