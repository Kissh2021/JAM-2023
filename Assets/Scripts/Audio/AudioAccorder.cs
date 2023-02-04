using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAccorder : MonoBehaviour
{
   eSeason m_currentSeason = eSeason.COUNT;
   private void Update()
   {
      if (SeasonManager.actualSeason != m_currentSeason)
      {
         m_currentSeason = SeasonManager.actualSeason;
         ChangeSeason(m_currentSeason);
      }
   }
   private void ChangeSeason(eSeason season)
   {
      FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SEASON", (int)season);
   }
}
