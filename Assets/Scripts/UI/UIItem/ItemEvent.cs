using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemEvent : MonoBehaviour
{
   [SerializeField] private Image imgProgress;
   public float duration;
   public void OnSetUp(int durationEv)
   {
      imgProgress.fillAmount = 0;
      duration = durationEv;
   }

   public void OnUpdateFill(float fillA)
   {
      imgProgress.fillAmount = 1-fillA/duration;
   }
}
