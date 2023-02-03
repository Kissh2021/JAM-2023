using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    public List<Element> elements;

    private void Start()
    {
        elements = new List<Element>();
    }

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
        int elemToSpawn = Random.Range(0, elements.Count + 1);

        if (elements[elemToSpawn] != null)
        {
            Element newElem = Instantiate<Element>(elements[elemToSpawn], transform);
        }
    }
}
