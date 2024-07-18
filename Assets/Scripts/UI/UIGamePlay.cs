using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIGamePlay : MonoBehaviour
{
    public Slider sliderHealth;
    public Slider sliderLevel;
    public TextMeshProUGUI txtLevel;
    [SerializeField] private Button btnPause;
    [SerializeField] private TextMeshProUGUI txtPing;
    public GameObject goEvent;
    public ItemEvent itemEvent;
    public GameObject goSpawnEvent;
    public List<GameObject> lsGOEvent = new List<GameObject>();
    public void OnSetUp(float maxHealth, float maxExp)
    {
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth; 
        sliderLevel.maxValue = maxExp; 
        sliderLevel.value = 0;
        txtPing.text = "0ms";
        // txtTimeEvent.text = "0s";
        // txtTimeEvent.gameObject.SetActive(false);
        txtLevel.text = Constants.PlayerBaseLevel.ToString();
    }

    public void OnPause_Clicked()
    {
        SendData<PauseEvent> data = new SendData<PauseEvent>(new PauseEvent());
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        sliderHealth.maxValue = AllManager._instance.playerManager.GetMaxHealthFromLevel();
        sliderHealth.value = currentHealth;
    }

  public void OnEventStart(int count, int idEvent,int duration)
  {
      for (int i = 0; i < count; i++)
      {
          GameObject goItem = Instantiate(itemEvent.gameObject, goSpawnEvent.transform);
          goItem.transform.position = goSpawnEvent.transform.position;
          goItem.transform.localScale = new Vector3(.3f, .3f, .3f);
          goItem.GetComponent<ItemEvent>().OnSetUp(duration);
          
          Vector3 targetPosition = goEvent.transform.position;
          
          goItem.transform.DOMove(targetPosition, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
          {
              goItem.transform.SetParent(goEvent.transform);
              goItem.transform.localScale = Vector3.one;
              
              lsGOEvent.Add(goItem);
              
          });
      }
  }


    public void SetHealthSliderValue(float currentHealth)
    {
        sliderHealth.value = currentHealth;
    }
    public void UpdatePingText(long ping)
    {
        if (ping < 50) txtPing.color = Color.green;
        else if (ping < 80) txtPing.color = Color.yellow;
        else txtPing.color = Color.red;
        txtPing.text = ping.ToString() + "ms";
    }

    public void UpdateLevelSlider(float expProgress)
    {
        sliderLevel.value = expProgress; 
    }

    public void LevelUpdateSlider(float expRequire)
    {
        sliderLevel.maxValue = expRequire;
        txtLevel.text = AllManager._instance.playerManager.level.ToString();
        sliderLevel.value = 0;
        sliderHealth.maxValue = AllManager._instance.playerManager.GetMaxHealthFromLevel();
        sliderHealth.value = sliderHealth.maxValue;
    }

    public void ChangeSliderEvent(int maxHP)
    {
        sliderHealth.maxValue = maxHP;
    }
}