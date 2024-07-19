using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemEvent : MonoBehaviour
{
   [SerializeField] private Image imgProgress;
   [SerializeField] private Image imgIcon;
   [SerializeField] private List<Sprite> lsSprite = new List<Sprite>();
   public GameObject goRemainPoint;
   public TextMeshProUGUI txtPoint;
   public float duration;
   public void OnSetUp(int idEvent,int durationEv)
   {
      imgIcon.sprite = lsSprite[idEvent];
      imgProgress.fillAmount = 0;
      duration = durationEv;
   }

   public void OnUpdateFill(float fillA)
   {
      imgProgress.fillAmount = 1-fillA/duration;
   }

   public void UpdateScoreQTE(int score)
   {
      txtPoint.text = "Remaining: " + score;
   }
}
