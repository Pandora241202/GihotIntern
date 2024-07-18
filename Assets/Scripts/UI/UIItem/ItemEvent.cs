using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemEvent : MonoBehaviour
{
   [SerializeField] private Image imgProgress;

   public void OnSetUp()
   {
      imgProgress.fillAmount = 0;
   }

   public void OnUpdateFill(float fillA)
   {
      imgProgress.fillAmount = fillA;
   }
}
