using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] Texture2D[] seasonsBackground;
    [SerializeField] float fadeSpeed = 1f;
    float range = 0f;
    private bool isFading = false;
    private eSeason currentSeason;
    Material mat;

    [ContextMenu("ProcFade")]
    public void ProcFade()
    {
        mat.SetTexture("_FirstTex", seasonsBackground[(int)currentSeason]);
        mat.SetTexture("_SecondTex", seasonsBackground[((int)currentSeason + 1) % (int)eSeason.COUNT]);
        isFading = true;
    }

    private void Start()
    {
        List<Material> mats = new List<Material>();
        gameObject.GetComponent<SpriteRenderer>().GetMaterials(mats);
        mat = mats[0];
        currentSeason = SeasonManager.actualSeason;

        mat.SetTexture("_FirstTex", seasonsBackground[(int)currentSeason]);
        mat.SetTexture("_SecondTex", seasonsBackground[((int)currentSeason + 1) % (int)eSeason.COUNT]);
    }

    void Fade()
    {
        range += fadeSpeed * Time.deltaTime;
        range = Mathf.Clamp01(range);
        mat.SetFloat("_Range", range);
        if (range >= 1f)
        { 
            range = 0f;
            currentSeason = (eSeason)(((int)currentSeason + 1) % (int)eSeason.COUNT);
            mat.SetTexture("_FirstTex", seasonsBackground[(int)currentSeason]);
            isFading = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isFading)
            Fade();
    }
}
