using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTransformer : MonoBehaviour
{
    [SerializeField] Color baseColor;
    [SerializeField] Color autumnColor;
    [SerializeField] Color winterColor;
    [SerializeField] Color summerColor;

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SeasonManager.seasonManager.OnSeasonChange += UpdateColor;
    }
    void UpdateColor(eSeason newSeason)
    {
        switch(newSeason)
        {
            case eSeason.SPRING:
                spriteRenderer.color = baseColor;
                break;
            case eSeason.SUMMER:
                spriteRenderer.color = summerColor;
                break;
            case eSeason.AUTUMN:
                spriteRenderer.color = autumnColor;
                break;
            case eSeason.WINTER:
                spriteRenderer.color = winterColor;
                break;
        }
    }
    private void OnDestroy()
    {
        SeasonManager.seasonManager.OnSeasonChange -= UpdateColor;
    }
}
