using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    [SerializeField] float SpawnRate;
    public List<Element> elements;
    float timer = 0;


    /// <summary>
    /// Add an element to the list so it can spawns
    /// </summary>
    /// <param name="element"></param>
    public void AddElement(Element element)
    {
        elements.Add(element);
    }

    /// <summary>
    /// Randomly spawns an element from his list at his location
    /// </summary>
    public void Spawn()
    {
        int elemToSpawn = Random.Range(0, elements.Count);

        if (elements[elemToSpawn] != null)
        {
            Element newElem = Instantiate<Element>(elements[elemToSpawn], transform);
            newElem.transform.parent = transform;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnRate)
        {
            Spawn();
            timer = 0;
        }
    }
}
