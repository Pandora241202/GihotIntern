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
    public Dictionary<int,GameObject> lsGOEvent = new Dictionary<int, GameObject>();
    public GameObject goTextEvent;
    public bool isShowText = false;
    public GameObject goWaiting;

    public void OnSetUp(float maxHealth, float maxExp)
    {
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        sliderLevel.maxValue = maxExp;
        sliderLevel.value = 0;
        txtPing.text = "0ms";
        goTextEvent.SetActive(false);
        // txtTimeEvent.text = "0s";
        // txtTimeEvent.gameObject.SetActive(false);
        txtLevel.text = Constants.PlayerBaseLevel.ToString();
    }

    public void OnPause_Clicked()
    {
        UIManager._instance.PlaySfx(0);
        SendData<PauseEvent> data = new SendData<PauseEvent>(new PauseEvent());
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        sliderHealth.maxValue = AllManager._instance.playerManager.GetMaxHealthFromLevel();
        sliderHealth.value = currentHealth;
    }

    public void ShowText()
    {
        isShowText = !isShowText;
        if (isShowText)
        {
            goTextEvent.SetActive(true);
            
            goTextEvent.transform.DOScale(Vector3.zero, 2f).OnComplete(() =>
            {
                goTextEvent.transform.localScale=Vector3.one;
                goTextEvent.SetActive(false);
            });
        }
        
    }
    public void OnEventStart(int idEvent, int duration)
    {
        GameObject goItem = Instantiate(itemEvent.gameObject, goSpawnEvent.transform);
        goItem.transform.position = goSpawnEvent.transform.position;
        goItem.transform.localScale = new Vector3(.3f, .3f, .3f);
        goItem.GetComponent<ItemEvent>().OnSetUp(idEvent,duration);

        Vector3 targetPosition = goEvent.transform.position;

        goItem.transform.DOMove(targetPosition, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            Debug.Log(idEvent);
            goItem.transform.SetParent(goEvent.transform);
            goItem.transform.localScale = Vector3.one;

            lsGOEvent.Add(idEvent,goItem);
        });
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