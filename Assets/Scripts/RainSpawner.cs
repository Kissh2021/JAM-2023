using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    private void Start()
    {
        SeasonManager.seasonManager.OnRainChange += OnRainChange;
    }

    private void OnRainChange(bool _rainStarts)
    {
        if (!_rainStarts)
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SeasonManager.seasonManager.OnRainChange -= OnRainChange;
    }
}
